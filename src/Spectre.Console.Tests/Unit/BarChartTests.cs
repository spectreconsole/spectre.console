using System.Threading.Tasks;
using Spectre.Console.Testing;
using VerifyXunit;
using Xunit;

namespace Spectre.Console.Tests.Unit
{
    [UsesVerify]
    public sealed class BarChartTests
    {
        [Fact]
        public async Task Should_Render_Correctly()
        {
            // Given
            var console = new FakeConsole(width: 80);

            // When
            console.Render(new BarChart()
                .Width(60)
                .Label("Number of fruits")
                .AddItem("Apple", 12)
                .AddItem("Orange", 54)
                .AddItem("Banana", 33));

            // Then
            await Verifier.Verify(console.Output);
        }
    }
}
