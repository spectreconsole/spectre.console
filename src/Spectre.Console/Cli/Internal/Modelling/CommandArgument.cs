using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace Spectre.Console.Cli
{
    internal sealed class CommandArgument : CommandParameter
    {
        public string Value { get; }
        public int Position { get; set; }

        public CommandArgument(
            Type parameterType, ParameterKind parameterKind, PropertyInfo property, string? description,
            TypeConverterAttribute? converter, DefaultValueAttribute? defaultValue,
            CommandArgumentAttribute argument, IEnumerable<ParameterValidationAttribute> validators)
                : base(parameterType, parameterKind, property, description, converter, defaultValue,
                      null, validators, argument.IsRequired)
        {
            Value = argument.ValueName;
            Position = argument.Position;
        }
    }
}
