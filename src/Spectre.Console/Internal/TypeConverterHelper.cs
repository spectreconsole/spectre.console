#nullable enable

using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Spectre.Console
{
    internal static class TypeConverterHelper
    {
        public static string ConvertToString<T>(T input) =>
          ConvertToString(input, typeof(T));

        public static string ConvertToString(object input)
        {
            _ = input ?? throw new ArgumentException(nameof(input));
            return ConvertToString(input, input.GetType());
        }

        private static string ConvertToString(object? input, Type type) =>
          GetTypeConverter(type).ConvertToInvariantString(input);

        public static bool TryConvertFromString<T>(string input, [MaybeNull] out T result)
        {
            if (TryConvertFromString(input, typeof(T), out object? resultObject))
            {
                if (!(resultObject is T))
                {
                    throw new InvalidCastException();
                }

                result = (T)resultObject;
                return true;
            }

            result = default;
            return false;
        }

        public static bool TryConvertFromString(string input, Type type, [MaybeNull] out object? result)
        {
            try
            {
                result = GetTypeConverter(type).ConvertFromInvariantString(input);
                return true;
            }
            catch
            {
                result = default;
                return false;
            }
        }

        public static TypeConverter GetTypeConverter<T>() =>
          GetTypeConverter(typeof(T));

        public static TypeConverter GetTypeConverter(Type type)
        {
            var converter = TypeDescriptor.GetConverter(type);
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
        }
    }
}
