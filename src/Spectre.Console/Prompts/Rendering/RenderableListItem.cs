namespace Spectre.Console
{
    internal sealed class RenderableListItem<T>
        where T : notnull
    {
        public T Data { get; }
        public int Index { get; }

        public RenderableListItem(T data, int index)
        {
            Data = data;
            Index = index;
        }
    }
}
