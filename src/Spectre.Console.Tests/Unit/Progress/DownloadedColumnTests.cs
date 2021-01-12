using System.Globalization;
using Shouldly;
using Xunit;

namespace Spectre.Console.Tests.Unit
{
    public sealed class DownloadedColumnTests
    {
        [Theory]
        [InlineData(0, 1, "0/1 byte")]
        [InlineData(37, 101, "37/101 bytes")]
        [InlineData(101, 101, "101 bytes")]
        [InlineData(512, 1024, "0.5/1.0 KB")]
        [InlineData(1024, 1024, "1.0 KB")]
        [InlineData(1024 * 512, 5 * 1024 * 1024, "0.5/5.0 MB")]
        [InlineData(5 * 1024 * 1024, 5 * 1024 * 1024, "5.0 MB")]
        public void Should_Return_Correct_Value(double value, double total, string expected)
        {
            // Given
            var fixture = new ProgressColumnFixture<DownloadedColumn>(value, total);
            fixture.Column.Culture = CultureInfo.InvariantCulture;

            // When
            var result = fixture.Render();

            // Then
            result.ShouldBe(expected);
        }
    }
}
