using System.IO.Compression;

namespace Spectre.Console.Tests;

public enum FigletTestFont
{
    Big,
    Poison,
    StarWars,
}

public sealed class FigletTestFontLoader
{
    const string _zipPath = "Spectre.Console.Tests/Data/figlet/fonts.zip";

    public static FigletFont Load(FigletTestFont font)
    {
        var filename = font switch
        {
            FigletTestFont.Big => "big.flf",
            FigletTestFont.Poison => "poison.flf",
            FigletTestFont.StarWars => "star_wars.flf",
            _ => throw new InvalidOperationException("Unknown test font"),
        };

        var path = $"Spectre.Console.Tests/Data/figlet/{filename}";
        var stream = EmbeddedResourceReader.LoadResourceStream(path);
        return FigletFont.Load(stream);
    }

    public static FigletFont LoadZipFont(string name)
    {
        using var stream = EmbeddedResourceReader.LoadResourceStream(_zipPath);
        using var archive = new ZipArchive(stream, ZipArchiveMode.Read);
        var entry = archive.GetEntry(name);
        if (entry == null)
        {
            throw new InvalidOperationException("Could not find font in archive");
        }

        return FigletFont.Load(entry.Open());
    }

    public static List<string> GetAllZipFontFileNames()
    {
        using var stream = EmbeddedResourceReader.LoadResourceStream(_zipPath);
        using var archive = new ZipArchive(stream, ZipArchiveMode.Read);
        return archive.Entries.Select(entry => entry.FullName).ToList();
    }
}