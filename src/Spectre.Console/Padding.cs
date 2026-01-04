namespace Spectre.Console;

/// <summary>
/// Represents padding.
/// </summary>
public struct Padding : IEquatable<Padding>
{
    /// <summary>
    /// Gets the left padding.
    /// </summary>
    public int Left { get; }

    /// <summary>
    /// Gets the top padding.
    /// </summary>
    public int Top { get; }

    /// <summary>
    /// Gets the right padding.
    /// </summary>
    public int Right { get; }

    /// <summary>
    /// Gets the bottom padding.
    /// </summary>
    public int Bottom { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Padding"/> struct.
    /// </summary>
    /// <param name="size">The padding for all sides.</param>
    public Padding(int size)
        : this(size, size, size, size)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Padding"/> struct.
    /// </summary>
    /// <param name="horizontal">The left and right padding.</param>
    /// <param name="vertical">The top and bottom padding.</param>
    public Padding(int horizontal, int vertical)
        : this(horizontal, vertical, horizontal, vertical)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Padding"/> struct.
    /// </summary>
    /// <param name="left">The left padding.</param>
    /// <param name="top">The top padding.</param>
    /// <param name="right">The right padding.</param>
    /// <param name="bottom">The bottom padding.</param>
    public Padding(int left, int top, int right, int bottom)
    {
        Left = left;
        Top = top;
        Right = right;
        Bottom = bottom;
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return obj is Padding padding && Equals(padding);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        unchecked
        {
            var hash = (int)2166136261;
            hash = (hash * 16777619) ^ Left.GetHashCode();
            hash = (hash * 16777619) ^ Top.GetHashCode();
            hash = (hash * 16777619) ^ Right.GetHashCode();
            hash = (hash * 16777619) ^ Bottom.GetHashCode();
            return hash;
        }
    }

    /// <inheritdoc/>
    public bool Equals(Padding other)
    {
        return Left == other.Left
            && Top == other.Top
            && Right == other.Right
            && Bottom == other.Bottom;
    }

    /// <summary>
    /// Checks if two <see cref="Padding"/> instances are equal.
    /// </summary>
    /// <param name="left">The first <see cref="Padding"/> instance to compare.</param>
    /// <param name="right">The second <see cref="Padding"/> instance to compare.</param>
    /// <returns><c>true</c> if the two instances are equal, otherwise <c>false</c>.</returns>
    public static bool operator ==(Padding left, Padding right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// Checks if two <see cref="Padding"/> instances are not equal.
    /// </summary>
    /// <param name="left">The first <see cref="Padding"/> instance to compare.</param>
    /// <param name="right">The second <see cref="Padding"/> instance to compare.</param>
    /// <returns><c>true</c> if the two instances are not equal, otherwise <c>false</c>.</returns>
    public static bool operator !=(Padding left, Padding right)
    {
        return !(left == right);
    }

    /// <summary>
    /// Gets the padding width.
    /// </summary>
    /// <returns>The padding width.</returns>
    public int GetWidth()
    {
        return Left + Right;
    }

    /// <summary>
    /// Gets the padding height.
    /// </summary>
    /// <returns>The padding height.</returns>
    public int GetHeight()
    {
        return Top + Bottom;
    }
}

/// <summary>
/// Contains extension methods for <see cref="Padding"/>.
/// </summary>
public static class PaddingExtensions
{
    /// <param name="padding">The padding.</param>
    extension(Padding? padding)
    {
        /// <summary>
        /// Gets the left padding.
        /// </summary>
        /// <returns>The left padding or zero if <c>padding</c> is null.</returns>
        public int GetLeftSafe()
        {
            return padding?.Left ?? 0;
        }

        /// <summary>
        /// Gets the right padding.
        /// </summary>
        /// <returns>The right padding or zero if <c>padding</c> is null.</returns>
        public int GetRightSafe()
        {
            return padding?.Right ?? 0;
        }

        /// <summary>
        /// Gets the top padding.
        /// </summary>
        /// <returns>The top padding or zero if <c>padding</c> is null.</returns>
        public int GetTopSafe()
        {
            return padding?.Top ?? 0;
        }

        /// <summary>
        /// Gets the bottom padding.
        /// </summary>
        /// <returns>The bottom padding or zero if <c>padding</c> is null.</returns>
        public int GetBottomSafe()
        {
            return padding?.Bottom ?? 0;
        }
    }
}