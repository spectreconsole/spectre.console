namespace Spectre.Console;

internal static class ListExtensions
{
    extension<T>(List<T> list)
    {
        public void RemoveLast()
        {
            ArgumentNullException.ThrowIfNull(list);

            if (list.Count > 0)
            {
                list.RemoveAt(list.Count - 1);
            }
        }

        public void AddOrReplaceLast(T item)
        {
            ArgumentNullException.ThrowIfNull(list);

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