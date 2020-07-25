using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Spectre.Console.Internal;

namespace Spectre.Console
{
    public sealed class Composer : IRenderable
    {
        private readonly BlockElement _root;

        public int Width => _root.Width;

        public Composer()
        {
            _root = new BlockElement();
        }

        public Composer LineBreak()
        {
            _root.Append(new LineBreakElement());
            return this;
        }

        public Composer Tab()
        {
            return Tabs(1);
        }

        public Composer Tabs(int count)
        {
            _root.Append(new TabElement(count));
            return this;
        }

        public Composer Space()
        {
            return Spaces(1);
        }

        public Composer Spaces(int count)
        {
            _root.Append(new SpaceElement(count));
            return this;
        }

        public Composer Repeat(char character, int count)
        {
            return Repeat(CultureInfo.CurrentCulture, character, count);
        }

        public Composer Repeat(IFormatProvider provider, char character, int count)
        {
            _root.Append(new RepeatingElement(count, new TextElement(character.ToString(provider))));
            return this;
        }

        public Composer Markup(string markup)
        {
            _root.Append(new MarkupElement(markup));
            return this;
        }

        public Composer Text(string text)
        {
            _root.Append(new TextElement(text));
            return this;
        }

        public Composer Append(IRenderable item)
        {
            _root.Append(item);
            return this;
        }

        public Composer Append(IEnumerable<IRenderable> source)
        {
            foreach (var item in source)
            {
                _root.Append(item);
            }

            return this;
        }

        public Composer Join(string separator, IEnumerable<IRenderable> items)
        {
            var array = items.ToArray();
            for (var i = 0; i < array.Length; i++)
            {
                _root.Append(array[i]);
                if (i != array.Length - 1)
                {
                    _root.Append(new TextElement(separator));
                }
            }

            return this;
        }

        public void Render(IAnsiConsole renderer)
        {
            _root.Render(renderer);
        }
    }
}
