using System;
using System.Diagnostics.CodeAnalysis;

namespace Spectre.Console.Internal
{
    internal static class ConsoleExtensions
    {
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

        public static IDisposable PushStyle(this IAnsiConsole console, Styles style)
        {
            if (console is null)
            {
                throw new ArgumentNullException(nameof(console));
            }

            var current = console.Style;
            console.Style = style;
            return new StyleScope(console, current);
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

    internal sealed class StyleScope : IDisposable
    {
        private readonly IAnsiConsole _console;
        private readonly Styles _style;

        public StyleScope(IAnsiConsole console, Styles color)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
            _style = color;
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
            _console.Style = _style;
        }
    }
}
