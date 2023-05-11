namespace Spectre.Console.Cli;

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
                // Got a value provider?
                if (parameter.ValueProvider != null)
                {
                    var context = new CommandParameterContext(parameter, resolver, null);
                    if (parameter.ValueProvider.TryGetValue(context, out var result))
                    {
                        result = ConvertValue(resolver, lookup, binder, parameter, result);

                        lookup.SetValue(parameter, result);
                        CommandValidator.ValidateParameter(parameter, lookup, resolver);
                        continue;
                    }
                }

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
                        value = ConvertValue(resolver, lookup, binder, parameter, value);

                        binder.Bind(parameter, resolver, value);
                        CommandValidator.ValidateParameter(parameter, lookup, resolver);
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
                        var (converter, stringConstructor) = GetConverter(lookup, binder, resolver, mapped.Parameter);
                        if (converter == null)
                        {
                            throw CommandRuntimeException.NoConverterFound(mapped.Parameter);
                        }

                        object? value;
                        var mappedValue = mapped.Value ?? string.Empty;
                        try
                        {
                            try
                            {
                                value = converter.ConvertFromInvariantString(mappedValue);
                            }
                            catch (NotSupportedException) when (stringConstructor != null)
                            {
                                value = stringConstructor.Invoke(new object[] { mappedValue });
                            }
                        }
                        catch (Exception exception) when (exception is not CommandRuntimeException)
                        {
                            throw CommandRuntimeException.ConversionFailed(mapped, converter, exception);
                        }

                        // Assign the value to the parameter.
                        binder.Bind(mapped.Parameter, resolver, value);
                    }
                }

                // Got a value provider?
                if (mapped.Parameter.ValueProvider != null)
                {
                    var context = new CommandParameterContext(mapped.Parameter, resolver, mapped.Value);
                    if (mapped.Parameter.ValueProvider.TryGetValue(context, out var result))
                    {
                        lookup.SetValue(mapped.Parameter, result);
                    }
                }

                CommandValidator.ValidateParameter(mapped.Parameter, lookup, resolver);
            }

            tree = tree.Next;
        }

        return lookup;
    }

    private static object? ConvertValue(ITypeResolver resolver, CommandValueLookup lookup, CommandValueBinder binder, CommandParameter parameter, object? result)
    {
        if (result != null && result.GetType() != parameter.ParameterType)
        {
            var (converter, _) = GetConverter(lookup, binder, resolver, parameter);
            if (converter != null)
            {
                result = result is Array array ? ConvertArray(array, converter) : converter.ConvertFrom(result);
            }
        }

        return result;
    }

    private static Array ConvertArray(Array sourceArray, TypeConverter converter)
    {
        Array? targetArray = null;
        for (var i = 0; i < sourceArray.Length; i++)
        {
            var item = sourceArray.GetValue(i);
            if (item != null)
            {
                var converted = converter.ConvertFrom(item);
                if (converted != null)
                {
                    targetArray ??= Array.CreateInstance(converted.GetType(), sourceArray.Length);
                    targetArray.SetValue(converted, i);
                }
            }
        }

        return targetArray ?? sourceArray;
    }

    [SuppressMessage("Style", "IDE0019:Use pattern matching", Justification = "It's OK")]
    private static (TypeConverter? Converter, ConstructorInfo? StringConstructor) GetConverter(CommandValueLookup lookup, CommandValueBinder binder, ITypeResolver resolver, CommandParameter parameter)
    {
        static ConstructorInfo? GetStringConstructor(Type type)
        {
            var constructor = type.GetConstructor(BindingFlags.Public | BindingFlags.Instance, null, new[] { typeof(string) }, null);
            return constructor?.GetParameters()[0].ParameterType == typeof(string) ? constructor : null;
        }

        if (parameter.Converter == null)
        {
            if (parameter.ParameterType.IsArray)
            {
                // Return a converter for each array item (not the whole array)
                var elementType = parameter.ParameterType.GetElementType();
                if (elementType == null)
                {
                    throw new InvalidOperationException("Could not get element type");
                }

                return (TypeDescriptor.GetConverter(elementType), GetStringConstructor(elementType));
            }

            if (parameter.IsFlagValue())
            {
                // Is the optional value instantiated?
                var value = lookup.GetValue(parameter) as IFlagValue;
                if (value == null)
                {
                    // Try to assign it with a null value.
                    // This will create the optional value instance without a value.
                    binder.Bind(parameter, resolver, null);
                    value = lookup.GetValue(parameter) as IFlagValue;
                    if (value == null)
                    {
                        throw new InvalidOperationException("Could not initialize optional value.");
                    }
                }

                // Return a converter for the flag element type.
                return (TypeDescriptor.GetConverter(value.Type), GetStringConstructor(value.Type));
            }

            return (TypeDescriptor.GetConverter(parameter.ParameterType), GetStringConstructor(parameter.ParameterType));
        }

        var type = Type.GetType(parameter.Converter.ConverterTypeName);
        return (resolver.Resolve(type) as TypeConverter, null);
    }
}