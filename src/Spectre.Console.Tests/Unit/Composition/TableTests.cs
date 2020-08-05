using System;
using Shouldly;
using Xunit;

namespace Spectre.Console.Tests.Unit.Composition
{
    public sealed class TableTests
    {
        public sealed class TheAddColumnMethod
        {
            [Fact]
            public void Should_Throw_If_Column_Is_Null()
            {
                // Given
                var table = new Table();

                // When
                var result = Record.Exception(() => table.AddColumn((string)null));

                // Then
                result.ShouldBeOfType<ArgumentNullException>()
                    .ParamName.ShouldBe("column");
            }
        }

        public sealed class TheAddColumnsMethod
        {
            [Fact]
            public void Should_Throw_If_Columns_Are_Null()
            {
                // Given
                var table = new Table();

                // When
                var result = Record.Exception(() => table.AddColumns(null));

                // Then
                result.ShouldBeOfType<ArgumentNullException>()
                    .ParamName.ShouldBe("columns");
            }
        }

        public sealed class TheAddRowMethod
        {
            [Fact]
            public void Should_Throw_If_Rows_Are_Null()
            {
                // Given
                var table = new Table();

                // When
                var result = Record.Exception(() => table.AddRow(null));

                // Then
                result.ShouldBeOfType<ArgumentNullException>()
                    .ParamName.ShouldBe("columns");
            }

            [Fact]
            public void Should_Throw_If_Row_Columns_Is_Less_Than_Number_Of_Columns()
            {
                // Given
                var table = new Table();
                table.AddColumn("Hello");
                table.AddColumn("World");

                // When
                var result = Record.Exception(() => table.AddRow("Foo"));

                // Then
                result.ShouldBeOfType<InvalidOperationException>();
                result.Message.ShouldBe("The number of row columns are less than the number of table columns.");
            }

            [Fact]
            public void Should_Throw_If_Row_Columns_Are_Greater_Than_Number_Of_Columns()
            {
                // Given
                var table = new Table();
                table.AddColumn("Hello");

                // When
                var result = Record.Exception(() => table.AddRow("Foo", "Bar"));

                // Then
                result.ShouldBeOfType<InvalidOperationException>();
                result.Message.ShouldBe("The number of row columns are greater than the number of table columns.");
            }
        }

        [Fact]
        public void Should_Measure_Table_Correctly()
        {
            // Given
            var console = new PlainConsole(width: 80);
            var table = new Table();
            table.AddColumns("Foo", "Bar", "Baz");
            table.AddRow("Qux", "Corgi", "Waldo");
            table.AddRow("Grault", "Garply", "Fred");

            // When
            console.Render(new Panel(table));

            // Then
            console.Lines.Count.ShouldBe(8);
            console.Lines[0].ShouldBe("┌─────────────────────────────┐");
            console.Lines[1].ShouldBe("│ ┌────────┬────────┬───────┐ │");
            console.Lines[2].ShouldBe("│ │ Foo    │ Bar    │ Baz   │ │");
            console.Lines[3].ShouldBe("│ ├────────┼────────┼───────┤ │");
            console.Lines[4].ShouldBe("│ │ Qux    │ Corgi  │ Waldo │ │");
            console.Lines[5].ShouldBe("│ │ Grault │ Garply │ Fred  │ │");
            console.Lines[6].ShouldBe("│ └────────┴────────┴───────┘ │");
            console.Lines[7].ShouldBe("└─────────────────────────────┘");
        }

        [Fact]
        public void Should_Render_Table_Correctly()
        {
            // Given
            var console = new PlainConsole(width: 80);
            var table = new Table();
            table.AddColumns("Foo", "Bar", "Baz");
            table.AddRow("Qux", "Corgi", "Waldo");
            table.AddRow("Grault", "Garply", "Fred");

            // When
            console.Render(table);

            // Then
            console.Lines.Count.ShouldBe(6);
            console.Lines[0].ShouldBe("┌────────┬────────┬───────┐");
            console.Lines[1].ShouldBe("│ Foo    │ Bar    │ Baz   │");
            console.Lines[2].ShouldBe("├────────┼────────┼───────┤");
            console.Lines[3].ShouldBe("│ Qux    │ Corgi  │ Waldo │");
            console.Lines[4].ShouldBe("│ Grault │ Garply │ Fred  │");
            console.Lines[5].ShouldBe("└────────┴────────┴───────┘");
        }

