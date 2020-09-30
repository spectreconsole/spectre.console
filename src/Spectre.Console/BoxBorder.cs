using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    /// <summary>
    /// Represents a border.
    /// </summary>
    public abstract partial class BoxBorder
    {
        private readonly Dictionary<BoxBorderPart, string> _lookup;

        /// <summary>
        /// Gets the safe border for this border or <c>null</c> if none exist.
        /// </summary>
        public virtual BoxBorder? SafeBorder { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BoxBorder"/> class.
        /// </summary>
        protected BoxBorder()
        {
            _lookup = Initialize();
        }

        private Dictionary<BoxBorderPart, string> Initialize()
        {
            var lookup = new Dictionary<BoxBorderPart, string>();
            foreach (BoxBorderPart? part in Enum.GetValues(typeof(BoxBorderPart)))
            {
                if (part == null)
                {
                    continue;
                }

                var text = GetBorderPart(part.Value);
                if (text.Length > 1)
                {
                    throw new InvalidOperationException("A box part cannot contain more than one character.");
                }

                lookup.Add(part.Value, GetBorderPart(part.Value));
            }

            return lookup;
        }

        /// <summary>
        /// Gets the string representation of a specific border part.
        /// </summary>
        /// <param name="part">The part to get a string representation for.</param>
        /// <param name="count">The number of repetitions.</param>
        /// <returns>A string representation of the specified border part.</returns>
        public string GetPart(BoxBorderPart part, int count)
        {
            // TODO: This need some optimization...
            return string.Join(string.Empty, Enumerable.Repeat(GetBorderPart(part)[0], count));
        }

        /// <summary>
        /// Gets the string representation of a specific border part.
        /// </summary>
        /// <param name="part">The part to get a string representation for.</param>
        /// <returns>A string representation of the specified border part.</returns>
        public string GetPart(BoxBorderPart part)
        {
            return _lookup[part].ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Gets the character representing the specified border part.
        /// </summary>
        /// <param name="part">The part to get the character representation for.</param>
        /// <returns>A character representation of the specified border part.</returns>
        protected abstract string GetBorderPart(BoxBorderPart part);
    }
}
