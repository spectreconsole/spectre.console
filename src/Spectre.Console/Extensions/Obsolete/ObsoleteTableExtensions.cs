using System;
using System.Linq;
using Spectre.Console.Internal;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    /// <summary>
    /// Contains extension methods for <see cref="Table"/>.
    /// </summary>
    public static class ObsoleteTableExtensions
    {
        /// <summary>
        /// Sets the table width.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="width">The width.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        [Obsolete("Use Width(..) instead.")]
        public static Table SetWidth(this Table table, int width)
        {
            if (table is null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            table.Width = width;
            return table;
        }

        /// <summary>
        /// Sets the table heading.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="text">The heading.</param>
        /// <param name="style">The style.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        [Obsolete("Use Heading(..) instead.")]
        public static Table SetHeading(this Table table, string text, Style? style = null)
        {
            if (table is null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            if (text is null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            return SetHeading(table, new TableTitle(text, style));
        }

        /// <summary>
        /// Sets the table heading.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="heading">The heading.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        [Obsolete("Use Heading(..) instead.")]
        public static Table SetHeading(this Table table, TableTitle heading)
        {
            if (table is null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            table.Heading = heading;
            return table;
        }

        /// <summary>
        /// Sets the table footnote.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="text">The footnote.</param>
        /// <param name="style">The style.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        [Obsolete("Use Footnote(..) instead.")]
        public static Table SetFootnote(this Table table, string text, Style? style = null)
        {
            if (table is null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            if (text is null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            return SetFootnote(table, new TableTitle(text, style));
        }

        /// <summary>
        /// Sets the table footnote.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="footnote">The footnote.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        [Obsolete("Use Footnote(..) instead.")]
        public static Table SetFootnote(this Table table, TableTitle footnote)
        {
            if (table is null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            table.Footnote = footnote;
            return table;
        }
    }
}
