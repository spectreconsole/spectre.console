namespace Spectre.Console.Tests.Unit;

[ExpectationPath("Widgets/Table/Rows")]
public sealed class TableRowCollectionTests
{
    [UsesVerify]
    public sealed class TheAddMethod
    {
        [Fact]
        public void Should_Throw_If_Columns_Are_Null()
        {
            // Given
            var table = new Table();

            // When
            var result = Record.Exception(() => table.Rows.Add(null));

            // Then
            result.ShouldBeOfType<ArgumentNullException>()
                .ParamName.ShouldBe("columns");
        }

        [Fact]
        public void Should_Add_Row_To_Collection()
        {
            // Given
            var table = new Table();
            table.AddColumn("Column #1");

            // When
            table.Rows.Add(new[] { Text.Empty });

            // Then
            table.Rows.Count.ShouldBe(1);
        }

        [Fact]
        public void Should_Return_Index_Of_Added_Row()
        {
            // Given
            var table = new Table();
            table.AddColumn("Column #1");
            table.Rows.Add(new[] { Text.Empty });

            // When
            var result = table.Rows.Add(new[] { Text.Empty });

            // Then
            result.ShouldBe(1);
        }

        [Fact]
        [Expectation("Add")]
        public Task Should_Add_Item_At_Correct_Place()
        {
            // Given
            var console = new TestConsole();
            var table = new Table();
            table.AddColumn("Column #1");
            table.Rows.Add(new[] { new Text("1") });
            table.Rows.Add(new[] { new Text("2") });
            table.Rows.Add(new[] { new Text("3") });

            // When
            console.Write(table);

            // Then
            return Verifier.Verify(console.Output);
        }
    }

    [UsesVerify]
    public sealed class TheInsertMethod
    {
        [Fact]
        public void Should_Throw_If_Columns_Are_Null()
        {
            // Given
            var table = new Table();

            // When
            var result = Record.Exception(() => table.Rows.Insert(0, null));

            // Then
            result.ShouldBeOfType<ArgumentNullException>()
                .ParamName.ShouldBe("columns");
        }

        [Fact]
        public void Should_Insert_Row()
        {
            // Given
            var table = new Table();
            table.AddColumn("Column #1");
            table.Rows.Add(new[] { Text.Empty });

            // When
            table.Rows.Insert(0, new[] { Text.Empty });

            // Then
            table.Rows.Count.ShouldBe(2);
        }

        [Fact]
        public void Should_Return_Index_Of_Inserted_Row()
        {
            // Given
            var table = new Table();
            table.AddColumn("Column #1");
            table.Rows.Add(new[] { new Text("1") });
            table.Rows.Add(new[] { new Text("2") });

            // When
            var result = table.Rows.Insert(1, new[] { new Text("3") });

            // Then
            result.ShouldBe(1);
        }

        [Fact]
        [Expectation("Insert")]
        public Task Should_Insert_Item_At_Correct_Place()
        {
            // Given
            var console = new TestConsole();
            var table = new Table();
            table.AddColumn("Column #1");
            table.Rows.Add(new[] { new Text("1") });
            table.Rows.Add(new[] { new Text("2") });
            table.Rows.Insert(1, new[] { new Text("3") });

            // When
            console.Write(table);

            // Then
            return Verifier.Verify(console.Output);
        }
    }

    [UsesVerify]
    public sealed class TheRemoveMethod
    {
        [Fact]
        public void Should_Throw_If_Index_Is_Negative()
        {
            // Given
            var table = new Table();
            table.AddColumn("Column #1");

            // When
            var result = Record.Exception(() => table.Rows.RemoveAt(-1));

            // Then
            result.ShouldBeOfType<IndexOutOfRangeException>()
                .Message.ShouldBe("Table row index cannot be negative.");
        }

        [Fact]
        public void Should_Throw_If_Index_Is_Larger_Than_Number_Of_Rows()
        {
            // Given
            var table = new Table();
            table.AddColumn("Column #1");
            table.Rows.Add(new[] { new Text("1") });
            table.Rows.Add(new[] { new Text("2") });
            table.Rows.Add(new[] { new Text("3") });

            // When
            var result = Record.Exception(() => table.Rows.RemoveAt(3));

            // Then
            result.ShouldBeOfType<IndexOutOfRangeException>()
                .Message.ShouldBe("Table row index cannot exceed the number of rows in the table.");
        }

        [Fact]
        [Expectation("Remove")]
        public Task Should_Remove_Row()
        {
            // Given
            var console = new TestConsole();
            var table = new Table();
            table.AddColumn("Column #1");
            table.Rows.Add(new[] { new Text("1") });
            table.Rows.Add(new[] { new Text("2") });
            table.Rows.Add(new[] { new Text("3") });
            table.Rows.RemoveAt(1);

            // When
            console.Write(table);

            // Then
            return Verifier.Verify(console.Output);
        }
    }

