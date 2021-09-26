using System;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    /// <summary>
    /// This <see cref="IPromptItemRenderer{T}"/> is based on a simple
    /// simple <c>Func&lt;T, string&gt;</c> converter to supply the
    /// markup required to render an item.
    /// </summary>
    /// <typeparam name="T">The type of the item that should be rendered.</typeparam>
    public sealed class SimpleMarkupPromptItemRenderer<T> : IPromptItemRenderer<T>
    {
        private readonly Func<T, string> _markupConverter;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleMarkupPromptItemRenderer{T}"/> class.
        /// </summary>
        /// <param name="markupConverter">The underlying converter to convert to markup.</param>
        public SimpleMarkupPromptItemRenderer(Func<T, string> markupConverter)
        {
            _markupConverter = markupConverter;
        }

        /// <inheritdoc cref="IPromptItemRenderer{T}.Render"/>
        public IRenderable Render(T item, PromptItemContext context)
        {
            var text = _markupConverter(item);

            if (context.State == PromptItemState.Current)
            {
                text = text.RemoveMarkup();
            }

            return new Markup(text, context.PromptStyle ?? Style.Plain);
        }
    }
}