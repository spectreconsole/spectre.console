namespace Spectre.Console.Ansi;

/// <summary>
/// Represents a parameter of a CSI or DCS sequence.
/// </summary>
/// <param name="Value">The parameter value. An omitted value is 0.</param>
/// <param name="Subparameters">
/// The colon-separated subparameters after the value, such as 3 in <c>CSI 4:3 m</c>.
/// An omitted subparameter is 0.
/// </param>
[Experimental(Constants.ExperimentalParserId)]
public sealed record AnsiParameter(int Value, IReadOnlyList<int> Subparameters)
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AnsiParameter"/> class
    /// for a parameter without subparameters.
    /// </summary>
    /// <param name="value">The parameter value.</param>
    public AnsiParameter(int value)
        : this(value, [])
    {
    }

    /// <inheritdoc/>
    public bool Equals(AnsiParameter? other)
    {
        return other is not null
            && Value == other.Value
            && Subparameters.SequenceEqual(other.Subparameters);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        unchecked
        {
            var hash = Value;
            foreach (var subparameter in Subparameters)
            {
                hash = (hash * 397) ^ subparameter;
            }

            return hash;
        }
    }

    // A record prints a list as its type name, so print the items instead
    private bool PrintMembers(StringBuilder builder)
    {
        builder.Append("Value = ").Append(Value.ToString(CultureInfo.InvariantCulture))
            .Append(", Subparameters = [").Append(string.Join(", ", Subparameters)).Append(']');

        return true;
    }
}
