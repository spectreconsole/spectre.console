namespace Spectre.Console;

internal static class StackExtensions
{
    extension<T>(Stack<T> stack)
    {
        public void PushRange(IEnumerable<T> source)
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
}