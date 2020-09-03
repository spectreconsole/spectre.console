using System.Collections.Generic;
using Shouldly;
using Xunit;

namespace Spectre.Console.Tests.Unit
{
    public sealed class ColumnsTests
    {
        private sealed class User
        {
            public string Name { get; set; }
            public string Country { get; set; }
        }

        [Fact]
        public void Should_Render_Columns_Correctly()
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
            console.Lines.Count.ShouldBe(4);
            console.Lines[0].ShouldBe("╭────────────────────╮ ╭────────────────╮ ╭─────────────────╮");
            console.Lines[1].ShouldBe("│ Savannah Thompson  │ │ Sophie Ramos   │ │ Katrin Goldberg │");
            console.Lines[2].ShouldBe("│ Australia          │ │ United States  │ │ Germany         │");
            console.Lines[3].ShouldBe("╰────────────────────╯ ╰────────────────╯ ╰─────────────────╯");
        }
    }
}
