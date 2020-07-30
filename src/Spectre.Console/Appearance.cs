using System;

namespace Spectre.Console
{
    /// <summary>
    /// Represents color and style.
    /// </summary>
    public sealed class Appearance : IEquatable<Appearance>
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
        /// Gets the style.
        /// </summary>
        public Styles Style { get; }

        /// <summary>
        /// Gets an <see cref="Appearance"/> with the
        /// default color and without style.
        /// </summary>
        public static Appearance Plain { get; } = new Appearance();

        private Appearance()
            : this(null, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Appearance"/> class.
        /// </summary>
        /// <param name="foreground">The foreground color.</param>
        /// <param name="background">The background color.</param>
        /// <param name="style">The style.</param>
        public Appearance(Color? foreground = null, Color? background = null, Styles? style = null)
        {
            Foreground = foreground ?? Color.Default;
            Background = background ?? Color.Default;
            Style = style ?? Styles.None;
        }

        /// <summary>
        /// Combines this appearance with another one.
        /// </summary>
        /// <param name="other">The item to combine with this.</param>
        /// <returns>A new appearance representing a combination of this and the other one.</returns>
        public Appearance Combine(Appearance other)
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

            return new Appearance(foreground, background, Style | other.Style);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            unchecked
            {
                var hash = (int)2166136261;
                hash = (hash * 16777619) ^ Foreground.GetHashCode();
                hash = (hash * 16777619) ^ Background.GetHashCode();
                hash = (hash * 16777619) ^ Style.GetHashCode();
                return hash;
            }
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return Equals(obj as Appearance);
        }

        /// <inheritdoc/>
        public bool Equals(Appearance other)
        {
            if (other == null)
            {
                return false;
            }

            return Foreground.Equals(other.Foreground) &&
                Background.Equals(other.Background) &&
                Style == other.Style;
        }
    }
}
