using System.Threading.Tasks;
using Spectre.Console.Testing;
using Spectre.Verify.Extensions;
using VerifyXunit;
using Xunit;

namespace Spectre.Console.Tests.Unit
{
    [UsesVerify]
    [ExpectationPath("Widgets/Status")]
    public sealed class StatusTests
    {
        [Fact]
        [Expectation("Render")]
        public Task Should_Render_Status_Correctly()
        {
            // Given
            var console = new FakeAnsiConsole(ColorSystem.TrueColor, width: 10);

            var status = new Status(console);
            status.AutoRefresh = false;
            status.Spinner = new DummySpinner1();

            // When
            status.Start("foo", ctx =>
            {
                ctx.Refresh();
                ctx.Spinner(new DummySpinner2());
                ctx.Status("bar");
                ctx.Refresh();
                ctx.Spinner(new DummySpinner1());
                ctx.Status("baz");
            });

            // Then
            return Verifier.Verify(console.Output);
        }
    }
}