        [Fact]
        public void Should_Expand_Table_To_Available_Space_If_Specified()
        {
            // Given
            var console = new PlainConsole(width: 80);
            var table = new Table() { Expand = true };
            table.AddColumns("Foo", "Bar", "Baz");
            table.AddRow("Qux", "Corgi", "Waldo");
            table.AddRow("Grault", "Garply", "Fred");

            // When
            console.Render(table);

            // Then
            console.Lines.Count.ShouldBe(6);
            console.Lines[0].Length.ShouldBe(80);
            console.Lines[0].ShouldBe("┌───────────────────────────┬───────────────────────────┬──────────────────────┐");
            console.Lines[1].ShouldBe("│ Foo                       │ Bar                       │ Baz                  │");
            console.Lines[2].ShouldBe("├───────────────────────────┼───────────────────────────┼──────────────────────┤");
            console.Lines[3].ShouldBe("│ Qux                       │ Corgi                     │ Waldo                │");
            console.Lines[4].ShouldBe("│ Grault                    │ Garply                    │ Fred                 │");
            console.Lines[5].ShouldBe("└───────────────────────────┴───────────────────────────┴──────────────────────┘");
        }

        [Fact]
        public void Should_Render_Table_With_Ascii_Border_Correctly()
        {
            // Given
            var console = new PlainConsole(width: 80);
            var table = new Table { Border = BorderKind.Ascii };
            table.AddColumns("Foo", "Bar", "Baz");
            table.AddRow("Qux", "Corgi", "Waldo");
            table.AddRow("Grault", "Garply", "Fred");

            // When
            console.Render(table);

            // Then
            console.Lines.Count.ShouldBe(6);
            console.Lines[0].ShouldBe("+-------------------------+");
            console.Lines[1].ShouldBe("| Foo    | Bar    | Baz   |");
            console.Lines[2].ShouldBe("|--------+--------+-------|");
            console.Lines[3].ShouldBe("| Qux    | Corgi  | Waldo |");
            console.Lines[4].ShouldBe("| Grault | Garply | Fred  |");
            console.Lines[5].ShouldBe("+-------------------------+");
        }

        [Fact]
        public void Should_Render_Table_With_Rounded_Border_Correctly()
        {
            // Given
            var console = new PlainConsole(width: 80);
            var table = new Table { Border = BorderKind.Rounded };
            table.AddColumns("Foo", "Bar", "Baz");
            table.AddRow("Qux", "Corgi", "Waldo");
            table.AddRow("Grault", "Garply", "Fred");

            // When
            console.Render(table);

            // Then
            console.Lines.Count.ShouldBe(6);
            console.Lines[0].ShouldBe("╭────────┬────────┬───────╮");
            console.Lines[1].ShouldBe("│ Foo    │ Bar    │ Baz   │");
            console.Lines[2].ShouldBe("├────────┼────────┼───────┤");
            console.Lines[3].ShouldBe("│ Qux    │ Corgi  │ Waldo │");
            console.Lines[4].ShouldBe("│ Grault │ Garply │ Fred  │");
            console.Lines[5].ShouldBe("╰────────┴────────┴───────╯");
        }

        [Fact]
        public void Should_Render_Table_With_No_Border_Correctly()
        {
            // Given
            var console = new PlainConsole(width: 80);
            var table = new Table { Border = BorderKind.None };
            table.AddColumns("Foo", "Bar", "Baz");
            table.AddRow("Qux", "Corgi", "Waldo");
            table.AddRow("Grault", "Garply", "Fred");

            // When
            console.Render(table);

            // Then
            console.Lines.Count.ShouldBe(3);
            console.Lines[0].ShouldBe("Foo     Bar     Baz  ");
            console.Lines[1].ShouldBe("Qux     Corgi   Waldo");
            console.Lines[2].ShouldBe("Grault  Garply  Fred ");
        }

        [Fact]
        public void Should_Render_Table_With_Multiple_Rows_In_Cell_Correctly()
        {
            // Given
            var console = new PlainConsole(width: 80);
            var table = new Table();
            table.AddColumns("Foo", "Bar", "Baz");
            table.AddRow("Qux\nQuuux", "Corgi", "Waldo");
            table.AddRow("Grault", "Garply", "Fred");

            // When
            console.Render(table);

            // Then
            console.Lines.Count.ShouldBe(7);
            console.Lines[0].ShouldBe("┌────────┬────────┬───────┐");
            console.Lines[1].ShouldBe("│ Foo    │ Bar    │ Baz   │");
            console.Lines[2].ShouldBe("├────────┼────────┼───────┤");
            console.Lines[3].ShouldBe("│ Qux    │ Corgi  │ Waldo │");
            console.Lines[4].ShouldBe("│ Quuux  │        │       │");
            console.Lines[5].ShouldBe("│ Grault │ Garply │ Fred  │");
            console.Lines[6].ShouldBe("└────────┴────────┴───────┘");
        }
    }
}
