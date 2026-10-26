namespace Spectre.Console.Tests.Unit;

public sealed class TableMeasurerTests
{
    public static TheoryData<int, double[], int[]> ProportionalWidths => new()
    {
        { 3, [1, 1], [2, 1] },
        { 9, [1, 2], [3, 6] },
        { 10, [1.5, 1], [6, 4] },
        { 10, [1, 1, 1], [4, 3, 3] },
        { 1, [1, 1], [1, 0] },
        { 9, [double.MaxValue, double.MaxValue / 2], [6, 3] },
    };

    [Theory]
    [MemberData(nameof(ProportionalWidths))]
    public void Should_Distribute_Proportional_Widths_Exactly(
        int availableWidth,
        double[] weights,
        int[] expected)
    {
        // Given
        var table = CreateTable(
            weights.Select(weight => CreateColumn().Width(new ProportionalWidth(weight))).ToArray());

        // When
        var result = CalculateColumnWidths(table, availableWidth);

        // Then
        result.ShouldBe(expected);
        result.Sum().ShouldBe(availableWidth);
    }

    [Fact]
    public void Should_Be_Invariant_To_Weight_Scale()
    {
        // Given
        var first = CreateTable(
            CreateColumn().Width(new ProportionalWidth(1)),
            CreateColumn().Width(new ProportionalWidth(2)));
        var second = CreateTable(
            CreateColumn().Width(new ProportionalWidth(10)),
            CreateColumn().Width(new ProportionalWidth(20)));

        // When
        var firstWidths = CalculateColumnWidths(first, 11);
        var secondWidths = CalculateColumnWidths(second, 11);

        // Then
        firstWidths.ShouldBe(secondWidths);
    }

    [Fact]
    public void Should_Allocate_Only_The_Remaining_Width()
    {
        // Given
        var table = CreateTable(
            CreateColumn().Width(2),
            CreateColumn("abc"),
            CreateColumn().Width(new ProportionalWidth(1)));

        // When
        var result = CalculateColumnWidths(table, 10);

        // Then
        result.ShouldBe([2, 3, 5]);
    }

    [Fact]
    public void Should_Not_Expand_Fixed_Columns()
    {
        // Given
        var table = CreateTable(
            CreateColumn().Width(2),
            CreateColumn().Width(new ProportionalWidth(1)),
            CreateColumn().Width(new ProportionalWidth(1)));
        table.Expand = true;

        // When
        var result = CalculateColumnWidths(table, 10);

        // Then
        result.ShouldBe([2, 4, 4]);
    }

    [Fact]
    public void Should_Respect_Explicit_Table_Width()
    {
        // Given
        var table = CreateTable(
            CreateColumn().Width(new ProportionalWidth(1)),
            CreateColumn().Width(new ProportionalWidth(1)));
        table.Width = 8;

        // When
        var result = CalculateColumnWidths(table, 20);

        // Then
        result.ShouldBe([4, 4]);
    }

    [Fact]
    public void Should_Exclude_Borders_And_Padding_From_Allocated_Width()
    {
        // Given
        var table = new Table();
        table.AddColumn(new TableColumn("").Width(new ProportionalWidth(1)));
        table.AddColumn(new TableColumn("").Width(new ProportionalWidth(1)));

        // When
        var result = CalculateColumnWidths(table, 20);

        // Then
        result.ShouldBe([7, 6]);
        result.Sum().ShouldBe(13);
    }

    [Fact]
    public void Should_Handle_Widths_Narrower_Than_Fixed_Content()
    {
        // Given
        var table = CreateTable(
            CreateColumn().Width(10),
            CreateColumn().Width(new ProportionalWidth(1)));

        // When
        var result = CalculateColumnWidths(table, 5);

        // Then
        result.ShouldAllBe(width => width >= 0);
        result.Sum().ShouldBe(5);
    }

    [Fact]
    public void Should_Allocate_Widths_When_A_Cell_Spans_Proportional_Columns()
    {
        // Given
        var table = CreateTable(
            CreateColumn().Width(new ProportionalWidth(1)),
            CreateColumn().Width(new ProportionalWidth(1)));
        table.AddRow(new TableCell("Spanning content").Span(2));

        // When
        var result = CalculateColumnWidths(table, 9);

        // Then
        result.ShouldBe([5, 4]);
    }

    [Fact]
    public void Should_Keep_Intrinsic_Measurement_Separate_From_Allocated_Width()
    {
        // Given
        var table = CreateTable(
            CreateColumn("A").Width(new ProportionalWidth(1)),
            CreateColumn("BB").Width(new ProportionalWidth(1)));
        var options = CreateRenderOptions(20);

        // When
        var result = ((IRenderable)table).Measure(options, 20);

        // Then
        result.ShouldBe(new Measurement(3, 3));
    }

    private static Table CreateTable(params TableColumn[] columns)
    {
        var table = new Table().NoBorder();
        table.AddColumns(columns);
        return table;
    }

    private static TableColumn CreateColumn(string header = "")
    {
        return new TableColumn(header).Padding(0, 0);
    }

    private static List<int> CalculateColumnWidths(Table table, int availableWidth)
    {
        var options = CreateRenderOptions(availableWidth);
        var measurer = new TableMeasurer(table, options);
        var totalCellWidth = measurer.CalculateTotalCellWidth(availableWidth);
        return measurer.CalculateColumnWidths(totalCellWidth);
    }

    private static RenderOptions CreateRenderOptions(int width)
    {
        return new RenderOptions(new Capabilities(), new Size(width, 24));
    }
}