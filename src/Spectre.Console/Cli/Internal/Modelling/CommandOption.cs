using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace Spectre.Console.Cli
{
    internal sealed class CommandOption : CommandParameter
    {
        public IReadOnlyList<string> LongNames { get; }
        public IReadOnlyList<string> ShortNames { get; }
        public string? ValueName { get; }
        public bool ValueIsOptional { get; }
        public bool IsShadowed { get; set; }

        public CommandOption(
            Type parameterType, ParameterKind parameterKind, PropertyInfo property, string? description,
            TypeConverterAttribute? converter, PairDeconstructorAttribute? deconstructor,
            CommandOptionAttribute optionAttribute, IEnumerable<ParameterValidationAttribute> validators,
            DefaultValueAttribute? defaultValue, bool valueIsOptional)
                : base(parameterType, parameterKind, property, description, converter,
                      defaultValue, deconstructor, validators, false)
        {
            LongNames = optionAttribute.LongNames;
            ShortNames = optionAttribute.ShortNames;
            ValueName = optionAttribute.ValueName;
            ValueIsOptional = valueIsOptional;
        }

        public string GetOptionName()
        {
            return LongNames.Count > 0 ? LongNames[0] : ShortNames[0];
        }
    }
}