using System.Runtime.CompilerServices;

namespace Spectre.Console.Cli;

internal static class CreateInstanceHelpers
{
    private static readonly Dictionary<Type, Func<int, Array>> _arrayBuilder;
    private static readonly Dictionary<Type, Func<IMultiMap>> _multiMapBuilder;
    private static readonly Dictionary<Type, Func<object[], object>> _instanceBuilder = new();

    static CreateInstanceHelpers()
    {
        _arrayBuilder = new Dictionary<Type, Func<int, Array>>
        {
            [typeof(bool)] = size => new bool[size],
            [typeof(byte)] = size => new byte[size],
            [typeof(sbyte)] = size => new sbyte[size],
            [typeof(char)] = size => new char[size],
            [typeof(double)] = size => new double[size],
            [typeof(string)] = size => new string[size],
            [typeof(int)] = size => new int[size],
            [typeof(short)] = size => new short[size],
            [typeof(long)] = size => new long[size],
            [typeof(float)] = size => new float[size],
            [typeof(ushort)] = size => new ushort[size],
            [typeof(uint)] = size => new uint[size],
            [typeof(ulong)] = size => new ulong[size],
            [typeof(DateTime)] = size => new DateTime[size],
            [typeof(DateTimeOffset)] = size => new DateTimeOffset[size],
            [typeof(decimal)] = size => new decimal[size],
            [typeof(TimeSpan)] = size => new TimeSpan[size],
            [typeof(Guid)] = size => new Guid[size],
#if !NETSTANDARD2_0
            [typeof(Int128)] = size => new Int128[size],
            [typeof(Half)] = size => new Half[size],
            [typeof(UInt128)] = size => new UInt128[size],
            [typeof(DateOnly)] = size => new DateOnly[size],
            [typeof(TimeOnly)] = size => new TimeOnly[size],
#endif
        };

        _multiMapBuilder = new Dictionary<Type, Func<IMultiMap>>
        {
            [typeof(bool)] = () => new MultiMap<string, bool>(),
            [typeof(bool)] = () => new MultiMap<string, bool>(),
            [typeof(byte)] = () => new MultiMap<string, byte>(),
            [typeof(sbyte)] = () => new MultiMap<string, sbyte>(),
            [typeof(char)] = () => new MultiMap<string, char>(),
            [typeof(double)] = () => new MultiMap<string, double>(),
            [typeof(string)] = () => new MultiMap<string, string>(),
            [typeof(int)] = () => new MultiMap<string, int>(),
            [typeof(short)] = () => new MultiMap<string, short>(),
            [typeof(long)] = () => new MultiMap<string, long>(),
            [typeof(float)] = () => new MultiMap<string, float>(),
            [typeof(ushort)] = () => new MultiMap<string, ushort>(),
            [typeof(uint)] = () => new MultiMap<string, uint>(),
            [typeof(ulong)] = () => new MultiMap<string, ulong>(),
            [typeof(DateTime)] = () => new MultiMap<string, DateTime>(),
            [typeof(DateTimeOffset)] = () => new MultiMap<string, DateTimeOffset>(),
            [typeof(decimal)] = () => new MultiMap<string, decimal>(),
            [typeof(TimeSpan)] = () => new MultiMap<string, TimeSpan>(),
            [typeof(Guid)] = () => new MultiMap<string, Guid>(),
#if !NETSTANDARD2_0
            [typeof(Int128)] = () => new MultiMap<string, Int128>(),
            [typeof(Half)] = () => new MultiMap<string, Half>(),
            [typeof(UInt128)] = () => new MultiMap<string, UInt128>(),
            [typeof(DateOnly)] = () => new MultiMap<string, DateOnly>(),
            [typeof(TimeOnly)] = () => new MultiMap<string, TimeOnly>(),
#endif
        };
    }

    /// <summary>
    /// Add a new known type instance builder.
    /// </summary>
    /// <param name="type">The type to build.</param>
    public static void RegisterNewInstanceBuilder([DynamicallyAccessedMembers(PublicConstructors)] Type type)
    {
        _instanceBuilder.Add(type, input =>
        {
            var constructor = type.GetConstructor(BindingFlags.Public | BindingFlags.Instance, null, input.Select(i => i.GetType()).ToArray(), null);
            if (constructor == null)
            {
                throw new InvalidOperationException("Could not create single parameter instance.");
            }

            return constructor.Invoke(input);
        });
    }

    public static bool TryGetInstance(Type type, object[] parameters, [NotNullWhen(true)] out object? result)
    {
        if (_instanceBuilder.TryGetValue(type, out var factory))
        {
            result = factory(parameters);
            return true;
        }

        if (CanDoCreateInstance)
        {
            result = BuildInstance(type, parameters);
            if (result != null)
            {
                return true;
            }
        }

        result = null;
        return false;
    }

    private static object? BuildInstance(Type type, object[] parameters)
    {
        if (CanDoUnreferencedCode)
        {
            var constructor = type.GetConstructor(BindingFlags.Public | BindingFlags.Instance, null, parameters.Select(i => i.GetType()).ToArray(), null);
            if (constructor == null)
            {
                throw new InvalidOperationException("Could not create single parameter instance.");
            }

            return constructor.Invoke(parameters);
        }

        return null;
    }

    public static Array CreateArrayInstance(Type type, int size)
    {
#if NET9_0_OR_GREATER
        return Array.CreateInstanceFromArrayType(type, size);
#else
        var elementType = type.GetElementType();
        if (elementType == null)
        {
            throw new InvalidOperationException("Could not create an array of type " + type.FullName + ".");
        }

        return CreateArrayInstanceFromElementType(elementType, size);
#endif
    }

    public static IMultiMap? CreateMultiMapInstance(Type type1, Type type2)
    {
        if (type1 == typeof(string))
        {
            if (_multiMapBuilder.TryGetValue(type2, out var multiMapBuilder))
            {
                return multiMapBuilder.Invoke();
            }
        }

        if (CanDoCreateInstance)
        {
            return Activator.CreateInstance(typeof(MultiMap<,>).MakeGenericType(type1, type2)) as IMultiMap;
        }

        throw new InvalidOperationException("Could not create a multi map of type " + type1.FullName + " and " + type2.FullName + ". If you are running in AOT, only dictionaries with a string key and an .NET primitive value are supported.");
    }

    [UnconditionalSuppressMessage("AOT", "IL3050", Justification = "We are only creating an array instance dynamically for non-value types not in our dictionary.")]
    public static Array CreateArrayInstanceFromElementType(Type getType, int sourceArrayLength)
    {
        if (_arrayBuilder.TryGetValue(getType, out var value))
        {
            return value(sourceArrayLength);
        }

        if (!getType.IsValueType || CanDoCreateInstance)
        {
            return Array.CreateInstance(getType, sourceArrayLength);
        }

        throw new InvalidOperationException("Cannot create array instance of type " + getType.FullName + ". ");
    }

    [FeatureGuard(typeof(RequiresUnreferencedCodeAttribute))]
    public static bool CanDoUnreferencedCode
    {
        get
        {
#if NET9_0_OR_GREATER
#pragma warning disable IL4000
            return RuntimeFeature.IsDynamicCodeSupported;
#pragma warning restore IL4000
#else
        return true;
#endif
        }
    }

    [FeatureGuard(typeof(RequiresDynamicCodeAttribute))]
    public static bool CanDoCreateInstance
    {
        get
        {
#if NET9_0_OR_GREATER
            return RuntimeFeature.IsDynamicCodeSupported;
#else
        return true;
#endif
        }
    }
}