    public sealed class TheClearMethod
    {
        [Fact]
        public void Should_Remove_All_Rows()
        {
            // Given
            var table = new Table();
            table.AddColumn("Column #1");
            table.Rows.Add(new[] { new Text("1") });
            table.Rows.Add(new[] { new Text("2") });
            table.Rows.Add(new[] { new Text("3") });
            table.Rows.Clear();

            // When
            var result = table.Rows.Count;

            // Then
            result.ShouldBe(0);
        }
    }

    [UsesVerify]
    public sealed class TheUpdateMethod
    {
        [Fact]
        public Task Should_Update_Row_With_String()
        {
            // Given
            var console = new TestConsole();
            var table = new Table();
            table.AddColumn("Column #1");
            table.AddColumn("Column #2");
            table.AddColumn("Column #3");
            table.Rows.Add(new[] { new Text("1") });
            table.Rows.Add(new[] { new Text("2") });
            table.Rows.Add(new[] { new Text("3"), new Text("4"), new Text("8") });

            table.UpdateCell(2, 2, "5");

            // When
            console.Write(table);

            // Then
            return Verifier.Verify(console.Output);
        }

        [Fact]
        public Task Should_Update_Row_With_Renderable()
        {
            // Given
            var console = new TestConsole();
            var table = new Table();
            table.AddColumn("Column #1");
            table.AddColumn("Column #2");
            table.AddColumn("Column #3");
            table.Rows.Add(new[] { new Text("1") });
            table.Rows.Add(new[] { new Text("2") });
            table.Rows.Add(new[] { new Text("3"), new Text("4"), new Text("8") });

            table.UpdateCell(2, 2, new Markup("5"));

            // When
            console.Write(table);

            // Then
            return Verifier.Verify(console.Output);
        }

        [Fact]
        public void Should_Throw_If_Index_Is_Larger_Than_Number_Of_Rows()
        {
            // Given
            var console = new TestConsole();
            var table = new Table();
            table.AddColumn("Column #1");
            table.AddColumn("Column #2");
            table.AddColumn("Column #3");
            table.Rows.Add(new[] { new Text("1") });
            table.Rows.Add(new[] { new Text("2") });
            table.Rows.Add(new[] { new Text("3"), new Text("4"), new Text("8") });
            table.UpdateCell(2, 2, "5");

            // When
            var result = Record.Exception(() => table.UpdateCell(5, 2, "5"));

            // Then
            result.ShouldBeOfType<IndexOutOfRangeException>()
                .Message.ShouldBe("Table row index cannot exceed the number of rows in the table.");
        }

        [Fact]
        public void Should_Throw_If_Index_Is_Larger_Than_Number_Of_Columns()
        {
            // Given
            var console = new TestConsole();
            var table = new Table();
            table.AddColumn("Column #1");
            table.AddColumn("Column #2");
            table.AddColumn("Column #3");
            table.Rows.Add(new[] { new Text("1") });
            table.Rows.Add(new[] { new Text("2") });
            table.Rows.Add(new[] { new Text("3"), new Text("4"), new Text("8") });
            table.UpdateCell(2, 2, "5");

            // When
            var result = Record.Exception(() => table.UpdateCell(2, 5, "5"));

            // Then
            result.ShouldBeOfType<IndexOutOfRangeException>()
                .Message.ShouldBe("Table column index cannot exceed the number of rows in the table.");
        }

        [Fact]
        public void Should_Throw_If_Index_Row_Is_Negative()
        {
            // Given
            var console = new TestConsole();
            var table = new Table();
            table.AddColumn("Column #1");
            table.AddColumn("Column #2");
            table.AddColumn("Column #3");
            table.Rows.Add(new[] { new Text("1") });
            table.Rows.Add(new[] { new Text("2") });
            table.Rows.Add(new[] { new Text("3"), new Text("4"), new Text("8") });
            table.UpdateCell(2, 2, "5");

            // When
            var result = Record.Exception(() => table.UpdateCell(-1, 2, "5"));

            // Then
            result.ShouldBeOfType<IndexOutOfRangeException>()
                .Message.ShouldBe("Table row index cannot be negative.");
        }

        [Fact]
        public void Should_Throw_If_Index_Column_Is_Negative()
        {
            // Given
            var console = new TestConsole();
            var table = new Table();
            table.AddColumn("Column #1");
            table.AddColumn("Column #2");
            table.AddColumn("Column #3");
            table.Rows.Add(new[] { new Text("1") });
            table.Rows.Add(new[] { new Text("2") });
            table.Rows.Add(new[] { new Text("3"), new Text("4"), new Text("8") });
            table.UpdateCell(2, 2, "5");

            // When
            var result = Record.Exception(() => table.UpdateCell(2, -1, "5"));

            // Then
            result.ShouldBeOfType<IndexOutOfRangeException>()
                .Message.ShouldBe("Table column index cannot be negative.");
        }
    }
}
