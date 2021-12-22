namespace Spectre.Console.Tests.Unit;

[UsesVerify]
[ExpectationPath("Widgets/Table/Rows/Extensions")]
public sealed class TableRowCollectionExtensionsTests
{
    [UsesVerify]
    public sealed class TheAddRowMethod
    {
        [Fact]
        [Expectation("Add", "Renderables")]
        public Task Should_Add_Renderables()
        {
            // Given
            var console = new TestConsole();
            var table = new Table();
            table.AddColumn("Column #1");
            table.AddColumn("Column #2");
            table.AddRow(new[] { new Text("1"), new Text("1-2") });
            table.AddRow(new[] { new Text("2"), new Text("2-2") });
            table.AddRow(new[] { new Text("3"), new Text("3-2") });

            // When
            console.Write(table);

            // Then
            return Verifier.Verify(console.Output);
        }

        [Fact]
        [Expectation("Add", "Strings")]
        public Task Should_Add_Strings()
        {
            // Given
            var console = new TestConsole();
            var table = new Table();
            table.AddColumn("Column #1");
            table.AddColumn("Column #2");
            table.AddRow("1", "1-2");
            table.AddRow("2", "2-2");
            table.AddRow("3", "3-2");

            // When
            console.Write(table);

            // Then
            return Verifier.Verify(console.Output);
        }
    }

    [UsesVerify]
    public sealed class TheInsertRowMethod
    {
        [Fact]
        [Expectation("Insert", "Renderables")]
        public Task Should_Insert_Renderables()
        {
            // Given
            var console = new TestConsole();
            var table = new Table();
            table.AddColumn("Column #1");
            table.AddColumn("Column #2");
            table.AddRow(new[] { new Text("1"), new Text("1-2") });
            table.AddRow(new[] { new Text("2"), new Text("2-2") });

            // When
            table.InsertRow(1, new[] { new Text("3"), new Text("3-2") });

            // Then
            console.Write(table);
            return Verifier.Verify(console.Output);
        }

        [Fact]
        [Expectation("Insert", "Strings")]
        public Task Should_Insert_Strings()
        {
            // Given
            var console = new TestConsole();
            var table = new Table();
            table.AddColumn("Column #1");
            table.AddColumn("Column #2");
            table.AddRow("1", "1-2");
            table.AddRow("2", "2-2");

            // When
            table.InsertRow(1, "3", "3-2");

            // Then
            console.Write(table);
            return Verifier.Verify(console.Output);
        }
    }

    [UsesVerify]
    public sealed class TheRemoveRowMethod
    {
        [Fact]
        [Expectation("Remove")]
        public Task Should_Remove_Row()
        {
            // Given
            var console = new TestConsole();
            var table = new Table();
            table.AddColumn("Column #1");
            table.AddColumn("Column #2");
            table.AddRow(new[] { new Text("1"), new Text("1-2") });
            table.AddRow(new[] { new Text("2"), new Text("2-2") });
            table.AddRow(new[] { new Text("3"), new Text("3-2") });

            // When
            table.RemoveRow(1);

            // Then
            console.Write(table);
            return Verifier.Verify(console.Output);
        }
    }
}
