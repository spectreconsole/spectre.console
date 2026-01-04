namespace Spectre.Console;

internal static class DictionaryExtensions
{
    extension<T1, T2>(KeyValuePair<T1, T2> tuple)
    {
        public void Deconstruct(out T1 key, out T2 value)
        {
            key = tuple.Key;
            value = tuple.Value;
        }
    }
}