using System;
using System.Collections.Generic;

namespace Spectre.Console
{
    internal static class ListExtensions
    {
        public static void RemoveLast<T>(this List<T> list)
        {
            if (list is null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            if (list.Count > 0)
            {
                list.RemoveAt(list.Count - 1);
            }
        }

        public static void AddOrReplaceLast<T>(this List<T> list, T item)
        {
            if (list is null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            if (list.Count == 0)
            {
                list.Add(item);
            }
            else
            {
                list[list.Count - 1] = item;
            }
        }
    }
}
