namespace Spectre.Console.Tests.Unit;

public sealed class DownloadedColumnTests
{
    [Theory]
    [InlineData(0, 1, "0/1 byte")]
    [InlineData(37, 101, "37/101 bytes")]
    [InlineData(101, 101, "101 bytes")]
    [InlineData(512, 1024, "0.5/1.0 KiB")]
    [InlineData(1024, 1024, "1.0 KiB")]
    [InlineData(1024 * 512, 5 * 1024 * 1024, "0.5/5.0 MiB")]
    [InlineData(5 * 1024 * 1024, 5 * 1024 * 1024, "5.0 MiB")]
    public void Binary_Unit_In_Bytes_Should_Return_Expected(double value, double total, string expected)
    {
        // Given
        var fixture = new ProgressColumnFixture<DownloadedColumn>(value, total);
        fixture.Column.Culture = CultureInfo.InvariantCulture;
        fixture.Column.Base = FileSizeBase.Binary;
        fixture.Column.ShowBits = false;

        // When
        var result = fixture.Render();

        // Then
        result.ShouldBe(expected);
    }

    [Theory]
    [InlineData(512, 1024, "4.0/8.0 Kibit")]
    [InlineData(1024, 1024, "8.0 Kibit")]
    public void Binary_Unit_In_Bits_Should_Return_Expected(double value, double total, string expected)
    {
        // Given
        var fixture = new ProgressColumnFixture<DownloadedColumn>(value, total);
        fixture.Column.Culture = CultureInfo.InvariantCulture;
        fixture.Column.Base = FileSizeBase.Binary;
        fixture.Column.ShowBits = true;

        // When
        var result = fixture.Render();

        // Then
        result.ShouldBe(expected);
    }

    [Theory]
    [InlineData(500, 1000, "0.5/1.0 KB")]
    [InlineData(1000, 1000, "1.0 KB")]
    public void Decimal_Unit_In_Bytes_Should_Return_Expected(double value, double total, string expected)
    {
        // Given
        var fixture = new ProgressColumnFixture<DownloadedColumn>(value, total);
        fixture.Column.Culture = CultureInfo.InvariantCulture;
        fixture.Column.Base = FileSizeBase.Decimal;
        fixture.Column.ShowBits = false;

        // When
        var result = fixture.Render();

        // Then
        result.ShouldBe(expected);
    }

    [Theory]
    [InlineData(500, 1000, "4.0/8.0 Kbit")]
    [InlineData(1000, 1000, "8.0 Kbit")]
    public void Decimal_Unit_In_Bits_Should_Return_Expected(double value, double total, string expected)
    {
        // Given
        var fixture = new ProgressColumnFixture<DownloadedColumn>(value, total);
        fixture.Column.Culture = CultureInfo.InvariantCulture;
        fixture.Column.Base = FileSizeBase.Decimal;
        fixture.Column.ShowBits = true;

        // When
        var result = fixture.Render();

        // Then
        result.ShouldBe(expected);
    }
}
