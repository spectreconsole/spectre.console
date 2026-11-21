namespace Spectre.Console;

/// <summary>
/// Represents a grid column.
/// </summary>
public sealed class GridColumn : IColumn, IHasDirtyState
{
    private bool _isDirty;
    private int? _width;
    private ProportionalWidth? _proportionalWidth;
    private bool _noWrap;
    private Padding? _padding;
    private Justify? _alignment;

    /// <inheritdoc/>
    bool IHasDirtyState.IsDirty => _isDirty;

    /// <summary>
    /// Gets or sets the width of the column.
    /// If <c>null</c> and no proportional width has been configured,
    /// the column will adapt to its contents.
    /// </summary>
    public int? Width
    {
        get => _width;
        set => MarkAsDirty(() =>
        {
            _width = value;
            _proportionalWidth = null;
        });
    }

    internal ProportionalWidth? ProportionalWidth => _proportionalWidth;

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

    internal void SetProportionalWidth(ProportionalWidth width)
    {
        width.Validate();
        MarkAsDirty(() =>
        {
            _width = null;
            _proportionalWidth = width;
        });
    }

    private void MarkAsDirty(Action action)
    {
        action();
        _isDirty = true;
    }
}