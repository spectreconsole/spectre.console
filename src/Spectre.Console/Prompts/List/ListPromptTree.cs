namespace Spectre.Console;

internal sealed class ListPromptTree<T>
    where T : notnull
{
    private readonly List<ListPromptItem<T>> _roots;
    private readonly IEqualityComparer<T> _comparer;

    public ListPromptTree(IEqualityComparer<T> comparer)
    {
        _roots = new List<ListPromptItem<T>>();
        _comparer = comparer ?? throw new ArgumentNullException(nameof(comparer));
    }

    public ListPromptItem<T>? Find(T item)
    {
        var stack = new Stack<ListPromptItem<T>>(_roots);
        while (stack.Count > 0)
        {
            var current = stack.Pop();
            if (_comparer.Equals(item, current.Data))
            {
                return current;
            }

            stack.PushRange(current.Children);
        }

        return null;
    }

    public int? IndexOf(T item)
    {
        var index = 0;
        foreach (var node in Traverse())
        {
            if (_comparer.Equals(item, node.Data))
            {
                return index;
            }

            index++;
        }

        return null;
    }

    public void Add(ListPromptItem<T> node)
    {
        _roots.Add(node);
    }

    public IEnumerable<ListPromptItem<T>> Traverse()
    {
        foreach (var root in _roots)
        {
            var stack = new Stack<ListPromptItem<T>>();
            stack.Push(root);

            while (stack.Count > 0)
            {
                var current = stack.Pop();
                yield return current;

                foreach (var child in current.Children.ReverseEnumerable())
                {
                    stack.Push(child);
                }
            }
        }
    }
}