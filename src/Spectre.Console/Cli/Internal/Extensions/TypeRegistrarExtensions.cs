using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Spectre.Console.Cli
{
    internal static class TypeRegistrarExtensions
    {
        public static void RegisterDependencies(this ITypeRegistrar registrar, CommandModel model)
        {
            var stack = new Stack<CommandInfo>();
            model.Commands.ForEach(c => stack.Push(c));
            if (model.DefaultCommand != null)
            {
                stack.Push(model.DefaultCommand);
            }

            while (stack.Count > 0)
            {
                var command = stack.Pop();

                if (command.SettingsType == null)
                {
                    // TODO: Error message
                    throw new InvalidOperationException("Command setting type cannot be null.");
                }

                if (command.CommandType != null)
                {
                    registrar?.Register(typeof(ICommand), command.CommandType);
                    registrar?.Register(command.CommandType, command.CommandType);
                }

                foreach (var parameter in command.Parameters)
                {
                    var pairDeconstructor = parameter?.PairDeconstructor?.Type;
                    if (pairDeconstructor != null)
                    {
                        registrar?.Register(pairDeconstructor, pairDeconstructor);
                    }

                    var typeConverterTypeName = parameter?.Converter?.ConverterTypeName;
                    if (!string.IsNullOrWhiteSpace(typeConverterTypeName))
                    {
                        var typeConverterType = Type.GetType(typeConverterTypeName);
                        Debug.Assert(typeConverterType != null, "Could not create type");
                        registrar?.Register(typeConverterType, typeConverterType);
                    }
                }

                foreach (var child in command.Children)
                {
                    stack.Push(child);
                }
            }
        }
    }
}
