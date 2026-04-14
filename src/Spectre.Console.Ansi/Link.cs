namespace Spectre.Console;

/// <summary>
/// Represents a link.
/// </summary>
/// <param name="url">The link URL.</param>
public sealed class Link(string url) : IEquatable<Link>
{
    /// <summary>
    /// Gets the link ID.
    /// </summary>
    public int? Id { get; } = Random.Shared.Next(0, int.MaxValue);

    /// <summary>
    /// Gets the url.
    /// </summary>
    public string Url { get; } = url;

    /// <inheritdoc />
    public bool Equals(Link? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Id == other.Id && Url == other.Url;
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is Link other && Equals(other);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        unchecked
        {
            return (Id.GetHashCode() * 397) ^ Url.GetHashCode();
        }
    }
}