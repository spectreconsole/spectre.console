using System;
using System.Collections.Generic;
using Spectre.Console.Rendering;

namespace Spectre.Console.Cli
{
    internal static class AnsiConsoleExtensions
    {
        private static readonly Lazy<IAnsiConsole> _console;

        static AnsiConsoleExtensions()
        {
            _console = new Lazy<IAnsiConsole>(() => AnsiConsole.Console);
        }

        public static IAnsiConsole GetConsole(this IAnsiConsole? console)
        {
            return console ?? _console.Value;
        }

        public static void SafeRender(this IAnsiConsole? console, IRenderable? renderable)
        {
            if (renderable != null)
            {
                console ??= _console.Value;
                console.Write(renderable);
            }
        }

        public static void SafeRender(this IAnsiConsole? console, IEnumerable<IRenderable?> renderables)
        {
            console ??= _console.Value;
            foreach (var renderable in renderables)
            {
                if (renderable != null)
                {
                    console.Write(renderable);
                }
            }
        }
    }
}
