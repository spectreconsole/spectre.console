using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Spectre.Console.Internal
{
    internal static class TypeConverterHelper
    {
        public static string ConvertToString<T>(T input)
        {
            return TypeDescriptor.GetConverter(typeof(T)).ConvertToInvariantString(input);
        }

        [SuppressMessage("Design", "CA1031:Do not catch general exception types")]
        public static bool TryConvertFromString<T>(string input, [NotNullWhen(true)] out T? result)
        {
            try
            {
                var converter = TypeDescriptor.GetConverter(typeof(T));
                result = (T?)converter.ConvertFromInvariantString(input);
                return result != null;
            }
            catch
            {
                result = default;
                return false;
            }
        }
    }
}
