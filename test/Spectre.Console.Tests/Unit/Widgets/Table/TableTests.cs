namespace Spectre.Console.Tests.Unit;

[UsesVerify]
[ExpectationPath("Widgets/Table")]
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

        [Fact]
        public void Should_Throw_If_Rows_Are_Not_Empty()
        {
            // Given
            var grid = new Table();
            grid.AddColumn("Foo");
            grid.AddRow("Hello World");

            // When
            var result = Record.Exception(() => grid.AddColumn("Bar"));

            // Then
            result.ShouldBeOfType<InvalidOperationException>()
                .Message.ShouldBe("Cannot add new columns to table with existing rows.");
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
            var result = Record.Exception(() => table.AddColumns((string[])null));

            // Then
            result.ShouldBeOfType<ArgumentNullException>()
                .ParamName.ShouldBe("columns");
        }
    }

    public sealed class TheAddRowMethod
    {
        [Fact]
        public void Should_Throw_If_String_Rows_Are_Null()
        {
            // Given
            var table = new Table();

            // When
            var result = Record.Exception(() => table.AddRow((string[])null));

            // Then
            result.ShouldBeOfType<ArgumentNullException>()
                .ParamName.ShouldBe("columns");
        }

        [Fact]
        public void Should_Add_Empty_Items_If_User_Provides_Less_Row_Items_Than_Columns()
        {
            // Given
            var table = new Table();
            table.AddColumn("Hello");
            table.AddColumn("World");

            // When
            table.AddRow("Foo");

            // Then
            table.Rows.Count.ShouldBe(1);
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

    [UsesVerify]
    public sealed class TheAddEmptyRowMethod
    {
        [Fact]
        [Expectation("AddEmptyRow")]
        public Task Should_Render_Table_Correctly()
        {
            // Given
            var console = new TestConsole();
            var table = new Table();
            table.AddColumns("Foo", "Bar", "Baz");
            table.AddRow("Qux", "Corgi", "Waldo");
            table.AddEmptyRow();
            table.AddRow("Grault", "Garply", "Fred");

            // When
            console.Write(table);

            // Then
            return Verifier.Verify(console.Output);
        }
    }

    [Fact]
    [Expectation("Render")]
    public Task Should_Render_Table_Correctly()
    {
        // Given
        var console = new TestConsole();
        var table = new Table();
        table.AddColumns("Foo", "Bar", "Baz");
        table.AddRow("Qux", "Corgi", "Waldo");
        table.AddRow("Grault", "Garply", "Fred");

        // When
        console.Write(table);

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("Render_EA_Character")]
    public Task Should_Render_Table_With_EA_Character_Correctly()
    {
        // Given
        var console = new TestConsole().Width(48);
        var table = new Table();
        table.AddColumns("Foo", "Bar", "Baz");
        table.AddRow("中文", "日本語", "한국어");
        table.AddRow("这是中文测试字符串", "これは日本語のテスト文字列です", "이것은한국어테스트문자열입니다");

        // When
        console.Write(table);

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("Render_Footers")]
    public Task Should_Render_Table_With_Footers_Correctly()
    {
        // Given
        var console = new TestConsole();
        var table = new Table();
        table.AddColumn(new TableColumn("Foo").Footer("Oof").RightAligned());
        table.AddColumn("Bar");
        table.AddColumns(new TableColumn("Baz").Footer("Zab"));
        table.AddRow("Qux", "Corgi", "Waldo");
        table.AddRow("Grault", "Garply", "Fred");

        // When
        console.Write(table);

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("Render_LeftAligned")]
    public Task Should_Left_Align_Table_Correctly()
    {
        // Given
        var console = new TestConsole();
        var table = new Table();
#pragma warning disable CS0618 // Type or member is obsolete
        table.Alignment = Justify.Left;
#pragma warning restore CS0618 // Type or member is obsolete
        table.AddColumns("Foo", "Bar", "Baz");
        table.AddRow("Qux", "Corgi", "Waldo");
        table.AddRow("Grault", "Garply", "Fred");

        // When
        console.Write(table);

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("Render_LeftAligned", "Align_Widget")]
    public Task Should_Left_Align_Table_Correctly_When_Wrapped_In_Align_Widget()
    {
        // Given
        var console = new TestConsole();
        var table = new Table();
        table.AddColumns("Foo", "Bar", "Baz");
        table.AddRow("Qux", "Corgi", "Waldo");
        table.AddRow("Grault", "Garply", "Fred");

        // When
        console.Write(Align.Left(table));

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("Render_Centered")]
    public Task Should_Center_Table_Correctly()
    {
        // Given
        var console = new TestConsole();
        var table = new Table();
#pragma warning disable CS0618 // Type or member is obsolete
        table.Alignment = Justify.Center;
#pragma warning restore CS0618 // Type or member is obsolete
        table.AddColumns("Foo", "Bar", "Baz");
        table.AddRow("Qux", "Corgi", "Waldo");
        table.AddRow("Grault", "Garply", "Fred");

        // When
        console.Write(table);

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("Render_Centered", "Align_Widget")]
    public Task Should_Center_Table_Correctly_When_Wrapped_In_Align_Widget()
    {
        // Given
        var console = new TestConsole();
        var table = new Table();
        table.AddColumns("Foo", "Bar", "Baz");
        table.AddRow("Qux", "Corgi", "Waldo");
        table.AddRow("Grault", "Garply", "Fred");

        // When
        console.Write(Align.Center(table));

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("Render_RightAligned")]
    public Task Should_Right_Align_Table_Correctly()
    {
        // Given
        var console = new TestConsole();
        var table = new Table();
#pragma warning disable CS0618 // Type or member is obsolete
        table.Alignment = Justify.Right;
#pragma warning restore CS0618 // Type or member is obsolete
        table.AddColumns("Foo", "Bar", "Baz");
        table.AddRow("Qux", "Corgi", "Waldo");
        table.AddRow("Grault", "Garply", "Fred");

        // When
        console.Write(table);

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("Render_RightAligned", "Align_Widget")]
    public Task Should_Right_Align_Table_Correctly_When_Wrapped_In_Align_Widget()
    {
        // Given
        var console = new TestConsole();
        var table = new Table();
        table.AddColumns("Foo", "Bar", "Baz");
        table.AddRow("Qux", "Corgi", "Waldo");
        table.AddRow("Grault", "Garply", "Fred");

        // When
        console.Write(Align.Right(table));

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("Render_Nested")]
    public Task Should_Render_Table_Nested_In_Panels_Correctly()
    {
        // A simple table
        var console = new TestConsole();
        var table = new Table() { Border = TableBorder.Rounded };
        table.AddColumn("Foo");
        table.AddColumn("Bar");
        table.AddColumn(new TableColumn("Baz") { Alignment = Justify.Right });
        table.AddRow("Qux\nQuuuuuux", "[blue]Corgi[/]", "Waldo");
        table.AddRow("Grault", "Garply", "Fred");

        // Render a table in some panels.
        console.Write(new Panel(new Panel(table)
        {
            Border = BoxBorder.Ascii,
        }));

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("Render_ColumnJustification")]
    public Task Should_Render_Table_With_Column_Justification_Correctly()
    {
        // Given
        var console = new TestConsole();
        var table = new Table();
        table.AddColumn(new TableColumn("Foo") { Alignment = Justify.Left });
        table.AddColumn(new TableColumn("Bar") { Alignment = Justify.Right });
        table.AddColumn(new TableColumn("Baz") { Alignment = Justify.Center });
        table.AddRow("Qux", "Corgi", "Waldo");
        table.AddRow("Grault", "Garply", "Lorem ipsum dolor sit amet");

        // When
        console.Write(table);

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("Render_Expand")]
    public Task Should_Expand_Table_To_Available_Space_If_Specified()
    {
        // Given
        var console = new TestConsole();
        var table = new Table() { Expand = true };
        table.AddColumns("Foo", "Bar", "Baz");
        table.AddRow("Qux", "Corgi", "Waldo");
        table.AddRow("Grault", "Garply", "Fred");

        // When
        console.Write(table);

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("Render_Multiline")]
    public Task Should_Render_Table_With_Multiple_Rows_In_Cell_Correctly()
    {
        // Given
        var console = new TestConsole();
        var table = new Table();
        table.AddColumns("Foo", "Bar", "Baz");
        table.AddRow("Qux\nQuuux", "Corgi", "Waldo");
        table.AddRow("Grault", "Garply", "Fred");

        // When
        console.Write(table);

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("Render_CellPadding")]
    public Task Should_Render_Table_With_Cell_Padding_Correctly()
    {
        // Given
        var console = new TestConsole();
        var table = new Table();
        table.AddColumns("Foo", "Bar");
        table.AddColumn(new TableColumn("Baz") { Padding = new Padding(3, 0, 2, 0) });
        table.AddRow("Qux\nQuuux", "Corgi", "Waldo");
        table.AddRow("Grault", "Garply", "Fred");

        // When
        console.Write(table);

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("Render_NoRows")]
    public Task Should_Render_Table_Without_Rows()
    {
        // Given
        var console = new TestConsole();
        var table = new Table();
        table.AddColumns("Foo", "Bar");
        table.AddColumn(new TableColumn("Baz") { Padding = new Padding(3, 0, 2, 0) });

        // When
        console.Write(table);

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("Render_Impossible")]
    public Task Should_Not_Draw_Tables_That_Are_Impossible_To_Draw()
    {
        // Given
        var console = new TestConsole().Width(25);

        var first = new Table().Border(TableBorder.Rounded).BorderColor(Color.Red);
        first.AddColumn(new TableColumn("[u]PS1[/]").Centered());
        first.AddColumn(new TableColumn("[u]PS2[/]"));
        first.AddColumn(new TableColumn("[u]PS3[/]"));
        first.AddRow("Hello", "[red]World[/]", string.Empty);
        first.AddRow("[blue]Bonjour[/]", "[white]le[/]", "[red]monde![/]");
        first.AddRow("[blue]Hej[/]", "[yellow]Världen[/]", string.Empty);

        var second = new Table().Border(TableBorder.Square).BorderColor(Color.Green);
        second.AddColumn(new TableColumn("[u]Foo[/]"));
        second.AddColumn(new TableColumn("[u]Bar[/]"));
        second.AddColumn(new TableColumn("[u]Baz[/]"));
        second.AddRow("Hello", "[red]World[/]", string.Empty);
        second.AddRow(first, new Text("Whaaat"), new Text("Lolz"));
        second.AddRow("[blue]Hej[/]", "[yellow]Världen[/]", string.Empty);

        var table = new Table().Border(TableBorder.Rounded);
        table.AddColumn(new TableColumn(new Panel("[u]ABC[/]").BorderColor(Color.Red)));
        table.AddColumn(new TableColumn(new Panel("[u]DEF[/]").BorderColor(Color.Green)));
        table.AddColumn(new TableColumn(new Panel("[u]GHI[/]").BorderColor(Color.Blue)));
        table.AddRow(new Text("Hello").Centered(), new Markup("[red]World[/]"), Text.Empty);
        table.AddRow(second, new Text("Whaat"), new Text("Lol").RightJustified());
        table.AddRow(new Markup("[blue]Hej[/]"), new Markup("[yellow]Världen[/]"), Text.Empty);

        // When
        console.Write(table);

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("Render_Title_Caption")]
    public Task Should_Render_Table_With_Title_And_Caption_Correctly()
    {
        // Given
        var console = new TestConsole();
        var table = new Table { Border = TableBorder.Rounded };
        table.Title = new TableTitle("Hello World");
        table.Caption = new TableTitle("Goodbye World");
        table.AddColumns("Foo", "Bar", "Baz");
        table.AddRow("Qux", "Corgi", "Waldo");
        table.AddRow("Grault", "Garply", "Fred");

        // When
        console.Write(table);

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("Render_Title_Caption_LeftAligned")]
    public Task Should_Left_Align_Table_With_Title_And_Caption_Correctly()
    {
        // Given
        var console = new TestConsole();
        var table = new Table { Border = TableBorder.Rounded };
        table.LeftAligned();
        table.Title = new TableTitle("Hello World");
        table.Caption = new TableTitle("Goodbye World");
        table.AddColumns("Foo", "Bar", "Baz");
        table.AddRow("Qux", "Corgi", "Waldo");
        table.AddRow("Grault", "Garply", "Fred");

        // When
        console.Write(table);

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("Render_Title_Caption_Centered")]
    public Task Should_Center_Table_With_Title_And_Caption_Correctly()
    {
        // Given
        var console = new TestConsole();
        var table = new Table { Border = TableBorder.Rounded };
        table.Centered();
        table.Title = new TableTitle("Hello World");
        table.Caption = new TableTitle("Goodbye World");
        table.AddColumns("Foo", "Bar", "Baz");
        table.AddRow("Qux", "Corgi", "Waldo");
        table.AddRow("Grault", "Garply", "Fred");

        // When
        console.Write(table);

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("Render_Title_Caption_RightAligned")]
    public Task Should_Right_Align_Table_With_Title_And_Caption_Correctly()
    {
        // Given
        var console = new TestConsole();
        var table = new Table { Border = TableBorder.Rounded };
        table.RightAligned();
        table.Title = new TableTitle("Hello World");
        table.Caption = new TableTitle("Goodbye World");
        table.AddColumns("Foo", "Bar", "Baz");
        table.AddRow("Qux", "Corgi", "Waldo");
        table.AddRow("Grault", "Garply", "Fred");

        // When
        console.Write(table);

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("Render_Title_Caption_LowerCase")]
    public Task Should_Render_Table_Without_Capitalizing_First_Letter()
    {
        // Given
        var console = new TestConsole();
        var table = new Table { Border = TableBorder.Rounded };
        table.Title = new TableTitle("hello world");
        table.Caption = new TableTitle("goodbye world");
        table.AddColumns("Foo", "Bar", "Baz");
        table.AddRow("Qux", "Corgi", "Waldo");
        table.AddRow("Grault", "Garply", "Fred");

        // When
        console.Write(table);

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("Render_Fold")]
    public Task Should_Render_With_Folded_Text_Table_Correctly()
    {
        // Given
        var console = new TestConsole().Width(30);
        var table = new Table();
        table.AddColumns("Foo", "Bar", "Baz");
        table.AddRow("Qux With A Long Description", "Corgi", "Waldo");
        table.AddRow("Grault", "Garply", "Fred On A Long Long Walk");

        var panel = new Panel(table);
        panel.Border = BoxBorder.Double;

        // When
        console.Write(panel);

        // Then
        return Verifier.Verify(console.Output);
    }
}
