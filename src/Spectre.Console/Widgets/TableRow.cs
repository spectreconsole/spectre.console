using System;
using System.Collections;
using System.Collections.Generic;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    /// <summary>
    /// Represents a table row.
    /// </summary>
    public sealed class TableRow : IEnumerable<IRenderable>
    {
        private readonly List<IRenderable> _items;

        /// <summary>
        /// Gets a row item at the specified table column index.
        /// </summary>
        /// <param name="index">The table column index.</param>
        /// <returns>The row item at the specified table column index.</returns>
        public IRenderable this[int index]
        {
            get => _items[index];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TableRow"/> class.
        /// </summary>
        /// <param name="items">The row items.</param>
        public TableRow(IEnumerable<IRenderable> items)
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
