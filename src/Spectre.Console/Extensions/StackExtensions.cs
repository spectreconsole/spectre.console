namespace Spectre.Console;

internal static class StackExtensions
{
    public static void PushRange<T>(this Stack<T> stack, IEnumerable<T> source)
    {
        if (stack is null)
        {
            throw new ArgumentNullException(nameof(stack));
        }

        if (source != null)
        {
            foreach (var item in source)
            {
                stack.Push(item);
            }
        }
    }
}