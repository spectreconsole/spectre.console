namespace Spectre.Console;

/// <summary>
/// Represents a relative weight used to divide the width remaining after
/// fixed and content-sized columns have been measured.
/// </summary>
/// <remarks>
/// Weights are ratios, not percentages. A column with a weight of <c>2</c>
/// receives twice the remaining width of a column with a weight of <c>1</c>.
/// </remarks>
public readonly struct ProportionalWidth : IEquatable<ProportionalWidth>
{
    /// <summary>
    /// Gets the positive, finite proportional weight.
    /// </summary>
    public double Weight { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProportionalWidth"/> struct.
    /// </summary>
    /// <param name="weight">The proportional weight.</param>
    public ProportionalWidth(double weight)
    {
        Validate(weight);
        Weight = weight;
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return obj is ProportionalWidth width && Equals(width);
    }

    /// <inheritdoc/>
    public bool Equals(ProportionalWidth other)
    {
        return Weight.Equals(other.Weight);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return Weight.GetHashCode();
    }

    /// <summary>
    /// Checks if two <see cref="ProportionalWidth"/> instances are equal.
    /// </summary>
    /// <param name="left">The first instance to compare.</param>
    /// <param name="right">The second instance to compare.</param>
    /// <returns><c>true</c> if the two instances are equal; otherwise, <c>false</c>.</returns>
    public static bool operator ==(ProportionalWidth left, ProportionalWidth right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// Checks if two <see cref="ProportionalWidth"/> instances are not equal.
    /// </summary>
    /// <param name="left">The first instance to compare.</param>
    /// <param name="right">The second instance to compare.</param>
    /// <returns><c>true</c> if the two instances are not equal; otherwise, <c>false</c>.</returns>
    public static bool operator !=(ProportionalWidth left, ProportionalWidth right)
    {
        return !(left == right);
    }

    internal void Validate()
    {
        Validate(Weight);
    }

    private static void Validate(double weight)
    {
        if (weight <= 0 || double.IsNaN(weight) || double.IsInfinity(weight))
        {
            throw new ArgumentOutOfRangeException(
                nameof(weight),
                "Proportional width weight must be finite and greater than zero.");
        }
    }
}