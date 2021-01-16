using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Spectre.Console
{
    internal static class TypeConverterHelper
    {
        public static string ConvertToString<T>(T input)
        {
            return GetTypeConverter<T>().ConvertToInvariantString(input);
        }

        public static bool TryConvertFromString<T>(string input, [MaybeNull] out T result)
        {
            try
            {
                result = (T)GetTypeConverter<T>().ConvertFromInvariantString(input);
                return true;
            }
            catch
            {
                result = default;
                return false;
            }
        }

        public static TypeConverter GetTypeConverter<T>()
        {
            var converter = TypeDescriptor.GetConverter(typeof(T));
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
        }
    }
}
