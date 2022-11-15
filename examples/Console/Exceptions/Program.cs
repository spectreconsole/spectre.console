using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;
using Spectre.Console;

namespace Exceptions;

public static class Program
{
    public static async Task Main(string[] args)
    {
        try
        {
            var foo = new List<string>();
            DoMagic(42, null, ref foo);
        }
        catch (Exception ex)
        {
            AnsiConsole.WriteLine();
            AnsiConsole.Write(new Rule("Default").LeftJustified());
            AnsiConsole.WriteLine();
            AnsiConsole.WriteException(ex);

            AnsiConsole.WriteLine();
            AnsiConsole.Write(new Rule("Compact").LeftJustified());
            AnsiConsole.WriteLine();
            AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything | ExceptionFormats.ShowLinks);

            AnsiConsole.WriteLine();
            AnsiConsole.Write(new Rule("Compact + Custom colors").LeftJustified());
            AnsiConsole.WriteLine();
            AnsiConsole.WriteException(ex, new ExceptionSettings
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

        try
        {
            await DoMagicAsync<int>(42, null);
        }
        catch (Exception ex)
        {
            AnsiConsole.WriteLine();
            AnsiConsole.Write(new Rule("Async").LeftJustified());
            AnsiConsole.WriteLine();
            AnsiConsole.WriteException(ex, ExceptionFormats.ShortenPaths);
        }
    }

    private static void DoMagic(int foo, string[,] bar, ref List<string> result)
    {
        try
        {
            CheckCredentials(foo, bar);
            result = new List<string>();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Whaaat?", ex);
        }
    }

    private static bool CheckCredentials(int? qux, string[,] corgi)
    {
        throw new InvalidCredentialException("The credentials are invalid.");
    }

    private static async Task DoMagicAsync<T>(T foo, string[,] bar)
    {
        try
        {
            await CheckCredentialsAsync(new[] { foo }.ToList(), new []{ foo },  bar);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Whaaat?", ex);
        }
    }

    private static async Task<bool> CheckCredentialsAsync<T>(List<T> qux, T[] otherArray, string[,] corgi)
    {
        await Task.Delay(0);
        throw new InvalidCredentialException("The credentials are invalid.");
    }
}

