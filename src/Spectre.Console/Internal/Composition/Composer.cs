using System;

namespace Spectre.Console.Internal
{
    internal sealed class Composer : IRenderable
    {
        private readonly BlockElement _root;

        /// <inheritdoc/>
        public int Length => _root.Length;

        public Composer()
        {
            _root = new BlockElement();
        }

        public static Composer New()
        {
            return new Composer();
        }

        public Composer Text(string text)
        {
            _root.Append(new TextElement(text));
            return this;
        }

        public Composer Foreground(Color color, Action<Composer> action)
        {
            if (action is null)
            {
                return this;
            }

            var content = new Composer();
            action(content);
            _root.Append(new ForegroundElement(color, content));
            return this;
        }

        public Composer Background(Color color, Action<Composer> action)
        {
            if (action is null)
            {
                return this;
            }

            var content = new Composer();
            action(content);
            _root.Append(new BackgroundElement(color, content));
            return this;
        }

        public Composer Style(Styles style, Action<Composer> action)
        {
            if (action is null)
            {
                return this;
            }

            var content = new Composer();
            action(content);
            _root.Append(new StyleElement(style, content));
            return this;
        }

        /// <inheritdoc/>
        public void Render(IAnsiConsole renderer)
        {
            _root.Render(renderer);
        }
    }
}
