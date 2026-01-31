namespace Spectre.Console.Tests.Unit;

public sealed class TableTests
{
    public sealed class TheExpandMethod
    {
        [Fact]
        public void Should_Not_Expand_Fixed_Width_Columns()
        {
            // Given
            var console = new TestConsole().Width(40);
            var table = new Table()
                .Expand()
                .HideHeaders()
                .HideFooters();

            table.AddColumn(new TableColumn("Time").Width(12));
            table.AddColumn("Message");
            table.AddRow("12:00", "Hello");

            // When
            console.Write(table);

            // Then
            var line = console.Lines.Single(l => l.Contains("12:00"));
            var firstSeparator = line.IndexOf('│');
            var secondSeparator = line.IndexOf('│', firstSeparator + 1);

            var firstColumnWidth = secondSeparator - firstSeparator - 1;
            firstColumnWidth.ShouldBe(14);
        }
    }
}
