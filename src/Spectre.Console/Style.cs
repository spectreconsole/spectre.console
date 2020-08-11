using System;
using Spectre.Console.Internal;

namespace Spectre.Console
{
    /// <summary>
    /// Represents color and text decoration.
    /// </summary>
    public sealed class Style : IEquatable<Style>
    {
        /// <summary>
        /// Gets the foreground color.
        /// </summary>
        public Color Foreground { get; }

        /// <summary>
        /// Gets the background color.
        /// </summary>
        public Color Background { get; }

        /// <summary>
        /// Gets the text decoration.
        /// </summary>
        public Decoration Decoration { get; }

        /// <summary>
        /// Gets an <see cref="Style"/> with the
        /// default colors and without text decoration.
        /// </summary>
        public static Style Plain { get; } = new Style();

        private Style()
            : this(null, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Style"/> class.
        /// </summary>
        /// <param name="foreground">The foreground color.</param>
        /// <param name="background">The background color.</param>
        /// <param name="decoration">The text decoration.</param>
        public Style(Color? foreground = null, Color? background = null, Decoration? decoration = null)
        {
            Foreground = foreground ?? Color.Default;
            Background = background ?? Color.Default;
            Decoration = decoration ?? Decoration.None;
        }

        /// <summary>
        /// Converts the string representation of a style to its <see cref="Style"/> equivalent.
        /// </summary>
        /// <param name="text">A string containing a style to parse.</param>
        /// <returns>A <see cref="Style"/> equivalent of the text contained in <paramref name="text"/>.</returns>
        public static Style Parse(string text)
        {
            return StyleParser.Parse(text);
        }

        /// <summary>
        /// Converts the string representation of a style to its <see cref="Style"/> equivalent.
        /// A return value indicates whether the operation succeeded.
        /// </summary>
        /// <param name="text">A string containing a style to parse.</param>
        /// <param name="result">
        /// When this method returns, contains the <see cref="Style"/> equivalent of the text contained in <paramref name="text"/>,
        /// if the conversion succeeded, or <c>null</c> if the conversion failed.
        /// </param>
        /// <returns><c>true</c> if s was converted successfully; otherwise, <c>false</c>.</returns>
        public static bool TryParse(string text, out Style? result)
        {
            return StyleParser.TryParse(text, out result);
        }

        /// <summary>
        /// Combines this style with another one.
        /// </summary>
        /// <param name="other">The item to combine with this.</param>
        /// <returns>A new style representing a combination of this and the other one.</returns>
        public Style Combine(Style other)
        {
            if (other is null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            var foreground = Foreground;
            if (!other.Foreground.IsDefault)
            {
                foreground = other.Foreground;
            }

            var background = Background;
            if (!other.Background.IsDefault)
            {
                background = other.Background;
            }

            return new Style(foreground, background, Decoration | other.Decoration);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            unchecked
            {
                var hash = (int)2166136261;
                hash = (hash * 16777619) ^ Foreground.GetHashCode();
                hash = (hash * 16777619) ^ Background.GetHashCode();
                hash = (hash * 16777619) ^ Decoration.GetHashCode();
                return hash;
            }
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            return Equals(obj as Style);
        }

        /// <inheritdoc/>
        public bool Equals(Style? other)
        {
            if (other == null)
            {
                return false;
            }

            return Foreground.Equals(other.Foreground) &&
                Background.Equals(other.Background) &&
                Decoration == other.Decoration;
        }
    }
}
