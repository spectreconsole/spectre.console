using System.Collections.Generic;
using System.Threading.Tasks;
using VerifyXunit;
using Xunit;

namespace Spectre.Console.Tests.Unit
{
    [UsesVerify]
    public sealed class ColumnsTests
    {
        private sealed class User
        {
            public string Name { get; set; }
            public string Country { get; set; }
        }

        [Fact]
        public Task Should_Render_Columns_Correctly()
        {
            // Given
            var console = new PlainConsole(width: 61);
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
            console.Render(new Columns(cards));

            // Then
            return Verifier.Verify(console.Lines);
        }
    }
}
