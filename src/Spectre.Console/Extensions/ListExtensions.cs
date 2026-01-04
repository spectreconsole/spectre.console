namespace Spectre.Console;

internal static class ListExtensions
{
    extension<T>(List<T> list)
    {
        public void RemoveLast()
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

        public void AddOrReplaceLast(T item)
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