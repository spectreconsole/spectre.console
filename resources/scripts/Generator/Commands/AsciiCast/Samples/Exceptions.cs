using System;
using System.Security.Authentication;
using Generator.Commands.Samples;
using Spectre.Console;

// keep the namespace here short because it'll be used in the display of the exceptions
// and we want to keep that below 100 characters wide.
namespace Samples
{
    public class Exceptions
    {
        internal abstract class BaseExceptionSample : BaseSample
        {
            public override (int Cols, int Rows) ConsoleSize => (100, 12);

            protected readonly Exception Exception = null!;

            protected BaseExceptionSample()
            {
                try
                {
                    DoMagic(42, null);
                }
                catch (Exception ex)
                {
                    Exception = ex;
                }
            }
        }

        internal class DefaultExceptionSample : BaseExceptionSample
        {
            public override void Run(IAnsiConsole console) => console.WriteException(Exception, ExceptionFormats.ShortenPaths);
        }

        internal class ShortenedExceptionSample : BaseExceptionSample
        {
            public override void Run(IAnsiConsole console) => console.WriteException(Exception, ExceptionFormats.ShortenEverything | ExceptionFormats.ShowLinks);
        }

        internal class CustomColorsExceptionSample : BaseExceptionSample
        {
            public override void Run(IAnsiConsole console)
            {
                console.WriteException(Exception, new ExceptionSettings
                {
                    Format = ExceptionFormats.ShortenEverything | ExceptionFormats.ShowLinks,
                    Style = new ExceptionStyle
                    {
                        Exception = new Style().Foreground(Color.Grey),
                        Message = new Style().Foreground(Color.White),
                        NonEmphasized = new Style().Foreground(Color.Cornsilk1),
                        Parenthesis = new Style().Foreground(Color.Cornsilk1),
                        Method = new Style().Foreground(Color.Red),
                        ParameterName = new Style().Foreground(Color.Cornsilk1),
                        ParameterType = new Style().Foreground(Color.Red),
                        Path = new Style().Foreground(Color.Red),
                        LineNumber = new Style().Foreground(Color.Cornsilk1),
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