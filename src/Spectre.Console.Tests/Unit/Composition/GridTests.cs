using System;
using Shouldly;
using Xunit;

namespace Spectre.Console.Tests.Unit.Composition
{
    public sealed class GridTests
    {
        public sealed class TheAddRowMethod
        {
            [Fact]
            public void Should_Throw_If_Rows_Are_Null()
            {
                // Given
                var grid = new Grid();

                // When
                var result = Record.Exception(() => grid.AddRow(null));

                // Then
                result.ShouldBeOfType<ArgumentNullException>()
                    .ParamName.ShouldBe("columns");
            }

            [Fact]
            public void Should_Throw_If_Row_Columns_Is_Less_Than_Number_Of_Columns()
            {
                // Given
                var grid = new Grid();
                grid.AddColumns(2);

                // When
                var result = Record.Exception(() => grid.AddRow("Foo"));

                // Then
                result.ShouldBeOfType<InvalidOperationException>();
                result.Message.ShouldBe("The number of row columns are less than the number of grid columns.");
            }

            [Fact]
            public void Should_Throw_If_Row_Columns_Are_Greater_Than_Number_Of_Columns()
            {
                // Given
                var grid = new Grid();
                grid.AddColumn();

                // When
                var result = Record.Exception(() => grid.AddRow("Foo", "Bar"));

                // Then
                result.ShouldBeOfType<InvalidOperationException>();
                result.Message.ShouldBe("The number of row columns are greater than the number of grid columns.");
            }
        }

        [Fact]
        public void Should_Render_Grid_With_No_Border_Correctly()
        {
            // Given
            var console = new PlainConsole(width: 80);
            var grid = new Grid();
            grid.AddColumns(3);
            grid.AddRow("Qux", "Corgi", "Waldo");
            grid.AddRow("Grault", "Garply", "Fred");

            // When
            console.Render(grid);

            // Then
            console.Lines.Count.ShouldBe(2);
            console.Lines[0].ShouldBe("Qux     Corgi   Waldo");
            console.Lines[1].ShouldBe("Grault  Garply  Fred ");
        }

        [Fact]
        public void Should_Render_Grid()
        {
            var console = new PlainConsole(width: 120);
            var grid = new Grid();
            grid.AddColumns(3);
            grid.AddRow("[bold]Options[/]", string.Empty, string.Empty);
            grid.AddRow("  [blue]-h[/], [blue]--help[/]", "   ", "Show command line help.");
            grid.AddRow("  [blue]-c[/], [blue]--configuration[/]", "   ", "The configuration to run for.\nThe default for most projects is [green]Debug[/].");

            // When
            console.Render(grid);

            // Then
            console.Lines.Count.ShouldBe(4);
            console.Lines[0].ShouldBe("Options                                                         ");
            console.Lines[1].ShouldBe("  -h, --help             Show command line help.                ");
            console.Lines[2].ShouldBe("  -c, --configuration    The configuration to run for.          ");
            console.Lines[3].ShouldBe("                         The default for most projects is Debug.");
        }
    }
}
