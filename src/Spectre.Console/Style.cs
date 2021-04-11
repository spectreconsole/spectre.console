using System;
using System.Collections.Generic;

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
        /// Gets the link associated with the style.
        /// </summary>
        public string? Link { get; }

        /// <summary>
        /// Gets an <see cref="Style"/> with the
        /// default colors and without text decoration.
        /// </summary>
        public static Style Plain { get; } = new Style();

        /// <summary>
        /// Initializes a new instance of the <see cref="Style"/> class.
        /// </summary>
        /// <param name="foreground">The foreground color.</param>
        /// <param name="background">The background color.</param>
        /// <param name="decoration">The text decoration.</param>
        /// <param name="link">The link.</param>
        public Style(Color? foreground = null, Color? background = null, Decoration? decoration = null, string? link = null)
        {
            Foreground = foreground ?? Color.Default;
            Background = background ?? Color.Default;
            Decoration = decoration ?? Decoration.None;
            Link = link;
        }

        /// <summary>
        /// Creates a new style from the specified foreground color.
        /// </summary>
        /// <param name="color">The foreground color.</param>
        /// <returns>A new <see cref="Style"/> with the specified foreground color.</returns>
        [Obsolete("Use ctor(..) instead")]
        public static Style WithForeground(Color color)
        {
            return new Style(foreground: color);
        }

        /// <summary>
        /// Creates a new style from the specified background color.
        /// </summary>
        /// <param name="color">The background color.</param>
        /// <returns>A new <see cref="Style"/> with the specified background color.</returns>
        [Obsolete("Use ctor(..) instead")]
        public static Style WithBackground(Color color)
        {
            return new Style(background: color);
        }

        /// <summary>
        /// Creates a new style from the specified text decoration.
        /// </summary>
        /// <param name="decoration">The text decoration.</param>
        /// <returns>A new <see cref="Style"/> with the specified text decoration.</returns>
        [Obsolete("Use ctor(..) instead")]
        public static Style WithDecoration(Decoration decoration)
        {
            return new Style(decoration: decoration);
        }

        /// <summary>
        /// Creates a new style from the specified link.
        /// </summary>
        /// <param name="link">The link.</param>
        /// <returns>A new <see cref="Style"/> with the specified link.</returns>
        [Obsolete("Use ctor(..) instead")]
        public static Style WithLink(string link)
        {
            return new Style(link: link);
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

            var link = Link;
            if (!string.IsNullOrWhiteSpace(other.Link))
            {
                link = other.Link;
            }

            return new Style(foreground, background, Decoration | other.Decoration, link);
        }

        /// <summary>
        /// Implicitly converts <see cref="string"/> into a <see cref="Style"/>.
        /// </summary>
        /// <param name="style">The style string.</param>
        public static implicit operator Style(string style)
        {
            return Parse(style);
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

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            int? GetLinkHashCode()
            {
#if NET5_0
                return Link?.GetHashCode(StringComparison.Ordinal);
#else
                return Link?.GetHashCode();
#endif
            }

            unchecked
            {
                var hash = (int)2166136261;
                hash = (hash * 16777619) ^ Foreground.GetHashCode();
                hash = (hash * 16777619) ^ Background.GetHashCode();
                hash = (hash * 16777619) ^ Decoration.GetHashCode();

                if (Link != null)
                {
                    hash = (hash * 16777619) ^ GetLinkHashCode() ?? 0;
                }

                return hash;
            }
        }

        /// <summary>
        /// Returns the markup representation of this style.
        /// </summary>
        /// <returns>The markup representation of this style.</returns>
        public string ToMarkup()
        {
            var builder = new List<string>();

            if (Decoration != Decoration.None)
            {
                var result = DecorationTable.GetMarkupNames(Decoration);
                if (result.Count != 0)
                {
                    builder.AddRange(result);
                }
            }

            if (Foreground != Color.Default)
            {
                builder.Add(Foreground.ToMarkup());
            }

            if (Background != Color.Default)
            {
                if (builder.Count == 0)
                {
                    builder.Add("default");
                }

                builder.Add("on " + Background.ToMarkup());
            }

            return string.Join(" ", builder);
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
                Decoration == other.Decoration &&
                string.Equals(Link, other.Link, StringComparison.Ordinal);
        }
    }
}
