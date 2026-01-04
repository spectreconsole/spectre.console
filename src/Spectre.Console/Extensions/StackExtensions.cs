namespace Spectre.Console;

internal static class StackExtensions
{
    extension<T>(Stack<T> stack)
    {
        public void PushRange(IEnumerable<T> source)
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
}