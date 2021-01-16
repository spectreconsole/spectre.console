using System;

namespace Spectre.Console.Tests.Data
{
    public static class TestExceptions
    {
        public static bool MethodThatThrows(int? number) => throw new InvalidOperationException("Throwing!");

        public static bool GenericMethodThatThrows<T0, T1, TRet>(int? number) => throw new InvalidOperationException("Throwing!");

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

        public static void ThrowWithGenericInnerException()
        {
            try
            {
                GenericMethodThatThrows<int, float, double>(null);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Something threw!", ex);
            }
        }
    }
}
