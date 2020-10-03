using System;
using System.Diagnostics.CodeAnalysis;

namespace Spectre.Console.Tests.Data
{
    public static class TestExceptions
    {
        [SuppressMessage("Usage", "CA1801:Review unused parameters", Justification = "<Pending>")]
        public static bool MethodThatThrows(int? number) => throw new InvalidOperationException("Throwing!");

        public static void ThrowWithInnerException()
        {
            try
            {
                MethodThatThrows(null);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Something threw!", ex);
            }
        }
    }
}
