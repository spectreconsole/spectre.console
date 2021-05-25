using System;
using System.Threading;

namespace Spectre.Console.Examples
{
    public static class ThreadSleeper
    {
        private static bool IsInCi = Environment.GetEnvironmentVariable("CI") != null;

        /// <summary>
        /// Suspends the current thread for timeout milliseconds as long as the environmental variable
        /// "CI" isn't found.
        /// </summary>
        /// <param name="millisecondsTimeout"></param>
        public static void Sleep(int millisecondsTimeout)
        {
            if (IsInCi == false)
            {
                Thread.Sleep(millisecondsTimeout);
            }
        }
    }
}