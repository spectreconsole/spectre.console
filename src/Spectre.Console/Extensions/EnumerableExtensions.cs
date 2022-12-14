namespace Spectre.Console;

internal static class EnumerableExtensions
{
    // List.Reverse clashes with IEnumerable<T>.Reverse, so this method only exists
    // so we won't have to cast List<T> to IEnumerable<T>.
    public static IEnumerable<T> ReverseEnumerable<T>(this IEnumerable<T> source)
    {
        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        return source.Reverse();
    }

    public static bool None<T>(this IEnumerable<T> source, Func<T, bool> predicate)
    {
        return !source.Any(predicate);
    }

    public static IEnumerable<T> Repeat<T>(this IEnumerable<T> source, int count)
    {
        while (count-- > 0)
        {
            foreach (var item in source)
            {
                yield return item;
            }
        }
    }

    public static int IndexOf<T>(this IEnumerable<T> source, T item)
        where T : class
    {
        var index = 0;
        foreach (var candidate in source)
        {
            if (candidate == item)
            {
                return index;
            }

            index++;
        }

        return -1;
    }

    public static int GetCount<T>(this IEnumerable<T> source)
    {
        if (source is IList<T> list)
        {
            return list.Count;
        }

        if (source is T[] array)
        {
            return array.Length;
        }

        return source.Count();
    }

    public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
    {
        foreach (var item in source)
        {
            action(item);
        }
    }

    public static bool AnyTrue(this IEnumerable<bool> source)
    {
        return source.Any(b => b);
    }

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

#if !NET6_0_OR_GREATER
    public static IEnumerable<(TFirst First, TSecond Second)> Zip<TFirst, TSecond>(
        this IEnumerable<TFirst> source, IEnumerable<TSecond> first)
    {
        return source.Zip(first, (first, second) => (first, second));
    }
#endif

    public static IEnumerable<(TFirst First, TSecond Second, TThird Third)> Zip<TFirst, TSecond, TThird>(
        this IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third)
    {
        return first.Zip(second, (a, b) => (a, b))
            .Zip(third, (a, b) => (a.a, a.b, b));
    }
}