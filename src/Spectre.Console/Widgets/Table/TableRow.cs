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

        internal bool IsHeader { get; }
        internal bool IsFooter { get; }

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
            : this(items, false, false)
        {
        }

        private TableRow(IEnumerable<IRenderable> items, bool isHeader, bool isFooter)
        {
            _items = new List<IRenderable>(items ?? Array.Empty<IRenderable>());

            IsHeader = isHeader;
            IsFooter = isFooter;
        }

        internal static TableRow Header(IEnumerable<IRenderable> items)
        {
            return new TableRow(items, true, false);
        }

        internal static TableRow Footer(IEnumerable<IRenderable> items)
        {
            return new TableRow(items, false, true);
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
