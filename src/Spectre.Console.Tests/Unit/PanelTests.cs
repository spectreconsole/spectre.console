using System.Collections.Generic;
using System.Threading.Tasks;
using Spectre.Console.Rendering;
using VerifyXunit;
using Xunit;

namespace Spectre.Console.Tests.Unit
{
    [UsesVerify]
    public sealed class PanelTests
    {
        [Fact]
        public Task Should_Render_Panel()
        {
            // Given
            var console = new PlainConsole(width: 80);

            // When
            console.Render(new Panel(new Text("Hello World")));

            // Then
            return Verifier.Verify(console.Lines);
        }

        [Fact]
        public Task Should_Render_Panel_With_Padding_Set_To_Zero()
        {
            // Given
            var console = new PlainConsole(width: 80);

            // When
            console.Render(new Panel(new Text("Hello World"))
            {
                Padding = new Padding(0, 0, 0, 0),
            });

            // Then
            return Verifier.Verify(console.Lines);
        }

        [Fact]
        public Task Should_Render_Panel_With_Padding()
        {
            // Given
            var console = new PlainConsole(width: 80);

            // When
            console.Render(new Panel(new Text("Hello World"))
            {
                Padding = new Padding(3, 1, 5, 2),
            });

            // Then
            return Verifier.Verify(console.Lines);
        }

        [Fact]
        public Task Should_Render_Panel_With_Header()
        {
            // Given
            var console = new PlainConsole(width: 80);

            // When
            console.Render(new Panel("Hello World")
            {
                Header = new PanelHeader("Greeting"),
                Expand = true,
                Padding = new Padding(2, 0, 2, 0),
            });

            // Then
            return Verifier.Verify(console.Lines);
        }

        [Fact]
        public Task Should_Render_Panel_With_Left_Aligned_Header()
        {
            // Given
            var console = new PlainConsole(width: 80);

            // When
            console.Render(new Panel("Hello World")
            {
                Header = new PanelHeader("Greeting").LeftAligned(),
                Expand = true,
            });

            // Then
            return Verifier.Verify(console.Lines);
        }

        [Fact]
        public Task Should_Render_Panel_With_Centered_Header()
        {
            // Given
            var console = new PlainConsole(width: 80);

            // When
            console.Render(new Panel("Hello World")
            {
                Header = new PanelHeader("Greeting").Centered(),
                Expand = true,
            });

            // Then
            return Verifier.Verify(console.Lines);
        }

        [Fact]
        public Task Should_Render_Panel_With_Right_Aligned_Header()
        {
            // Given
            var console = new PlainConsole(width: 80);

            // When
            console.Render(new Panel("Hello World")
            {
                Header = new PanelHeader("Greeting").RightAligned(),
                Expand = true,
            });

            // Then
            return Verifier.Verify(console.Lines);
        }

        [Fact]
        public Task Should_Collapse_Header_If_It_Will_Not_Fit()
        {
            // Given
            var console = new PlainConsole(width: 10);

            // When
            console.Render(new Panel("Hello World")
            {
                Header = new PanelHeader("Greeting"),
                Expand = true,
            });

            // Then
            return Verifier.Verify(console.Lines);
        }

        [Fact]
        public Task Should_Render_Panel_With_Unicode_Correctly()
        {
            // Given
            var console = new PlainConsole(width: 80);

            // When
            console.Render(new Panel(new Text(" \n💩\n ")));

            // Then
            return Verifier.Verify(console.Lines);
        }

        [Fact]
        public Task Should_Render_Panel_With_Multiple_Lines()
        {
            // Given
            var console = new PlainConsole(width: 80);

            // When
            console.Render(new Panel(new Text("Hello World\nFoo Bar")));

            // Then
            return Verifier.Verify(console.Lines);
        }

        [Fact]
        public Task Should_Preserve_Explicit_Line_Ending()
        {
            // Given
            var console = new PlainConsole(width: 80);
            var text = new Panel(
                new Markup("I heard [underline on blue]you[/] like 📦\n\n\n\nSo I put a 📦 in a 📦"));

            // When
            console.Render(text);

            // Then
            return Verifier.Verify(console.Lines);
        }

        [Fact]
        public Task Should_Expand_Panel_If_Enabled()
        {
            // Given
            var console = new PlainConsole(width: 80);

            // When
            console.Render(new Panel(new Text("Hello World"))
            {
                Expand = true,
            });

            // Then
            return Verifier.Verify(console.Lines);
        }

        [Fact]
        public Task Should_Justify_Child_To_Right_Correctly()
        {
            // Given
            var console = new PlainConsole(width: 25);

            // When
            console.Render(
                new Panel(new Text("Hello World").RightAligned())
                {
                    Expand = true,
                });

            // Then
            return Verifier.Verify(console.Lines);
        }

        [Fact]
        public Task Should_Center_Child_Correctly()
        {
            // Given
            var console = new PlainConsole(width: 25);

            // When
            console.Render(
                new Panel(new Text("Hello World").Centered())
                {
                    Expand = true,
                });

            // Then
            return Verifier.Verify(console.Lines);
        }

        [Fact]
        public Task Should_Render_Panel_Inside_Panel_Correctly()
        {
            // Given
            var console = new PlainConsole(width: 80);

            // When
            console.Render(new Panel(new Panel(new Text("Hello World"))));

            // Then
            return Verifier.Verify(console.Lines);
        }

        [Fact]
        public Task Should_Wrap_Content_Correctly()
        {
            // Given
            var console = new PlainConsole(width: 84);
            var rows = new List<IRenderable>();
            var grid = new Grid();
            grid.AddColumn(new GridColumn().PadLeft(2).PadRight(0));
            grid.AddColumn(new GridColumn().PadLeft(1).PadRight(0));
            grid.AddRow("at", "[grey]System.Runtime.CompilerServices.TaskAwaiter.[/][yellow]HandleNonSuccessAndDebuggerNotification[/]([blue]Task[/] task)");
            rows.Add(grid);

            var panel = new Panel(grid)
                .Expand().RoundedBorder()
                .BorderStyle(new Style().Foreground(Color.Grey))
                .Header("[grey]Short paths[/]");

            // When
            console.Render(panel);

            // Then
            return Verifier.Verify(console.Lines);
        }
    }
}
