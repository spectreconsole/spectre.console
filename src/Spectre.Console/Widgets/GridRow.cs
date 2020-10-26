using System;
using System.Collections;
using System.Collections.Generic;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    /// <summary>
    /// Represents a grid row.
    /// </summary>
    public sealed class GridRow : IEnumerable<IRenderable>
    {
        private readonly List<IRenderable> _items;

        /// <summary>
        /// Gets a row item at the specified grid column index.
        /// </summary>
        /// <param name="index">The grid column index.</param>
        /// <returns>The row item at the specified grid column index.</returns>
        public IRenderable this[int index]
        {
            get => _items[index];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GridRow"/> class.
        /// </summary>
        /// <param name="items">The row items.</param>
        public GridRow(IEnumerable<IRenderable> items)
        {
            _items = new List<IRenderable>(items ?? Array.Empty<IRenderable>());
        }

        internal void Add(IRenderable item)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            _items.Add(item);
        }

        /// <inheritdoc/>
        public IEnumerator<IRenderable> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
