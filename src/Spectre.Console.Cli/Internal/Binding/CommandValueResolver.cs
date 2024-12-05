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
                        object? value;
                        var converter = GetConverter(lookup, binder, resolver, mapped.Parameter);
                        var mappedValue = mapped.Value ?? string.Empty;
                        try
                        {
                            value = converter.ConvertFrom(mappedValue);
                        }
                        catch (Exception exception) when (exception is not CommandRuntimeException)
                        {
                            throw CommandRuntimeException.ConversionFailed(mapped, converter.TypeConverter, exception);
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

    private static object? ConvertValue(ITypeResolver resolver, CommandValueLookup lookup, CommandValueBinder binder,
        CommandParameter parameter, object? result)
    {
        if (result != null && result.GetType() != parameter.ParameterType)
        {
            var converter = GetConverter(lookup, binder, resolver, parameter);
            result = result is Array array ? ConvertArray(array, converter) : converter.ConvertFrom(result);
        }

        return result;
    }

    private static Array ConvertArray(Array sourceArray, SmartConverter converter)
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
                    targetArray ??= CreateInstanceHelpers.CreateArrayInstanceFromElementType(converted.GetType(), sourceArray.Length);
                    targetArray.SetValue(converted, i);
                }
            }
        }

        return targetArray ?? sourceArray;
    }

    [SuppressMessage("Style", "IDE0019:Use pattern matching", Justification = "It's OK")]
    private static SmartConverter GetConverter(CommandValueLookup lookup, CommandValueBinder binder,
        ITypeResolver resolver, CommandParameter parameter)
    {
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

                return new SmartConverter(elementType);
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
                return new SmartConverter(value.Type);
            }

            return new SmartConverter(parameter.ParameterType);
        }

        var type = Type.GetType(parameter.Converter.ConverterTypeName);
        if (type == null || resolver.Resolve(type) is not TypeConverter typeConverter)
        {
            throw CommandRuntimeException.NoConverterFound(parameter);
        }

        return new SmartConverter(typeConverter, type);
    }

    /// <summary>
    /// Convert inputs using the given <see cref="TypeConverter"/> and fallback to finding a constructor taking a single argument of the input type.
    /// </summary>
    private readonly ref struct SmartConverter
    {
        public SmartConverter(TypeConverter typeConverter, Type type)
        {
            TypeConverter = typeConverter;
            Type = type;
        }

        public SmartConverter(Type type)
        {
            Type = type;
            TypeConverter = TypeConverterHelper.GetTypeConverter(type);
        }

        public TypeConverter TypeConverter { get; }
        private Type Type { get; }

        public object? ConvertFrom(object input)
        {
            try
            {
                return TypeConverter.ConvertFrom(null, CultureInfo.InvariantCulture, input);
            }
            catch (NotSupportedException)
            {
                return CreateSingleParameterInstance(input);
            }
        }

        private object? CreateSingleParameterInstance(object input)
        {
            if (CreateInstanceHelpers.TryGetInstance(Type, [input], out var instance))
            {
                return instance;
            }

            throw new InvalidOperationException("Could not create single parameter instance.");
        }
    }
}

