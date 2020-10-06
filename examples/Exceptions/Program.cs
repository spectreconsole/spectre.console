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
                AnsiConsole.Render(new Panel("[u]Default[/]").Expand());
                AnsiConsole.WriteLine();
                AnsiConsole.WriteException(ex);

                AnsiConsole.WriteLine();
                AnsiConsole.Render(new Panel("[u]Compact[/]").Expand());
                AnsiConsole.WriteLine();
                AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything | ExceptionFormats.ShowLinks);

                AnsiConsole.WriteLine();
                AnsiConsole.Render(new Panel("[u]Custom colors[/]").Expand());
                AnsiConsole.WriteLine();
                AnsiConsole.WriteException(ex, new ExceptionSettings
                {
                    Format = ExceptionFormats.ShortenEverything | ExceptionFormats.ShowLinks,
                    Style = new ExceptionStyle
                    {
                        Exception = Style.WithForeground(Color.Grey),
                        Message = Style.WithForeground(Color.White),
                        NonEmphasized = Style.WithForeground(Color.Cornsilk1),
                        Parenthesis = Style.WithForeground(Color.Cornsilk1),
                        Method = Style.WithForeground(Color.Red),
                        ParameterName = Style.WithForeground(Color.Cornsilk1),
                        ParameterType = Style.WithForeground(Color.Red),
                        Path = Style.WithForeground(Color.Red),
                        LineNumber = Style.WithForeground(Color.Cornsilk1),
                    }
                });
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
