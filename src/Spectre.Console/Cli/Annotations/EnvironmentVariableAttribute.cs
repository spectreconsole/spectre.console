#nullable enable

using System;
using Spectre.Console.Cli;

namespace Spectre.Console
{
    internal sealed class EnvironmentVariableAttribute : ParameterValueProviderAttribute
    {
        private readonly string _name;

        public EnvironmentVariableAttribute(string name)
        {
            _name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public override bool TryGetValue(CommandParameterContext context, out object? result)
        {
            result = null;

            if (context.Value == null)
            {
                var valStr = Environment.GetEnvironmentVariable(_name);

                if (valStr is null)
                {
                    return false;
                }

                if (TypeConverterHelper.TryConvertFromString(valStr, context.Parameter.ParameterType, out object? convertedResult))
                {
                    result = convertedResult;
                    return true;
                }
            }

            return false;
        }
    }
}
