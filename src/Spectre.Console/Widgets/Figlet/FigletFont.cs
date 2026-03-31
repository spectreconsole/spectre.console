namespace Spectre.Console;

/// <summary>
/// Represents a Figlet font.
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
    /// Gets the hardblank character used in this font's glyph definitions.
    /// Hardblanks are rendered as spaces in output but are treated as opaque
    /// (non-space) characters during fitting and smushing.
    /// </summary>
    public char Hardblank { get; }

    /// <summary>
    /// Gets the horizontal smushing rules encoded as bit flags (bits 0–5, values 1–32).
    /// A value of 0 means universal smushing (any two non-space characters smush, right wins).
    /// </summary>
    public int SmushingRules { get; }

    /// <summary>
    /// Gets the default Figlet font.
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
            if (!_characters.TryAdd(character.Code, character))
            {
                throw new InvalidOperationException("Character already exist");
            }
        }

        Height = header.Height;
        Baseline = header.Baseline;
        MaxWidth = header.MaxLength;
        Hardblank = header.Hardblank;

        // Bits 0–5 (values 1, 2, 4, 8, 16, 32) encode the six horizontal smushing rules.
        // A result of 0 means no specific rules are defined, i.e. use universal smushing.
        if (header.FullLayout.HasValue)
        {
            SmushingRules = header.FullLayout.Value & 63;
        }
        else if (header.OldLayout > 0)
        {
            SmushingRules = header.OldLayout & 63;
        }
        else
        {
            SmushingRules = 0;
        }
    }

    /// <summary>
    /// Loads a Figlet font from the specified stream.
    /// </summary>
    /// <param name="stream">The stream to load the Figlet font from.</param>
    /// <returns>The loaded Figlet font.</returns>
    public static FigletFont Load(Stream stream)
    {
        using (var reader = new StreamReader(stream))
        {
            return Parse(reader.ReadToEnd());
        }
    }

    /// <summary>
    /// Loads a Figlet font from disk.
    /// </summary>
    /// <param name="path">The path of the Figlet font to load.</param>
    /// <returns>The loaded Figlet font.</returns>
    public static FigletFont Load(string path)
    {
        return Parse(File.ReadAllText(path));
    }

    /// <summary>
    /// Parses a Figlet font from the specified <see cref="string"/>.
    /// </summary>
    /// <param name="source">The Figlet font source.</param>
    /// <returns>The parsed Figlet font.</returns>
    public static FigletFont Parse(string source)
    {
        return FigletFontParser.Parse(source);
    }

    internal int GetWidth(string text)
    {
        return text.Sum(character => GetCharacter(character)?.Width ?? 0);
    }

    internal IEnumerable<FigletCharacter> GetCharacters(string text)
    {
        ArgumentNullException.ThrowIfNull(text);

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

    private FigletCharacter? GetCharacter(char character)
    {
        _characters.TryGetValue(character, out var result);
        return result;
    }
}