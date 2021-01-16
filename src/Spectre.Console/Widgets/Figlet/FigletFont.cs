using System;
using System.Collections.Generic;
using System.IO;

namespace Spectre.Console
{
    /// <summary>
    /// Represents a FIGlet font.
    /// </summary>
    public sealed class FigletFont
    {
        private const string StandardFont = "Spectre.Console/Widgets/Figlet/Fonts/Standard.flf";

        private readonly Dictionary<int, FigletCharacter> _characters;
        private static readonly Lazy<FigletFont> _standard;

        /// <summary>
        /// Gets the number of characters in the font.
        /// </summary>
        public int Count => _characters.Count;

        /// <summary>
        /// Gets the height of the font.
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// Gets the font's baseline.
        /// </summary>
        public int Baseline { get; }

        /// <summary>
        /// Gets the font's maximum width.
        /// </summary>
        public int MaxWidth { get; }

        /// <summary>
        /// Gets the default FIGlet font.
        /// </summary>
        public static FigletFont Default => _standard.Value;

        static FigletFont()
        {
            _standard = new Lazy<FigletFont>(() => Parse(
                ResourceReader.ReadManifestData(StandardFont)));
        }

        internal FigletFont(IEnumerable<FigletCharacter> characters, FigletHeader header)
        {
            _characters = new Dictionary<int, FigletCharacter>();

            foreach (var character in characters)
            {
                if (_characters.ContainsKey(character.Code))
                {
                    throw new InvalidOperationException("Character already exist");
                }

                _characters[character.Code] = character;
            }

            Height = header.Height;
            Baseline = header.Baseline;
            MaxWidth = header.MaxLength;
        }

        /// <summary>
        /// Loads a FIGlet font from the specified stream.
        /// </summary>
        /// <param name="stream">The stream to load the FIGlet font from.</param>
        /// <returns>The loaded FIGlet font.</returns>
        public static FigletFont Load(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                return Parse(reader.ReadToEnd());
            }
        }

        /// <summary>
        /// Loads a FIGlet font from disk.
        /// </summary>
        /// <param name="path">The path of the FIGlet font to load.</param>
        /// <returns>The loaded FIGlet font.</returns>
        public static FigletFont Load(string path)
        {
            return Parse(File.ReadAllText(path));
        }

        /// <summary>
        /// Parses a FIGlet font from the specified <see cref="string"/>.
        /// </summary>
        /// <param name="source">The FIGlet font source.</param>
        /// <returns>The parsed FIGlet font.</returns>
        public static FigletFont Parse(string source)
        {
            return FigletFontParser.Parse(source);
        }

        internal int GetWidth(string text)
        {
            var width = 0;
            foreach (var character in text)
            {
                width += GetCharacter(character)?.Width ?? 0;
            }

            return width;
        }

        internal FigletCharacter? GetCharacter(char character)
        {
            _characters.TryGetValue(character, out var result);
            return result;
        }

        internal IEnumerable<FigletCharacter> GetCharacters(string text)
        {
            if (text is null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            var result = new List<FigletCharacter>();
            foreach (var character in text)
            {
                if (_characters.TryGetValue(character, out var figletCharacter))
                {
                    result.Add(figletCharacter);
                }
            }

            return result;
        }
    }
}
