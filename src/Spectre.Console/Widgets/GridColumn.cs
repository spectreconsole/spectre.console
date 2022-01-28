namespace Spectre.Console;

/// <summary>
/// Represents a grid column.
/// </summary>
public sealed class GridColumn : IColumn, IHasDirtyState
{
    private bool _isDirty;
    private double? _width;
    private SizeMode _sizeMode;
    private bool _noWrap;
    private Padding? _padding;
    private Justify? _alignment;

    /// <inheritdoc/>
    bool IHasDirtyState.IsDirty => _isDirty;

    /// <summary>
    /// Gets or sets the width of the column.
    /// The interpretation of width depends on the value of <see cref="SizeMode"/>.<br/>
    /// <br/>
    /// By default it is set to <see langword="null"/>, and the column will size to its contents <see cref="SizeMode.SizeToContent"/>
    /// is set to <see cref="SizeMode.SizeToContent" />.
    /// </summary>
    public double? Width
    {
        get => _width;
        set => MarkAsDirty(() => _width = value);
    }

    /// <summary>
    /// Gets or sets the size mode which defines how the column width will be interpreted:
    /// <list type="bullet">
    ///     <item>
    ///         <term><see cref="SizeMode.SizeToContent">SizeToContent (Auto)</see></term>
    ///         <description><see cref="Width" /> value is ignored and width will auto-size to content.</description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="SizeMode.Fixed">Fixed</see></term>
    ///         <description><see cref="Width" /> value is interpreted as integer, fixed size.</description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="SizeMode.Star">Star (*)</see></term>
    ///         <description><see cref="Width" /> value is interpreted as double and means proportional sizing. If the width value is <see langword="null"/> 1 is implied</description>
    ///     </item>
    /// </list>
    /// If mixed <see cref="SizeMode.SizeToContent" /> and <see cref="SizeMode.Fixed" /> widths with <see cref="SizeMode.Star" /> (proportional) widths:<br/>
    /// The <see cref="SizeMode.Star" /> columns are apportioned to the remainder after the <see cref="SizeMode.SizeToContent" /> and
    /// <see cref="SizeMode.Fixed" /> widths have been calculated.<br/>
    /// </summary>
    public SizeMode SizeMode
    {
        get => _sizeMode;
        set => MarkAsDirty(() => _sizeMode = value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether wrapping of
    /// text within the column should be prevented.
    /// </summary>
    public bool NoWrap
    {
        get => _noWrap;
        set => MarkAsDirty(() => _noWrap = value);
    }

    /// <summary>
    /// Gets or sets the padding of the column.
    /// Vertical padding (top and bottom) is ignored.
    /// </summary>
    public Padding? Padding
    {
        get => _padding;
        set => MarkAsDirty(() => _padding = value);
    }

    /// <summary>
    /// Gets or sets the alignment of the column.
    /// </summary>
    public Justify? Alignment
    {
        get => _alignment;
        set => MarkAsDirty(() => _alignment = value);
    }

    /// <summary>
    /// Gets a value indicating whether the user
    /// has set an explicit padding for this column.
    /// </summary>
    internal bool HasExplicitPadding => Padding != null;

    private void MarkAsDirty(Action action)
    {
        action();
        _isDirty = true;
    }
}