using DynamicMember = System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute;

namespace Spectre.Console;

internal static class TypeConverterHelper
{
    private const DynamicallyAccessedMemberTypes ConverterAnnotation = DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.PublicFields;

    private static bool IsGetConverterSupported =>
        !AppContext.TryGetSwitch("Spectre.Console.TypeConverterHelper.IsGetConverterSupported ", out var enabled) || enabled;

    public static string ConvertToString<[DynamicMember(ConverterAnnotation)] T>(T input)
    {
        var result = GetTypeConverter<T>().ConvertToInvariantString(input);
        if (result == null)
        {
            throw new InvalidOperationException("Could not convert input to a string");
        }

        return result;
    }

    public static bool TryConvertFromString<[DynamicMember(ConverterAnnotation)] T>(string input, [MaybeNull] out T? result)
    {
        try
        {
            result = (T?)GetTypeConverter<T>().ConvertFromInvariantString(input);
            return true;
        }
        catch
        {
            result = default;
            return false;
        }
    }

    public static bool TryConvertFromStringWithCulture<[DynamicMember(ConverterAnnotation)] T>(string input, CultureInfo? info, [MaybeNull] out T? result)
    {
        try
        {
            if (info == null)
            {
                return TryConvertFromString<T>(input, out result);
            }
            else
            {
                result = (T?)GetTypeConverter<T>().ConvertFromString(null!, info, input);
            }

            return true;
        }
        catch
        {
            result = default;
            return false;
        }
    }

    public static TypeConverter GetTypeConverter<[DynamicMember(ConverterAnnotation)] T>()
    {
        var converter = GetConverter();
        if (converter != null)
        {
            return converter;
        }

        var attribute = typeof(T).GetCustomAttribute<TypeConverterAttribute>();
        if (attribute != null)
        {
            var type = Type.GetType(attribute.ConverterTypeName, false, false);
            if (type != null)
            {
                converter = Activator.CreateInstance(type) as TypeConverter;
                if (converter != null)
                {
                    return converter;
                }
            }
        }

        throw new InvalidOperationException("Could not find type converter");

        [UnconditionalSuppressMessage("ReflectionAnalysis", "IL2087", Justification = "Feature switches are not currently supported in the analyzer")]
        [UnconditionalSuppressMessage("ReflectionAnalysis", "IL2026", Justification = "Feature switches are not currently supported in the analyzer")]
        static TypeConverter? GetConverter()
        {
            if (!IsGetConverterSupported)
            {
                return GetIntrinsicConverter(typeof(T));
            }

            return TypeDescriptor.GetConverter(typeof(T));
        }
    }

    private delegate TypeConverter FuncWithDam([DynamicMember(ConverterAnnotation)] Type type);

    private static readonly Dictionary<Type, FuncWithDam> _intrinsicConverters;

    static TypeConverterHelper()
    {
        _intrinsicConverters = new Dictionary<Type, FuncWithDam>
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

#if NET7_0_OR_GREATER
            [typeof(Int128)] = _ => new Int128Converter(),
            [typeof(Half)] = _ => new HalfConverter(),
            [typeof(UInt128)] = _ => new UInt128Converter(),
            [typeof(DateOnly)] = _ => new DateOnlyConverter(),
            [typeof(TimeOnly)] = _ => new TimeOnlyConverter(),
            [typeof(Version)] = _ => new VersionConverter(),
#endif
        };
    }

    [UnconditionalSuppressMessage("ReflectionAnalysis", "IL2111", Justification = "Delegate reflection is safe for all usages in this type.")]
    private static FuncWithDam CreateEnumConverter() => ([DynamicMember(ConverterAnnotation)] type) => new EnumConverter(type);

    /// <summary>
    /// A highly-constrained version of <see cref="TypeDescriptor.GetConverter(Type)" /> that only returns intrinsic converters.
    /// </summary>
    private static TypeConverter? GetIntrinsicConverter([DynamicMember(ConverterAnnotation)] Type type)
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
            type = typeof(Enum);
        }

        if (_intrinsicConverters.TryGetValue(type, out var factory))
        {
            return factory(type);
        }

        return null;
    }
}
