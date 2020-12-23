using System;
using System.Diagnostics;
using Shouldly;

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

        [DebuggerStepThrough]
        public static void As<T>(this T item, Action<T> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            action(item);
        }

        [DebuggerStepThrough]
        public static void As<T>(this object item, Action<T> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            action((T)item);
        }

        [DebuggerStepThrough]
        public static void ShouldBe<T>(this Type item)
        {
            item.ShouldBe(typeof(T));
        }
    }
}
