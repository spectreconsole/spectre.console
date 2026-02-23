namespace Spectre.Console;

internal static class StackExtensions
{
    public static void PushRange<T>(this Stack<T> stack, IEnumerable<T> source)
    {
        ArgumentNullException.ThrowIfNull(stack);

        if (source != null)
        {
            foreach (var item in source)
            {
                stack.Push(item);
            }
        }
    }
}