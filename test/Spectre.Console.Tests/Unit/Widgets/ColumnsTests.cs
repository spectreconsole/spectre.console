using System.Collections.Generic;
using System.Threading.Tasks;
using Spectre.Console.Testing;
using Spectre.Verify.Extensions;
using VerifyXunit;
using Xunit;

namespace Spectre.Console.Tests.Unit
{
    [UsesVerify]
    [ExpectationPath("Widgets/Columns")]
    public sealed class ColumnsTests
    {
        private sealed class User
        {
            public string Name { get; set; }
            public string Country { get; set; }
        }

        [Fact]
        [Expectation("Render")]
        public Task Should_Render_Columns_Correctly()
        {
            // Given
            var console = new TestConsole().Width(61);
            var users = new[]
            {
                new User { Name = "Savannah Thompson", Country = "Australia" },
                new User { Name = "Sophie Ramos", Country = "United States" },
                new User { Name = "Katrin Goldberg", Country = "Germany" },
            };

            var cards = new List<Panel>();
            foreach (var user in users)
            {
                cards.Add(
                    new Panel($"[b]{user.Name}[/]\n[yellow]{user.Country}[/]")
                        .RoundedBorder().Expand());
            }

            // When
            console.Write(new Columns(cards));

            // Then
            return Verifier.Verify(console.Output);
        }
    }
}
