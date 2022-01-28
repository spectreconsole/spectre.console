namespace Spectre.Console;

/// <summary>
/// Represents a table column.
/// </summary>
public sealed class TableColumn : IColumn
{
    /// <summary>
    /// Gets or sets the column header.
    /// </summary>
    public IRenderable Header { get; set; }

    /// <summary>
    /// Gets or sets the column footer.
    /// </summary>
    public IRenderable? Footer { get; set; }

    /// <summary>
    /// Gets or sets the width of the column.
    /// The interpretation of width depends on the value of <see cref="SizeMode"/>.
    /// <br/>
    /// By default it is set to <see langword="null"/>, and the column will size to its contents <see cref="SizeMode.SizeToContent"/>
    /// is set to <see cref="SizeMode.SizeToContent" />.
    /// </summary>
    public double? Width { get; set; }

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
    /// If mixed <see cref="SizeMode.SizeToContent" /> and <see cref="SizeMode.Fixed" /> widths with <see cref="SizeMode.Star" /> (proportional) widths:
    /// The <see cref="SizeMode.Star" /> columns are apportioned to the remainder after the <see cref="SizeMode.SizeToContent" /> and
    /// <see cref="SizeMode.Fixed" /> widths have been calculated.
    /// </summary>
    public SizeMode SizeMode { get; set; }

    /// <summary>
    /// Gets or sets the padding of the column.
    /// Vertical padding (top and bottom) is ignored.
    /// </summary>
    public Padding? Padding { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether wrapping of
    /// text within the column should be prevented.
    /// </summary>
    public bool NoWrap { get; set; }

    /// <summary>
    /// Gets or sets the alignment of the column.
    /// </summary>
    public Justify? Alignment { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="TableColumn"/> class.
    /// </summary>
    /// <param name="header">The table column header.</param>
    public TableColumn(string header)
        : this(new Markup(header).Overflow(Overflow.Ellipsis))
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TableColumn"/> class.
    /// </summary>
    /// <param name="header">The <see cref="IRenderable"/> instance to use as the table column header.</param>
    public TableColumn(IRenderable header)
    {
        Header = header ?? throw new ArgumentNullException(nameof(header));
        Width = null;
        Padding = new Padding(1, 0, 1, 0);
        NoWrap = false;
        Alignment = null;
    }
}