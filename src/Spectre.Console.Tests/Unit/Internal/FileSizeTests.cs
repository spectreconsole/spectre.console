namespace Spectre.Console.Tests.Unit.Internal;

public sealed class FileSizeTests
{
    [Theory]
    [InlineData(0, "0 bytes")]
    [InlineData(37, "37 bytes")]
    [InlineData(512, "512 bytes")]
    [InlineData(15 * 1024, "15.0 KiB")]
    [InlineData(1024 * 512, "512.0 KiB")]
    [InlineData(5 * 1024 * 1024, "5.0 MiB")]
    [InlineData(9 * 1024 * 1024, "9.0 MiB")]
    public void Binary_Unit_In_Bytes_Should_Return_Expected(double bytes, string expected)
    {
        // Given
        var filesize = new FileSize(bytes, FileSizeBase.Binary);

        // When
        var result = filesize.ToString();

        // Then
        result.ShouldBe(expected);
    }

    [Theory]
    [InlineData(0, "0 bits")]
    [InlineData(37, "296 bits")]
    [InlineData(512, "4.0 Kibit")]
    [InlineData(15 * 1024, "120.0 Kibit")]
    [InlineData(1024 * 512, "4.0 Mibit")]
    [InlineData(5 * 1024 * 1024, "40.0 Mibit")]
    [InlineData(210 * 1024 * 1024, "1.6 Gibit")]
    [InlineData(900 * 1024 * 1024, "7.0 Gibit")]
    public void Binary_Unit_In_Bits_Should_Return_Expected(double bytes, string expected)
    {
        // Given
        var filesize = new FileSize(bytes, FileSizeBase.Binary, showBits: true);

        // When
        var result = filesize.ToString();

        // Then
        result.ShouldBe(expected);
    }

    [Theory]
    [InlineData(0, "0 bytes")]
    [InlineData(37, "37 bytes")]
    [InlineData(512, "512 bytes")]
    [InlineData(15 * 1024, "15.4 KB")]
    [InlineData(1024 * 512, "524.3 KB")]
    [InlineData(5 * 1024 * 1024, "5.2 MB")]
    [InlineData(9 * 1024 * 1024, "9.4 MB")]
    public void Decimal_Unit_In_Bytes_Should_Return_Expected(double bytes, string expected)
    {
        // Given
        var filesize = new FileSize(bytes, FileSizeBase.Decimal);

        // When
        var result = filesize.ToString();

        // Then
        result.ShouldBe(expected);
    }

    [Theory]
    [InlineData(0, "0 bits")]
    [InlineData(37, "296 bits")]
    [InlineData(512, "4.1 Kbit")]
    [InlineData(15 * 1024, "122.9 Kbit")]
    [InlineData(1024 * 512, "4.2 Mbit")]
    [InlineData(5 * 1024 * 1024, "41.9 Mbit")]
    [InlineData(900 * 1024 * 1024, "7.5 Gbit")]
    public void Decimal_Unit_In_Bits_Should_Return_Expected(double bytes, string expected)
    {
        // Given
        var filesize = new FileSize(bytes, FileSizeBase.Decimal, showBits: true);

        // When
        var result = filesize.ToString();

        // Then
        result.ShouldBe(expected);
    }
}