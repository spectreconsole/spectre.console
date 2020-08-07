using System;

namespace Spectre.Console
{
    /// <summary>
    /// Represents a measurement.
    /// </summary>
    public struct Padding : IEquatable<Padding>
    {
        /// <summary>
        /// Gets the left padding.
        /// </summary>
        public int Left { get; }

        /// <summary>
        /// Gets the right padding.
        /// </summary>
        public int Right { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Padding"/> struct.
        /// </summary>
        /// <param name="left">The left padding.</param>
        /// <param name="right">The right padding.</param>
        public Padding(int left, int right)
        {
            Left = left;
            Right = right;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
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
                hash = (hash * 16777619) ^ Right.GetHashCode();
                return hash;
            }
        }

        /// <inheritdoc/>
        public bool Equals(Padding other)
        {
            return Left == other.Left && Right == other.Right;
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
        /// Gets the horizontal padding.
        /// </summary>
        /// <returns>The horizontal padding.</returns>
        public int GetHorizontalPadding()
        {
            return Left + Right;
        }
    }
}
