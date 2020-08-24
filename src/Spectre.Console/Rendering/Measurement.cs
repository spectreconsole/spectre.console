using System;

namespace Spectre.Console.Rendering
{
    /// <summary>
    /// Represents a measurement.
    /// </summary>
    public struct Measurement : IEquatable<Measurement>
    {
        /// <summary>
        /// Gets the minimum width.
        /// </summary>
        public int Min { get; }

        /// <summary>
        /// Gets the maximum width.
        /// </summary>
        public int Max { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Measurement"/> struct.
        /// </summary>
        /// <param name="min">The minimum width.</param>
        /// <param name="max">The maximum width.</param>
        public Measurement(int min, int max)
        {
            Min = min;
            Max = max;
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            return obj is Measurement measurement && Equals(measurement);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            unchecked
            {
                var hash = (int)2166136261;
                hash = (hash * 16777619) ^ Min.GetHashCode();
                hash = (hash * 16777619) ^ Max.GetHashCode();
                return hash;
            }
        }

        /// <inheritdoc/>
        public bool Equals(Measurement other)
        {
            return Min == other.Min && Max == other.Max;
        }

        /// <summary>
        /// Checks if two <see cref="Measurement"/> instances are equal.
        /// </summary>
        /// <param name="left">The first measurement instance to compare.</param>
        /// <param name="right">The second measurement instance to compare.</param>
        /// <returns><c>true</c> if the two measurements are equal, otherwise <c>false</c>.</returns>
        public static bool operator ==(Measurement left, Measurement right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Checks if two <see cref="Measurement"/> instances are not equal.
        /// </summary>
        /// <param name="left">The first measurement instance to compare.</param>
        /// <param name="right">The second measurement instance to compare.</param>
        /// <returns><c>true</c> if the two measurements are not equal, otherwise <c>false</c>.</returns>
        public static bool operator !=(Measurement left, Measurement right)
        {
            return !(left == right);
        }
    }
}
