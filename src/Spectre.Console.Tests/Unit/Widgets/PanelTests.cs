namespace Spectre.Console.Tests.Unit;

[ExpectationPath("Widgets/Panel")]
public sealed class PanelTests
{
    [Fact]
    [Expectation("Render")]
    public Task Should_Render_Panel()
    {
        // Given
        var console = new TestConsole();

        // When
        console.Write(new Panel(new Text("Hello World")));

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("Render_ZeroPadding")]
    public Task Should_Render_Panel_With_Padding_Set_To_Zero()
    {
        // Given
        var console = new TestConsole();

        // When
        console.Write(new Panel(new Text("Hello World"))
        {
            Padding = new Padding(0, 0, 0, 0),
        });

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("Render_Padding")]
    public Task Should_Render_Panel_With_Padding()
    {
        // Given
        var console = new TestConsole();

        // When
        console.Write(new Panel(new Text("Hello World"))
        {
            Padding = new Padding(3, 1, 5, 2),
        });

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("Render_Header")]
    public Task Should_Render_Panel_With_Header()
    {
        // Given
        var console = new TestConsole();

        // When
        console.Write(new Panel("Hello World")
        {
            Header = new PanelHeader("Greeting"),
            Expand = true,
            Padding = new Padding(2, 0, 2, 0),
        });

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("Render_Header_LeftAligned")]
    public Task Should_Render_Panel_With_Left_Aligned_Header()
    {
        // Given
        var console = new TestConsole();

        // When
        console.Write(new Panel("Hello World")
        {
            Header = new PanelHeader("Greeting").LeftJustified(),
            Expand = true,
        });

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("Render_Header_Centered")]
    public Task Should_Render_Panel_With_Centered_Header()
    {
        // Given
        var console = new TestConsole();

        // When
        console.Write(new Panel("Hello World")
        {
            Header = new PanelHeader("Greeting").Centered(),
            Expand = true,
        });

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("Render_Header_RightAligned")]
    public Task Should_Render_Panel_With_Right_Aligned_Header()
    {
        // Given
        var console = new TestConsole();

        // When
        console.Write(new Panel("Hello World")
        {
            Header = new PanelHeader("Greeting").RightJustified(),
            Expand = true,
        });

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("Render_Header_Collapse")]
    public Task Should_Collapse_Header_If_It_Will_Not_Fit()
    {
        // Given
        var console = new TestConsole().Width(10);

        // When
        console.Write(new Panel("Hello World")
        {
            Header = new PanelHeader("Greeting"),
            Expand = true,
        });

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("Render_Header_Wider_Than_Content")]
    public Task Should_Render_Header_Wider_Than_Content()
    {
        var console = new TestConsole().Width(40);

        console.Write(new Panel("x")
        {
            Header = new PanelHeader("HDR"),
        });

        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("Render_Header_Collapse_In_Constrained_Parent")]
    public Task Should_Collapse_Header_In_Constrained_Parent()
    {
        var console = new TestConsole().Width(40);
        var grid = new Grid();
        grid.AddColumn(new GridColumn { Width = 10 });
        grid.AddRow(new Panel("Hello World")
        {
            Header = new PanelHeader("Configuration Settings"),
        });

        console.Write(grid);

        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("Render_Header_Collapse_In_Table_Column")]
    public Task Should_Collapse_Header_In_Table_Column()
    {
        var console = new TestConsole().Width(20);
        var table = new Table();
        table.AddColumn("A");
        table.AddRow(new Panel("Hello World")
        {
            Header = new PanelHeader("Configuration Settings"),
        });

        console.Write(table);

        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("Render_Header_Explicit_Width")]
    public Task Should_Not_Grow_Explicit_Width_For_Header()
    {
        var console = new TestConsole().Width(40);

        console.Write(new Panel("x")
        {
            Header = new PanelHeader("HDR"),
            Width = 5,
        });

        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("Render_Header_Multiline")]
    public Task Should_Measure_Header_On_One_Line()
    {
        var console = new TestConsole().Width(40);

        console.Write(new Panel("x")
        {
            Header = new PanelHeader(" Line one\nLine two "),
        });

        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("Render_Empty_Header")]
    public Task Should_Not_Grow_For_Empty_Header()
    {
        var console = new TestConsole().Width(40);

        console.Write(new Panel("x")
        {
            Padding = new Padding(0, 0, 0, 0),
        }.HeaderAlignment(Justify.Center));

        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("Render_Unicode")]
    public Task Should_Render_Panel_With_Unicode_Correctly()
    {
        // Given
        var console = new TestConsole();

        // When
        console.Write(new Panel(new Text(" \n💩\n ")));

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("Render_Multiline")]
    public Task Should_Render_Panel_With_Multiple_Lines()
    {
        // Given
        var console = new TestConsole();

        // When
        console.Write(new Panel(new Text("Hello World\nFoo Bar")));

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("Render_LineEndings")]
    public Task Should_Preserve_Explicit_Line_Ending()
    {
        // Given
        var console = new TestConsole();
        var text = new Panel(
            new Markup("I heard [underline on blue]you[/] like 📦\n\n\n\nSo I put a 📦 in a 📦"));

        // When
        console.Write(text);

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("Render_Expand")]
    public Task Should_Expand_Panel_If_Enabled()
    {
        // Given
        var console = new TestConsole();

        // When
        console.Write(new Panel(new Text("Hello World"))
        {
            Expand = true,
        });

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("Render_Width")]
    public Task Should_Render_To_Specified_Width()
    {
        // Given
        var console = new TestConsole();

        // When
        console.Write(new Panel(new Text("Hello World"))
        {
            Width = 25,
        });

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("Render_Width_MaxWidth")]
    public Task Should_Use_Max_Width_If_Specified_Width_Is_Too_Large()
    {
        // Given
        var console = new TestConsole();
        console.Profile.Width = 20;

        // When
        console.Write(new Panel(new Text("Hello World"))
        {
            Width = 25,
        });

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("Render_Height")]
    public Task Should_Render_To_Specified_Height()
    {
        // Given
        var console = new TestConsole();

        // When
        console.Write(new Panel(new Text("Hello World\nHello Hello Hello"))
        {
            Height = 25,
        });

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("Render_Width_Height")]
    public Task Should_Render_To_Specified_Width_And_Height()
    {
        // Given
        var console = new TestConsole();

        // When
        console.Write(new Panel("Hello World\nHello Hello Hello")
        {
            Width = 50,
            Height = 25,
        });

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("Render_Child_RightAligned")]
    public Task Should_Justify_Child_To_Right_Correctly()
    {
        // Given
        var console = new TestConsole().Width(25);

        // When
        console.Write(
            new Panel(new Text("Hello World").RightJustified())
            {
                Expand = true,
            });

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("Render_Child_Centered")]
    public Task Should_Center_Child_Correctly()
    {
        // Given
        var console = new TestConsole().Width(25);

        // When
        console.Write(
            new Panel(new Text("Hello World").Centered())
            {
                Expand = true,
            });

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("Render_Child_Panel")]
    public Task Should_Render_Panel_Inside_Panel_Correctly()
    {
        // Given
        var console = new TestConsole();

        // When
        console.Write(new Panel(new Panel(new Text("Hello World"))));

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("Render_Wrap")]
    public Task Should_Wrap_Content_Correctly()
    {
        // Given
        var console = new TestConsole().Width(84);
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
        console.Write(panel);

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("Render_CJK")]
    public Task Should_Wrap_Table_With_CJK_Tables_In_Panel_Correctly()
    {
        // Given
        var console = new TestConsole();

        var table = new Table();
        table.AddColumn("测试");
        table.AddRow("测试");
        var panel = new Panel(table);

        // When
        console.Write(panel);

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("Render_Text_Overflow_Crop")]
    public Task Should_Respect_Text_Overflow_Crop_In_Panel()
    {
        // Given
        var console = new TestConsole();
        var text = new Text("foo pneumonoultramicroscopicsilicovolcanoconiosis bar qux")
            .Overflow(Overflow.Crop);
        var panel = new Panel(text)
        {
            Width = 20,
        };

        // When
        console.Write(panel);

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("Render_Text_Overflow_Ellipsis")]
    public Task Should_Respect_Text_Overflow_Ellipsis_In_Panel()
    {
        // Given
        var console = new TestConsole();
        var text = new Text("foo pneumonoultramicroscopicsilicovolcanoconiosis bar qux")
            .Overflow(Overflow.Ellipsis);
        var panel = new Panel(text)
        {
            Width = 20,
        };

        // When
        console.Write(panel);

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("Render_Text_Overflow_Fold")]
    public Task Should_Respect_Text_Overflow_Fold_In_Panel()
    {
        // Given
        var console = new TestConsole();
        var text = new Text("foo pneumonoultramicroscopicsilicovolcanoconiosis bar qux")
            .Overflow(Overflow.Fold);
        var panel = new Panel(text)
        {
            Width = 20,
        };

        // When
        console.Write(panel);

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("Render_Markup_Overflow_Ellipsis")]
    public Task Should_Respect_Markup_Overflow_Ellipsis_In_Panel()
    {
        // Given
        var console = new TestConsole();
        var markup = new Markup("[yellow]foo[/] [red]pneumonoultramicroscopicsilicovolcanoconiosis[/] [blue]bar qux[/]")
            .Overflow(Overflow.Ellipsis);
        var panel = new Panel(markup)
        {
            Width = 20,
        };

        // When
        console.Write(panel);

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("Render_Text_Overflow_With_Padding")]
    public Task Should_Respect_Text_Overflow_In_Panel_With_Padding()
    {
        // Given
        var console = new TestConsole();
        var text = new Text("foo pneumonoultramicroscopicsilicovolcanoconiosis bar qux")
            .Overflow(Overflow.Ellipsis);
        var panel = new Panel(text)
        {
            Width = 25,
            Padding = new Padding(2, 0, 2, 0),
        };

        // When
        console.Write(panel);

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("Render_Nested_Panel_With_Overflow")]
    public Task Should_Respect_Text_Overflow_In_Nested_Panel()
    {
        // Given
        var console = new TestConsole();
        var text = new Text("foo pneumonoultramicroscopicsilicovolcanoconiosis bar qux")
            .Overflow(Overflow.Crop);
        var innerPanel = new Panel(text)
        {
            Width = 15,
        };
        var outerPanel = new Panel(innerPanel)
        {
            Width = 25,
        };

        // When
        console.Write(outerPanel);

        // Then
        return Verifier.Verify(console.Output);
    }
}