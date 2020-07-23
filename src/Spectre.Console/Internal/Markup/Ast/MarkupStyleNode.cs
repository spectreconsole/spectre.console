using System;

namespace Spectre.Console.Internal
{
    internal sealed class MarkupStyleNode : IMarkupNode
    {
        private readonly Styles? _style;
        private readonly Color? _foreground;
        private readonly Color? _background;
        private readonly IMarkupNode _element;

        public MarkupStyleNode(
            Styles? style,
            Color? foreground,
            Color? background,
            IMarkupNode element)
        {
            _style = style;
            _foreground = foreground;
            _background = background;
            _element = element ?? throw new ArgumentNullException(nameof(element));
        }

        public void Render(IAnsiConsole renderer)
        {
            var style = (IDisposable)null;
            var foreground = (IDisposable)null;
            var background = (IDisposable)null;

            if (_style != null)
            {
                style = renderer.PushStyle(_style.Value);
            }

            if (_foreground != null)
            {
                foreground = renderer.PushColor(_foreground.Value, foreground: true);
            }

            if (_background != null)
            {
                background = renderer.PushColor(_background.Value, foreground: false);
            }

            _element.Render(renderer);

            background?.Dispose();
            foreground?.Dispose();
            style?.Dispose();
        }
    }
}
