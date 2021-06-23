using System;
using System.Diagnostics;

namespace Spectre.Console
{
    public static class ShouldlyExtensions
    {
        [DebuggerStepThrough]
        public static T And<T>(this T item, Action<T> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            action(item);
            return item;
        }
    }
}
