using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Spectre.Console.Composition
{
    /// <summary>
    /// Represents a border used by tables.
    /// </summary>
    public abstract class Border
    {
        private readonly Dictionary<BorderPart, string> _lookup;

        private static readonly Dictionary<BorderKind, Border> _borders = new Dictionary<BorderKind, Border>
        {
            { BorderKind.None, new NoBorder() },
            { BorderKind.Ascii, new AsciiBorder() },
            { BorderKind.Square, new SquareBorder() },
            { BorderKind.Rounded, new RoundedBorder() },
        };

        private static readonly Dictionary<BorderKind, BorderKind> _safeLookup = new Dictionary<BorderKind, BorderKind>
        {
            { BorderKind.Rounded, BorderKind.Square },
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="Border"/> class.
        /// </summary>
        protected Border()
        {
            _lookup = Initialize();
        }

        /// <summary>
        /// Gets a <see cref="Border"/> represented by the specified <see cref="BorderKind"/>.
        /// </summary>
        /// <param name="kind">The kind of border to get.</param>
        /// <param name="safe">Whether or not to get a "safe" border that can be rendered in a legacy console.</param>
        /// <returns>A <see cref="Border"/> instance representing the specified <see cref="BorderKind"/>.</returns>
        public static Border GetBorder(BorderKind kind, bool safe)
        {
            if (safe && _safeLookup.TryGetValue(kind, out var safeKind))
            {
                kind = safeKind;
            }

            if (!_borders.TryGetValue(kind, out var border))
            {
                throw new InvalidOperationException("Unknown border kind");
            }

            return border;
        }

        private Dictionary<BorderPart, string> Initialize()
        {
            var lookup = new Dictionary<BorderPart, string>();
            foreach (BorderPart? part in Enum.GetValues(typeof(BorderPart)))
            {
                if (part == null)
                {
                    continue;
                }

                var text = GetBoxPart(part.Value);
                if (text.Length > 1)
                {
                    throw new InvalidOperationException("A box part cannot contain more than one character.");
                }

                lookup.Add(part.Value, GetBoxPart(part.Value));
            }

            return lookup;
        }

        /// <summary>
        /// Gets the string representation of a specific border part.
        /// </summary>
        /// <param name="part">The part to get a string representation for.</param>
        /// <param name="count">The number of repetitions.</param>
        /// <returns>A string representation of the specified border part.</returns>
        public string GetPart(BorderPart part, int count)
        {
            // TODO: This need some optimization...
            return string.Join(string.Empty, Enumerable.Repeat(GetBoxPart(part)[0], count));
        }

        /// <summary>
        /// Gets the string representation of a specific border part.
        /// </summary>
        /// <param name="part">The part to get a string representation for.</param>
        /// <returns>A string representation of the specified border part.</returns>
        public string GetPart(BorderPart part)
        {
            return _lookup[part].ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Gets the character representing the specified border part.
        /// </summary>
        /// <param name="part">The part to get the character representation for.</param>
        /// <returns>A character representation of the specified border part.</returns>
        protected abstract string GetBoxPart(BorderPart part);
    }
}
