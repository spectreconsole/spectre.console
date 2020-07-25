using System;
using System.Globalization;
using Spectre.Console.Internal;

namespace Spectre.Console
{
    /// <summary>
    /// A utility that makes it easy to create
    /// formatted text programatically.
    /// </summary>
    public sealed class Composer : IConsoleElement
    {
        private readonly BlockElement _root;

        /// <inheritdoc/>
        int IConsoleElement.Width => _root.Width;

        /// <summary>
        /// Initializes a new instance of the <see cref="Composer"/> class.
        /// </summary>
        public Composer()
        {
            _root = new BlockElement();
        }

        /// <summary>
        /// Appends a line break.
        /// </summary>
        /// <returns>The same <see cref="Composer"/> instance so that multiple calls can be chained.</returns>
        public Composer LineBreak()
        {
            _root.Append(new LineBreakElement());
            return this;
        }

        /// <summary>
        /// Appends a horizontal tab character.
        /// </summary>
        /// <returns>The same <see cref="Composer"/> instance so that multiple calls can be chained.</returns>
        public Composer Tab()
        {
            return Tabs(1);
        }

        /// <summary>
        /// Appends one or more horizontal tab characters.
        /// </summary>
        /// <param name="count">The number of tabs.</param>
        /// <returns>The same <see cref="Composer"/> instance so that multiple calls can be chained.</returns>
        public Composer Tabs(int count)
        {
            _root.Append(new TabElement(count));
            return this;
        }

        /// <summary>
        /// Appends a space.
        /// </summary>
        /// <returns>The same <see cref="Composer"/> instance so that multiple calls can be chained.</returns>
        public Composer Space()
        {
            return Spaces(1);
        }

        /// <summary>
        /// Appends one or more spaces.
        /// </summary>
        /// <param name="count">The number of spaces.</param>
        /// <returns>The same <see cref="Composer"/> instance so that multiple calls can be chained.</returns>
        public Composer Spaces(int count)
        {
            _root.Append(new SpaceElement(count));
            return this;
        }

        /// <summary>
        /// Appends a character the specified number of times.
        /// </summary>
        /// <param name="character">The character to append.</param>
        /// <param name="count">The number of times the character is appended.</param>
        /// <returns>The same <see cref="Composer"/> instance so that multiple calls can be chained.</returns>
        public Composer Repeat(char character, int count)
        {
            return Repeat(CultureInfo.CurrentCulture, character, count);
        }

        /// <summary>
        /// Appends a character the specified number of times.
        /// </summary>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <param name="character">The character to append.</param>
        /// <param name="count">The number of times the character is appended.</param>
        /// <returns>The same <see cref="Composer"/> instance so that multiple calls can be chained.</returns>
        public Composer Repeat(IFormatProvider provider, char character, int count)
        {
            _root.Append(new RepeatingElement(count, new TextElement(character.ToString(provider))));
            return this;
        }

        /// <summary>
        /// Appends a markup string.
        /// </summary>
        /// <param name="markup">The markup code to append.</param>
        /// <returns>The same <see cref="Composer"/> instance so that multiple calls can be chained.</returns>
        public Composer Markup(string markup)
        {
            _root.Append(new MarkupElement(markup));
            return this;
        }

        /// <summary>
        /// Appends text.
        /// </summary>
        /// <param name="text">The text to append.</param>
        /// <returns>The same <see cref="Composer"/> instance so that multiple calls can be chained.</returns>
        public Composer Text(string text)
        {
            _root.Append(new TextElement(text));
            return this;
        }

        /// <inheritdoc/>
        public void Render(IAnsiConsole renderer)
        {
            _root.Render(renderer);
        }
    }
}
