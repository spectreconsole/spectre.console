using System;
using System.Collections.Generic;
using System.Linq;

namespace Spectre.Console.Internal
{
    internal static class EnumerableExtensions
    {
        public static IEnumerable<(int Index, bool First, bool Last, T Item)> Enumerate<T>(this IEnumerable<T> source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return Enumerate(source.GetEnumerator());
        }

        public static IEnumerable<(int Index, bool First, bool Last, T Item)> Enumerate<T>(this IEnumerator<T> source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var first = true;
            var last = !source.MoveNext();
            T current;

            for (var index = 0; !last; index++)
            {
                current = source.Current;
                last = !source.MoveNext();
                yield return (index, first, last, current);
                first = false;
            }
        }

        public static IEnumerable<TResult> SelectIndex<T, TResult>(this IEnumerable<T> source, Func<T, int, TResult> func)
        {
            return source.Select((value, index) => func(value, index));
        }
    }
}
