namespace Spectre.Console;

/// <summary>
/// Contains extension methods for <see cref="string"/>.
/// </summary>
public static class StringExtensions
{
    // Cache whether or not internally normalized line endings
    // already are normalized. No reason to do yet another replace if it is.
    private static readonly bool _alreadyNormalized
        = Environment.NewLine.Equals("\n", StringComparison.OrdinalIgnoreCase);

    /// <param name="text">The text to escape.</param>
    extension(string? text)
    {
        /// <summary>
        /// Escapes text so that it won’t be interpreted as markup.
        /// </summary>
        /// <returns>A string that is safe to use in markup.</returns>
        public string EscapeMarkup()
        {
            if (text == null)
            {
                return string.Empty;
            }

            return text
                .ReplaceExact("[", "[[")
                .ReplaceExact("]", "]]");
        }

        /// <summary>
        /// Removes markup from the specified string.
        /// </summary>
        /// <returns>A string that does not have any markup.</returns>
        public string RemoveMarkup()
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return string.Empty;
            }

            var result = new StringBuilder();

            var tokenizer = new MarkupTokenizer(text);
            while (tokenizer.MoveNext() && tokenizer.Current != null)
            {
                if (tokenizer.Current.Kind == MarkupTokenKind.Text)
                {
                    result.Append(tokenizer.Current.Value);
                }
            }

            return result.ToString();
        }

        internal string CapitalizeFirstLetter(CultureInfo? culture = null)
        {
            if (text == null)
            {
                return string.Empty;
            }

            culture ??= CultureInfo.InvariantCulture;

            if (text.Length > 0 && char.IsLower(text[0]))
            {
                text = string.Format(culture, "{0}{1}", char.ToUpper(text[0], culture), text.Substring(1));
            }

            return text;
        }

        internal string? RemoveNewLines()
        {
            return text?.ReplaceExact("\r\n", string.Empty)
                ?.ReplaceExact("\n", string.Empty);
        }

        internal string NormalizeNewLines(bool native = false)
        {
            text = text?.ReplaceExact("\r\n", "\n");
            text ??= string.Empty;

            if (native && !_alreadyNormalized)
            {
                text = text.ReplaceExact("\n", Environment.NewLine);
            }

            return text;
        }
    }

    extension(string text)
    {
        /// <summary>
        /// Gets the cell width of the specified text.
        /// </summary>
        /// <param name="text">The text to get the cell width of.</param>
        /// <returns>The cell width of the text.</returns>
        public int GetCellWidth()
        {
            return Cell.GetCellLength(text);
        }

        internal string[] SplitLines()
        {
            var result = text?.NormalizeNewLines()?.Split(new[] { '\n' }, StringSplitOptions.None);
            return result ?? Array.Empty<string>();
        }

        internal string[] SplitWords(StringSplitOptions options = StringSplitOptions.None)
        {
            var result = new List<string>();

            static string Read(StringBuffer reader, Func<char, bool> criteria)
            {
                var buffer = new StringBuilder();
                while (!reader.Eof)
                {
                    var current = reader.Peek();
                    if (!criteria(current))
                    {
                        break;
                    }

                    buffer.Append(reader.Read());
                }

                return buffer.ToString();
            }

            using (var reader = new StringBuffer(text))
            {
                while (!reader.Eof)
                {
                    var current = reader.Peek();
                    if (char.IsWhiteSpace(current))
                    {
                        var x = Read(reader, c => char.IsWhiteSpace(c));
                        if (options != StringSplitOptions.RemoveEmptyEntries)
                        {
                            result.Add(x);
                        }
                    }
                    else
                    {
                        result.Add(Read(reader, c => !char.IsWhiteSpace(c)));
                    }
                }
            }

            return result.ToArray();
        }

        internal string Repeat(int count)
        {
            ArgumentNullException.ThrowIfNull(text);

            if (count <= 0)
            {
                return string.Empty;
            }

            if (count == 1)
            {
                return text;
            }

            return string.Concat(Enumerable.Repeat(text, count));
        }

        /// <summary>
        /// "Masks" every character in a string.
        /// </summary>
        /// <param name="value">String value to mask.</param>
        /// <param name="mask">Character to use for masking.</param>
        /// <returns>Masked string.</returns>
        public string Mask(char? mask)
        {
            if (mask is null)
            {
                return string.Empty;
            }

            return new string(mask.Value, text.Length);
        }

        /// <summary>
        /// Highlights the first text match in provided value.
        /// </summary>
        /// <param name="value">Input value.</param>
        /// <param name="searchText">Text to search for.</param>
        /// <param name="highlightStyle">The style to apply to the matched text.</param>
        /// <returns>Markup of input with the first matched text highlighted.</returns>
        internal string Highlight(string searchText, Style? highlightStyle)
        {
            ArgumentNullException.ThrowIfNull(text);

            ArgumentNullException.ThrowIfNull(searchText);

            ArgumentNullException.ThrowIfNull(highlightStyle);

            if (searchText.Length == 0)
            {
                return text;
            }

            var foundSearchPattern = false;
            var builder = new StringBuilder();
            using var tokenizer = new MarkupTokenizer(text);
            while (tokenizer.MoveNext())
            {
                var token = tokenizer.Current!;

                switch (token.Kind)
                {
                    case MarkupTokenKind.Text:
                        {
                            var tokenValue = token.Value;
                            if (tokenValue.Length == 0)
                            {
                                break;
                            }

                            if (foundSearchPattern)
                            {
                                builder.Append(tokenValue);
                                break;
                            }

                            var index = tokenValue.IndexOf(searchText, StringComparison.OrdinalIgnoreCase);
                            if (index == -1)
                            {
                                builder.Append(tokenValue);
                                break;
                            }

                            foundSearchPattern = true;
                            var before = tokenValue.Substring(0, index);
                            var match = tokenValue.Substring(index, searchText.Length);
                            var after = tokenValue.Substring(index + searchText.Length);

                            builder
                                .Append(before)
                                .AppendWithStyle(highlightStyle, match)
                                .Append(after);

                            break;
                        }

                    case MarkupTokenKind.Open:
                        {
                            builder.Append("[" + token.Value + "]");
                            break;
                        }

                    case MarkupTokenKind.Close:
                        {
                            builder.Append("[/]");
                            break;
                        }

                    default:
                        {
                            throw new InvalidOperationException("Unknown markup token kind.");
                        }
                }
            }

            return builder.ToString();
        }

        internal string ReplaceExact(string oldValue, string? newValue)
        {
#if NETSTANDARD2_0
            return text.Replace(oldValue, newValue);
#else
            return text.Replace(oldValue, newValue, StringComparison.Ordinal);
#endif
        }

        internal bool ContainsExact(string value)
        {
#if NETSTANDARD2_0
            return text.Contains(value);
#else
            return text.Contains(value, StringComparison.Ordinal);
#endif
        }

#if NETSTANDARD2_0
        internal bool Contains(string value, System.StringComparison comparisonType)
        {
            return text.IndexOf(value, comparisonType) != -1;
        }
#endif
    }
}