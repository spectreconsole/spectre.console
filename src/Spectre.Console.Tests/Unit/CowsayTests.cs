using System.Threading.Tasks;
using Spectre.Console.Testing;
using VerifyXunit;
using Xunit;

namespace Spectre.Console.Tests.Unit
{
    [UsesVerify]
    public class CowsayTests
    {
        [Fact]
        public Task Simple()
        {
            // Given
            var console = new FakeConsole(width: 80);
            var cowsay = new Cowsay(new Text("Cow says moo"));

            // When
            console.Render(cowsay);

            // Then
            return Verifier.Verify(console.Output);
        }
    }
}