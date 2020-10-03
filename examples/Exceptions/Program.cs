using System;
using System.Security.Authentication;
using Spectre.Console;

namespace Exceptions
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                DoMagic(42, null);
            }
            catch (Exception ex)
            {
                AnsiConsole.WriteLine();
                AnsiConsole.WriteException(ex);

                AnsiConsole.WriteLine();
                AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything | ExceptionFormats.ShowLinks);
            }
        }

        private static void DoMagic(int foo, string[,] bar)
        {
            try
            {
                CheckCredentials(foo, bar);
            }
            catch(Exception ex)
            {
                throw new InvalidOperationException("Whaaat?", ex);
            }
        }

        private static void CheckCredentials(int qux, string[,] corgi)
        {
            throw new InvalidCredentialException("The credentials are invalid.");
        }
    }
}
