using System;
using System.Collections.Generic;
using System.Reflection;

namespace Spectre.Console.Cli
{
    internal static class CommandConstructorBinder
    {
        public static CommandSettings CreateSettings(CommandValueLookup lookup, ConstructorInfo constructor, ITypeResolver resolver)
        {
            if (constructor.DeclaringType == null)
            {
                throw new InvalidOperationException("Cannot create settings since constructor have no declaring type.");
            }

            var parameters = new List<object?>();
            var mapped = new HashSet<Guid>();
            foreach (var parameter in constructor.GetParameters())
            {
                if (lookup.TryGetParameterWithName(parameter.Name, out var result))
                {
                    parameters.Add(result.Value);
                    mapped.Add(result.Parameter.Id);
                }
                else
                {
                    var value = resolver.Resolve(parameter.ParameterType);
                    if (value == null)
                    {
                        throw CommandRuntimeException.CouldNotResolveType(parameter.ParameterType);
                    }

                    parameters.Add(value);
                }
            }

            // Create the settings.
            if (!(Activator.CreateInstance(constructor.DeclaringType, parameters.ToArray()) is CommandSettings settings))
            {
                throw new InvalidOperationException("Could not create settings");
            }

            // Try to do property injection for parameters that wasn't injected.
            foreach (var (parameter, value) in lookup)
            {
                if (!mapped.Contains(parameter.Id) && parameter.Property.SetMethod != null)
                {
                    parameter.Property.SetValue(settings, value);
                }
            }

            return settings;
        }
    }
}
