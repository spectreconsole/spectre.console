using System;
using System.Diagnostics.CodeAnalysis;

namespace Spectre.Console.Internal
{
    internal static class AnsiConsoleExtensions
    {
        public static IDisposable PushStyle(this IAnsiConsole console, Style style)
        {
            if (console is null)
            {
                throw new ArgumentNullException(nameof(console));
            }

            if (style is null)
            {
                throw new ArgumentNullException(nameof(style));
            }

            var current = new Style(console.Foreground, console.Background, console.Decoration);
            console.SetColor(style.Foreground, true);
            console.SetColor(style.Background, false);
            console.Decoration = style.Decoration;
            return new StyleScope(console, current);
        }

        public static IDisposable PushColor(this IAnsiConsole console, Color color, bool foreground)
        {
            if (console is null)
            {
                throw new ArgumentNullException(nameof(console));
            }

            var current = foreground ? console.Foreground : console.Background;
            console.SetColor(color, foreground);
            return new ColorScope(console, current, foreground);
        }

        public static IDisposable PushDecoration(this IAnsiConsole console, Decoration decoration)
        {
            if (console is null)
            {
                throw new ArgumentNullException(nameof(console));
            }

            var current = console.Decoration;
            console.Decoration = decoration;
            return new DecorationScope(console, current);
        }

        public static void SetColor(this IAnsiConsole console, Color color, bool foreground)
        {
            if (console is null)
            {
                throw new ArgumentNullException(nameof(console));
            }

            if (foreground)
            {
                console.Foreground = color;
            }
            else
            {
                console.Background = color;
            }
        }
    }

    internal sealed class StyleScope : IDisposable
    {
        private readonly IAnsiConsole _console;
        private readonly Style _style;

        public StyleScope(IAnsiConsole console, Style style)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
            _style = style ?? throw new ArgumentNullException(nameof(style));
        }

        [SuppressMessage("Design", "CA1065:Do not raise exceptions in unexpected locations")]
        [SuppressMessage("Performance", "CA1821:Remove empty Finalizers")]
        ~StyleScope()
        {
            throw new InvalidOperationException("Style scope was not disposed.");
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            _console.SetColor(_style.Foreground, true);
            _console.SetColor(_style.Background, false);
            _console.Decoration = _style.Decoration;
        }
    }

    internal sealed class ColorScope : IDisposable
    {
        private readonly IAnsiConsole _console;
        private readonly Color _color;
        private readonly bool _foreground;

        public ColorScope(IAnsiConsole console, Color color, bool foreground)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
            _color = color;
            _foreground = foreground;
        }

        [SuppressMessage("Design", "CA1065:Do not raise exceptions in unexpected locations")]
        [SuppressMessage("Performance", "CA1821:Remove empty Finalizers")]
        ~ColorScope()
        {
            throw new InvalidOperationException("Color scope was not disposed.");
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            _console.SetColor(_color, _foreground);
        }
    }

    internal sealed class DecorationScope : IDisposable
    {
        private readonly IAnsiConsole _console;
        private readonly Decoration _decoration;

        public DecorationScope(IAnsiConsole console, Decoration decoration)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
            _decoration = decoration;
        }

        [SuppressMessage("Design", "CA1065:Do not raise exceptions in unexpected locations")]
        [SuppressMessage("Performance", "CA1821:Remove empty Finalizers")]
        ~DecorationScope()
        {
            throw new InvalidOperationException("Decoration scope was not disposed.");
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            _console.Decoration = _decoration;
        }
    }
}
