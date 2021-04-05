using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Spectre.Console.Cli
{
    internal static class CommandValueResolver
    {
        public static CommandValueLookup GetParameterValues(CommandTree? tree, ITypeResolver resolver)
        {
            var lookup = new CommandValueLookup();
            var binder = new CommandValueBinder(lookup);

            CommandValidator.ValidateRequiredParameters(tree);

            while (tree != null)
            {
                // Process unmapped parameters.
                foreach (var parameter in tree.Unmapped)
                {
                    if (parameter.IsFlagValue())
                    {
                        // Set the flag value to an empty, not set instance.
                        var instance = Activator.CreateInstance(parameter.ParameterType);
                        lookup.SetValue(parameter, instance);
                    }
                    else
                    {
                        // Is this an option with a default value?
                        if (parameter.DefaultValue != null)
                        {
                            var value = parameter.DefaultValue?.Value;

                            // Need to convert the default value?
                            if (value != null && value.GetType() != parameter.ParameterType)
                            {
                                var converter = GetConverter(lookup, binder, resolver, parameter);
                                if (converter != null)
                                {
                                    value = converter.ConvertFrom(value);
                                }
                            }

                            binder.Bind(parameter, resolver, value);
                            CommandValidator.ValidateParameter(parameter, lookup);
                        }
                        else if (Nullable.GetUnderlyingType(parameter.ParameterType) != null ||
                                 !parameter.ParameterType.IsValueType)
                        {
                            lookup.SetValue(parameter, null);
                        }
                    }
                }

                // Process mapped parameters.
                foreach (var mapped in tree.Mapped)
                {
                    if (mapped.Parameter.WantRawValue)
                    {
                        // Just try to assign the raw value.
                        binder.Bind(mapped.Parameter, resolver, mapped.Value);
                    }
                    else
                    {
                        var converter = GetConverter(lookup, binder, resolver, mapped.Parameter);
                        if (converter == null)
                        {
                            throw CommandRuntimeException.NoConverterFound(mapped.Parameter);
                        }

                        if (mapped.Parameter.IsFlagValue() && mapped.Value == null)
                        {
                            if (mapped.Parameter is CommandOption option && option.DefaultValue != null)
                            {
                                // Set the default value.
                                binder.Bind(mapped.Parameter, resolver, option.DefaultValue?.Value);
                            }
                            else
                            {
                                // Set the flag but not the value.
                                binder.Bind(mapped.Parameter, resolver, null);
                            }
                        }
                        else
                        {
                            // Assign the value to the parameter.
                            binder.Bind(mapped.Parameter, resolver, converter.ConvertFromInvariantString(mapped.Value));
                        }
                    }

                    CommandValidator.ValidateParameter(mapped.Parameter, lookup);
                }

                tree = tree.Next;
            }

            return lookup;
        }

        [SuppressMessage("Style", "IDE0019:Use pattern matching", Justification = "It's OK")]
        private static TypeConverter? GetConverter(CommandValueLookup lookup, CommandValueBinder binder, ITypeResolver resolver, CommandParameter parameter)
        {
            if (parameter.Converter == null)
            {
                if (parameter.ParameterType.IsArray)
                {
                    // Return a converter for each array item (not the whole array)
                    return TypeDescriptor.GetConverter(parameter.ParameterType.GetElementType());
                }

                if (parameter.IsFlagValue())
                {
                    // Is the optional value instanciated?
                    var value = lookup.GetValue(parameter) as IFlagValue;
                    if (value == null)
                    {
                        // Try to assign it with a null value.
                        // This will create the optional value instance without a value.
                        binder.Bind(parameter, resolver, null);
                        value = lookup.GetValue(parameter) as IFlagValue;
                        if (value == null)
                        {
                            throw new InvalidOperationException("Could not intialize optional value.");
                        }
                    }

                    // Return a converter for the flag element type.
                    return TypeDescriptor.GetConverter(value.Type);
                }

                return TypeDescriptor.GetConverter(parameter.ParameterType);
            }

            var type = Type.GetType(parameter.Converter.ConverterTypeName);
            return resolver.Resolve(type) as TypeConverter;
        }
    }
}
