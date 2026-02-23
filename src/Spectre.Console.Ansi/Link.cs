namespace Spectre.Console;

/// <summary>
/// Represents a link.
/// </summary>
/// <param name="url">The link URL.</param>
public sealed class Link(string url)
{
    /// <summary>
    /// Gets the link ID.
    /// </summary>
    public int? Id { get; } = Random.Shared.Next(0, int.MaxValue);

    /// <summary>
    /// Gets the url.
    /// </summary>
    public string Url { get; } = url;
}