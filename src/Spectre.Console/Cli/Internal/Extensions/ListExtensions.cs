using System;
using System.Collections.Generic;

namespace Spectre.Console.Cli
{
    internal static class ListExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source != null && action != null)
            {
                foreach (var item in source)
                {
                    action(item);
                }
            }
        }

        public static T AddAndReturn<T>(this IList<T> source, T item)
            where T : class
        {
            source.Add(item);
            return item;
        }

        public static void AddIfNotNull<T>(this IList<T> source, T? item)
            where T : class
        {
            if (item != null)
            {
                source.Add(item);
            }
        }

        public static void AddRangeIfNotNull<T>(this IList<T> source, IEnumerable<T?> items)
            where T : class
        {
            foreach (var item in items)
            {
                if (item != null)
                {
                    source.Add(item);
                }
            }
        }

        public static void AddRange<T>(this IList<T> source, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                source.Add(item);
            }
        }
    }
}
