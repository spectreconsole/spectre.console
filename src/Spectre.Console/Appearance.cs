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
        public static Appearance Plain { get; }

        static Appearance()
        {
            Plain = new Appearance();
        }

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
