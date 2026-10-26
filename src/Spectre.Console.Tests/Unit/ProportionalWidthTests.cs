namespace Spectre.Console.Tests.Unit;

public sealed class ProportionalWidthTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(double.NaN)]
    [InlineData(double.PositiveInfinity)]
    [InlineData(double.NegativeInfinity)]
    public void Should_Reject_Invalid_Weights(double weight)
    {
        // When
        var result = Record.Exception(() => new ProportionalWidth(weight));

        // Then
        result.ShouldBeOfType<ArgumentOutOfRangeException>()
            .ParamName.ShouldBe("weight");
    }

    [Fact]
    public void Should_Reject_Default_Value_When_Applied()
    {
        // Given
        var column = new TableColumn("Column");

        // When
        var result = Record.Exception(() => column.Width(default(ProportionalWidth)));

        // Then
        result.ShouldBeOfType<ArgumentOutOfRangeException>()
            .ParamName.ShouldBe("weight");
    }

    [Fact]
    public void Should_Replace_Fixed_Width_With_Proportional_Width()
    {
        // Given
        var column = new TableColumn("Column").Width(10);

        // When
        column.Width(new ProportionalWidth(2));

        // Then
        column.Width.ShouldBeNull();
        column.ProportionalWidth.ShouldBe(new ProportionalWidth(2));
    }

    [Fact]
    public void Should_Replace_Proportional_Width_With_Fixed_Width()
    {
        // Given
        var column = new TableColumn("Column").Width(new ProportionalWidth(2));

        // When
        column.Width = 10;

        // Then
        column.Width.ShouldBe(10);
        column.ProportionalWidth.ShouldBeNull();
    }

    [Fact]
    public void Should_Replace_Proportional_Width_With_Content_Sizing()
    {
        // Given
        var column = new TableColumn("Column").Width(new ProportionalWidth(2));

        // When
        column.Width(null);

        // Then
        column.Width.ShouldBeNull();
        column.ProportionalWidth.ShouldBeNull();
    }
}