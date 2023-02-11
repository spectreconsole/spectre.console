namespace Spectre.Console.Tests.Unit;

[UsesVerify]
[ExpectationPath("Prompts/Text")]
public sealed class TextPromptTests
{
    [Fact]
    public void Should_Return_Entered_Text()
    {
        // Given
        var console = new TestConsole();
        console.Input.PushTextWithEnter("Hello World");

        // When
        var result = console.Prompt(new TextPrompt<string>("Enter text:"));

        // Then
        result.ShouldBe("Hello World");
    }

    [Fact]
    [Expectation("ConversionError")]
    public Task Should_Return_Validation_Error_If_Value_Cannot_Be_Converted()
    {
        // Given
        var console = new TestConsole();
        console.Input.PushTextWithEnter("ninety-nine");
        console.Input.PushTextWithEnter("99");

        // When
        console.Prompt(new TextPrompt<int>("Age?"));

        // Then
        return Verifier.Verify(console.Lines);
    }

    [Fact]
    [Expectation("DefaultValue")]
    public Task Should_Chose_Default_Value_If_Nothing_Is_Entered()
    {
        // Given
        var console = new TestConsole();
        console.Input.PushKey(ConsoleKey.Enter);

        // When
        console.Prompt(
            new TextPrompt<string>("Favorite fruit?")
                .AddChoice("Banana")
                .AddChoice("Orange")
                .DefaultValue("Banana"));

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("InvalidChoice")]
    public Task Should_Return_Error_If_An_Invalid_Choice_Is_Made()
    {
        // Given
        var console = new TestConsole();
        console.Input.PushTextWithEnter("Apple");
        console.Input.PushTextWithEnter("Banana");

        // When
        console.Prompt(
            new TextPrompt<string>("Favorite fruit?")
                .AddChoice("Banana")
                .AddChoice("Orange")
                .DefaultValue("Banana"));

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("AcceptChoice")]
    public Task Should_Accept_Choice_In_List()
    {
        // Given
        var console = new TestConsole();
        console.Input.PushTextWithEnter("Orange");

        // When
        console.Prompt(
            new TextPrompt<string>("Favorite fruit?")
                .AddChoice("Banana")
                .AddChoice("Orange")
                .DefaultValue("Banana"));

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("AutoComplete_Empty")]
    public Task Should_Auto_Complete_To_First_Choice_If_Pressing_Tab_On_Empty_String()
    {
        // Given
        var console = new TestConsole();
        console.Input.PushKey(ConsoleKey.Tab);
        console.Input.PushKey(ConsoleKey.Enter);

        // When
        console.Prompt(
            new TextPrompt<string>("Favorite fruit?")
                .AddChoice("Banana")
                .AddChoice("Orange")
                .DefaultValue("Banana"));

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("AutoComplete_BestMatch")]
    public Task Should_Auto_Complete_To_Best_Match()
    {
        // Given
        var console = new TestConsole();
        console.Input.PushText("Band");
        console.Input.PushKey(ConsoleKey.Tab);
        console.Input.PushKey(ConsoleKey.Enter);

        // When
        console.Prompt(
            new TextPrompt<string>("Favorite fruit?")
                .AddChoice("Banana")
                .AddChoice("Bandana")
                .AddChoice("Orange"));

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("AutoComplete_NextChoice")]
    public Task Should_Auto_Complete_To_Next_Choice_When_Pressing_Tab_On_A_Match()
    {
        // Given
        var console = new TestConsole();
        console.Input.PushText("Apple");
        console.Input.PushKey(ConsoleKey.Tab);
        console.Input.PushKey(ConsoleKey.Enter);

        // When
        console.Prompt(
            new TextPrompt<string>("Favorite fruit?")
                .AddChoice("Apple")
                .AddChoice("Banana")
                .AddChoice("Orange"));

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("AutoComplete_PreviousChoice")]
    public Task Should_Auto_Complete_To_Previous_Choice_When_Pressing_ShiftTab_On_A_Match()
    {
        // Given
        var console = new TestConsole();
        console.Input.PushText("Ban");
        console.Input.PushKey(ConsoleKey.Tab);
        console.Input.PushKey(ConsoleKey.Tab);
        var shiftTab = new ConsoleKeyInfo((char)ConsoleKey.Tab, ConsoleKey.Tab, true, false, false);
        console.Input.PushKey(shiftTab);
        console.Input.PushKey(ConsoleKey.Enter);

        // When
        console.Prompt(
            new TextPrompt<string>("Favorite fruit?")
                .AddChoice("Banana")
                .AddChoice("Bandana")
                .AddChoice("Orange"));

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("CustomValidation")]
    public Task Should_Return_Error_If_Custom_Validation_Fails()
    {
        // Given
        var console = new TestConsole();
        console.Input.PushTextWithEnter("22");
        console.Input.PushTextWithEnter("102");
        console.Input.PushTextWithEnter("ABC");
        console.Input.PushTextWithEnter("99");

        // When
        console.Prompt(
            new TextPrompt<int>("Guess number:")
                .ValidationErrorMessage("Invalid input")
                .Validate(age =>
                {
                    if (age < 99)
                    {
                        return ValidationResult.Error("Too low");
                    }
                    else if (age > 99)
                    {
                        return ValidationResult.Error("Too high");
                    }

                    return ValidationResult.Success();
                }));

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("CustomConverter")]
    public Task Should_Use_Custom_Converter()
    {
        // Given
        var console = new TestConsole();
        console.Input.PushTextWithEnter("Banana");

        // When
        var result = console.Prompt(
            new TextPrompt<(int, string)>("Favorite fruit?")
                .AddChoice((1, "Apple"))
                .AddChoice((2, "Banana"))
                .WithConverter(testData => testData.Item2));

        // Then
        result.Item1.ShouldBe(2);
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("SecretDefaultValue")]
    public Task Should_Choose_Masked_Default_Value_If_Nothing_Is_Entered_And_Prompt_Is_Secret()
    {
        // Given
        var console = new TestConsole();
        console.Input.PushKey(ConsoleKey.Enter);

        // When
        console.Prompt(
            new TextPrompt<string>("Favorite fruit?")
                .Secret()
                .DefaultValue("Banana"));

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("SecretDefaultValueCustomMask")]
    public Task Should_Choose_Custom_Masked_Default_Value_If_Nothing_Is_Entered_And_Prompt_Is_Secret_And_Mask_Is_Custom()
    {
        // Given
        var console = new TestConsole();
        console.Input.PushKey(ConsoleKey.Enter);

        // When
        console.Prompt(
            new TextPrompt<string>("Favorite fruit?")
                .Secret('-')
                .DefaultValue("Banana"));

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("SecretDefaultValueNullMask")]
    public Task Should_Choose_Empty_Masked_Default_Value_If_Nothing_Is_Entered_And_Prompt_Is_Secret_And_Mask_Is_Null()
    {
        // Given
        var console = new TestConsole();
        console.Input.PushKey(ConsoleKey.Enter);

        // When
        console.Prompt(
            new TextPrompt<string>("Favorite fruit?")
                .Secret(null)
                .DefaultValue("Banana"));

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("NoSuffix")]
    public Task Should_Not_Append_Questionmark_Or_Colon_If_No_Choices_Are_Set()
    {
        // Given
        var console = new TestConsole();
        console.Input.PushTextWithEnter("Orange");

        // When
        console.Prompt(
            new TextPrompt<string>("Enter command$"));

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("DefaultValueStyleNotSet")]
    public Task Uses_default_style_for_default_value_if_no_style_is_set()
    {
        // Given
        var console = new TestConsole
        {
            EmitAnsiSequences = true,
        };
        console.Input.PushTextWithEnter("Input");

        var prompt = new TextPrompt<string>("Enter Value:")
                .ShowDefaultValue()
                .DefaultValue("default");

        // When
        console.Prompt(prompt);

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("DefaultValueStyleSet")]
    public Task Uses_specified_default_value_style()
    {
        // Given
        var console = new TestConsole
        {
            EmitAnsiSequences = true,
        };
        console.Input.PushTextWithEnter("Input");

        var prompt = new TextPrompt<string>("Enter Value:")
                .ShowDefaultValue()
                .DefaultValue("default")
                .DefaultValueStyle(Color.Red);

        // When
        console.Prompt(prompt);

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("ChoicesStyleNotSet")]
    public Task Uses_default_style_for_choices_if_no_style_is_set()
    {
        // Given
        var console = new TestConsole
        {
            EmitAnsiSequences = true,
        };
        console.Input.PushTextWithEnter("Choice 2");

        var prompt = new TextPrompt<string>("Enter Value:")
                .ShowChoices()
                .AddChoice("Choice 1")
                .AddChoice("Choice 2");

        // When
        console.Prompt(prompt);

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("ChoicesStyleSet")]
    public Task Uses_the_specified_choices_style()
    {
        // Given
        var console = new TestConsole
        {
            EmitAnsiSequences = true,
        };
        console.Input.PushTextWithEnter("Choice 2");

        var prompt = new TextPrompt<string>("Enter Value:")
                .ShowChoices()
                .AddChoice("Choice 1")
                .AddChoice("Choice 2")
                .ChoicesStyle(Color.Red);

        // When
        console.Prompt(prompt);

        // Then
        return Verifier.Verify(console.Output);
    }
}