internal static class TypeConverterHelper
    {
        public static TypeConverter GetTypeConverter(Type type)
        {
            var converter = GetConverter(type);
            if (converter != null)
            {
                return converter;
            }

            var attribute = type.GetCustomAttribute<TypeConverterAttribute>();
            if (attribute != null)
            {
                var converterType = Type.GetType(attribute.ConverterTypeName, false, false);
                if (converterType != null)
                {
                    converter = Activator.CreateInstance(converterType) as TypeConverter;
                    if (converter != null)
                    {
                        return converter;
                    }
                }
            }

            throw new InvalidOperationException("Could not find type converter");

            [UnconditionalSuppressMessage("ReflectionAnalysis", "IL2067",
                Justification = "Feature switches are not currently supported in the analyzer")]
            [UnconditionalSuppressMessage("ReflectionAnalysis", "IL2026",
                Justification = "Feature switches are not currently supported in the analyzer")]
            static TypeConverter? GetConverter(Type type)
            {
                // otherwise try and use the intrinsic converter. if we can't find one, then
                // try and use GetConverter.
                var intrinsicConverter = GetIntrinsicConverter(type);
                return intrinsicConverter ?? TypeDescriptor.GetConverter(type);
            }
        }

        private static readonly Dictionary<Type, Func<Type, TypeConverter>> _intrinsicConverters;
        private static readonly Dictionary<Type, string> _defaultValuesAsString;

        static TypeConverterHelper()
        {
            _intrinsicConverters = new Dictionary<Type, Func<Type, TypeConverter>>
            {
                [typeof(bool)] = _ => new BooleanConverter(),
                [typeof(byte)] = _ => new ByteConverter(),
                [typeof(sbyte)] = _ => new SByteConverter(),
                [typeof(char)] = _ => new CharConverter(),
                [typeof(double)] = _ => new DoubleConverter(),
                [typeof(string)] = _ => new StringConverter(),
                [typeof(int)] = _ => new Int32Converter(),
                [typeof(short)] = _ => new Int16Converter(),
                [typeof(long)] = _ => new Int64Converter(),
                [typeof(float)] = _ => new SingleConverter(),
                [typeof(ushort)] = _ => new UInt16Converter(),
                [typeof(uint)] = _ => new UInt32Converter(),
                [typeof(ulong)] = _ => new UInt64Converter(),
                [typeof(object)] = _ => new TypeConverter(),
                [typeof(CultureInfo)] = _ => new CultureInfoConverter(),
                [typeof(DateTime)] = _ => new DateTimeConverter(),
                [typeof(DateTimeOffset)] = _ => new DateTimeOffsetConverter(),
                [typeof(decimal)] = _ => new DecimalConverter(),
                [typeof(TimeSpan)] = _ => new TimeSpanConverter(),
                [typeof(Guid)] = _ => new GuidConverter(),
                [typeof(Uri)] = _ => new UriTypeConverter(),
                [typeof(Array)] = _ => new ArrayConverter(),
                [typeof(ICollection)] = _ => new CollectionConverter(),
                [typeof(Enum)] = CreateEnumConverter(),
#if !NETSTANDARD2_0
                [typeof(Int128)] = _ => new Int128Converter(),
                [typeof(Half)] = _ => new HalfConverter(),
                [typeof(UInt128)] = _ => new UInt128Converter(),
                [typeof(DateOnly)] = _ => new DateOnlyConverter(),
                [typeof(TimeOnly)] = _ => new TimeOnlyConverter(),
                [typeof(Version)] = _ => new VersionConverter(),
#endif
            };

            _defaultValuesAsString = new Dictionary<Type, string>
            {
                [typeof(bool)] = default(bool).ToString(),
                [typeof(byte)] = default(byte).ToString(),
                [typeof(sbyte)] = default(sbyte).ToString(),
                [typeof(char)] = default(char).ToString(),
                [typeof(double)] = default(double).ToString(CultureInfo.CurrentCulture),
                [typeof(string)] = string.Empty,
                [typeof(int)] = default(int).ToString(),
                [typeof(short)] = default(short).ToString(),
                [typeof(long)] = default(long).ToString(),
                [typeof(float)] = default(float).ToString(CultureInfo.CurrentCulture),
                [typeof(ushort)] = default(ushort).ToString(),
                [typeof(uint)] = default(uint).ToString(),
                [typeof(ulong)] = default(ulong).ToString(),
                [typeof(DateTime)] = default(DateTime).ToString(CultureInfo.CurrentCulture),
                [typeof(DateTimeOffset)] = default(DateTimeOffset).ToString(CultureInfo.CurrentCulture),
                [typeof(decimal)] = default(decimal).ToString(CultureInfo.CurrentCulture),
                [typeof(TimeSpan)] = default(TimeSpan).ToString(),
                [typeof(Guid)] = default(Guid).ToString(),
#if !NETSTANDARD2_0
                [typeof(Int128)] = default(Int128).ToString(),
                [typeof(Half)] = default(Half).ToString(),
                [typeof(UInt128)] = default(UInt128).ToString(),
                [typeof(DateOnly)] = default(DateOnly).ToString(),
                [typeof(TimeOnly)] = default(TimeOnly).ToString(),
#endif
            };
        }

        public static string GetDefaultValueOfType(Type type) => _defaultValuesAsString[type];

        [UnconditionalSuppressMessage("ReflectionAnalysis", "IL2111", Justification = "Delegate reflection is safe for all usages in this type.")]
        private static Func<Type, TypeConverter> CreateEnumConverter() => ([DynamicallyAccessedMembers(PublicConstructors | PublicFields)] type) => new EnumConverter(type);

        /// <summary>
        /// A highly-constrained version of <see cref="TypeDescriptor.GetConverter(Type)" /> that only returns intrinsic converters.
        /// </summary>
        private static TypeConverter? GetIntrinsicConverter([DynamicallyAccessedMembers(PublicConstructors | PublicFields)] Type type)
        {
            if (type.IsArray)
            {
                type = typeof(Array);
            }

            if (typeof(ICollection).IsAssignableFrom(type))
            {
                type = typeof(ICollection);
            }

            if (type.IsEnum)
            {
                return new EnumConverter(type);
            }

            if (_intrinsicConverters.TryGetValue(type, out var factory))
            {
                return factory(type);
            }

            return null;
        }
    }