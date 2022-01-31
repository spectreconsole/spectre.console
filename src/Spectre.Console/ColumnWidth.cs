namespace Spectre.Console;

/// <summary>
/// Column width definition.
/// The size mode defines how the column width will be interpreted:
/// <list type="bullet">
///     <item>
///         <term><see cref="SizeMode.SizeToContent">SizeToContent (Auto)</see></term>
///         <description><see cref="Value" /> value is ignored and width will auto-size to content.</description>
///     </item>
///     <item>
///         <term><see cref="SizeMode.Fixed">Fixed</see></term>
///         <description><see cref="Value" /> value is interpreted as integer, fixed size.</description>
///     </item>
///     <item>
///         <term><see cref="SizeMode.Proportional">Star (*)</see></term>
///         <description><see cref="Value" /> value is interpreted as double and means proportional sizing. If the width value is <see langword="null"/> 1 is implied</description>
///     </item>
/// </list>
/// If mixed <see cref="SizeMode.SizeToContent" /> and <see cref="SizeMode.Fixed" /> widths with <see cref="SizeMode.Proportional" /> (proportional) widths:
/// The <see cref="SizeMode.Proportional" /> columns are apportioned to the remainder after the <see cref="SizeMode.SizeToContent" /> and
/// <see cref="SizeMode.Fixed" /> widths have been calculated.
/// </summary>
public class ColumnWidth : IEquatable<ColumnWidth?>
{
    /// <summary>
    /// Gets the size mode, to define
    /// how the width <see cref="Value"/> is interpreted.
    /// </summary>
    public SizeMode SizeMode
    {
        get;
    }

    /// <summary>
    /// Gets the width value.
    /// </summary>
    public double Value
    {
        get;
    }

    private ColumnWidth(SizeMode sizeMode, double value)
    {
        SizeMode = sizeMode;
        Value = value;
    }

    /// <summary>
    /// Implictly converts an <see cref="int"/> to a fixed column width.
    /// </summary>
    /// <param name="size">The column fixed width.</param>
    public static implicit operator ColumnWidth(int? size) => size == null ? SizeToContent() : Fixed(size.Value);

    /// <summary>
    /// Determines whether the specified objects are equal.
    /// </summary>
    /// <param name="left">The first object to compare.</param>
    /// <param name="right">The second object to compare.</param>
    /// <returns><see langword="true"/> if the specified objects are equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator ==(ColumnWidth? left, ColumnWidth? right) => EqualityComparer<ColumnWidth>.Default.Equals(left, right);

    /// <summary>
    /// Determines whether the specified objects are not equal.
    /// </summary>
    /// <param name="left">The first object to compare.</param>
    /// <param name="right">The second object to compare.</param>
    /// <returns><see langword="true"/> if the specified objects are not equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator !=(ColumnWidth? left, ColumnWidth? right) => !(left == right);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => Equals(obj as ColumnWidth);

    /// <inheritdoc/>
    public bool Equals(ColumnWidth? other)
        => other != null &&
           SizeMode == other.SizeMode &&
           Value == other.Value;

    /// <inheritdoc/>
    public override int GetHashCode()
#if NETSTANDARD2_0
        => new { SizeMode, Value }.GetHashCode();
#else
        => HashCode.Combine(SizeMode, Value);
#endif

    /// <summary>
    /// Creates a <see cref="ColumnWidth"/> with a fix size.
    /// </summary>
    /// <param name="size">The column fixed width.</param>
    /// <returns>A fix size width.</returns>
    public static ColumnWidth Fixed(int size)
    {
        if (size < 0.0)
        {
            throw new ArgumentException("Fixed size cannot be negative", nameof(size));
        }

        return new ColumnWidth(SizeMode.Fixed, size);
    }

    /// <summary>
    /// Creates a proportional weighted <see cref="ColumnWidth"/>.
    /// </summary>
    /// <param name="weight">The column weighted width.</param>
    /// <returns>A proportional weighted width.</returns>
    public static ColumnWidth Proportional(double weight = 1.0)
    {
        if (weight < 0.0)
        {
            throw new ArgumentException("Weight cannot be negative", nameof(weight));
        }

        return new ColumnWidth(SizeMode.Proportional, weight);
    }

    /// <summary>
    /// Creates a <see cref="ColumnWidth"/> to auto size to content.
    /// </summary>
    /// <returns>An auto size width.</returns>
    public static ColumnWidth SizeToContent() => new ColumnWidth(SizeMode.SizeToContent, 0.0);
}