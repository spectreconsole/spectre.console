namespace Spectre.Console.Tests.Unit;

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
    [Expectation("SecretValueBackspaceNullMask")]
    public Task Should_Not_Erase_Prompt_Text_On_Backspace_If_Prompt_Is_Secret_And_Mask_Is_Null()
    {
        // Given
        var console = new TestConsole();
        console.Input.PushText("Bananas");
        console.Input.PushKey(ConsoleKey.Backspace);
        console.Input.PushKey(ConsoleKey.Enter);

        // When
        console.Prompt(
            new TextPrompt<string>("Favorite fruit?")
                .Secret(null));

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
    [Expectation("Issue_1638")]
    public Task Should_Append_Colon_When_No_Default_Value_Is_Set()
    {
        // Given
        var console = new TestConsole();
        console.Input.PushTextWithEnter("input");

        // When
        console.Prompt(
            new TextPrompt<string>("no default, with suffix"));

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

    [Fact]
    [Expectation("InvalidDefaultChoice")]
    public Task Should_Return_Error_If_Default_Choice_Invalid()
    {
        // Given
        var console = new TestConsole();
        console.Input.PushTextWithEnter("");
        console.Input.PushTextWithEnter("a");

        // When
        console.Prompt(
            new TextPrompt<string>("Favorite fruit?")
                .AddChoice("Banana")
                .AddChoice("Orange")
                .DefaultValue("Banan")
                .ShowDefaultValue(true)
                .EditableDefaultValue(true));

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("ClearOnFinish")]
    public Task Should_Clear_Prompt_Line_When_ClearOnFinish_Is_Enabled()
    {
        // Given
        var console = new TestConsole
        {
            EmitAnsiSequences = true,
        };
        console.Input.PushTextWithEnter("secret-value");

        // When
        console.Prompt(
            new TextPrompt<string>("Enter a value")
                .Secret()
                .ClearOnFinish());

        // Then
        return Verifier.Verify(console.Output);
    }

    [Theory]
    [InlineData("yes")]
    [InlineData("Yes")]
    [InlineData("YES")]
    public async Task Uses_case_insensitive_comparison_when_no_comparer_is_passed(string input)
    {
        // Given
        var console = new TestConsole { EmitAnsiSequences = true, };
        console.Input.PushTextWithEnter(input);

        var prompt = new TextPrompt<string>("Was the tool helpful?")
            .AddChoices(["Yes", "Partially", "No"]);

        // When
        var result = await console.PromptAsync(prompt);

        // Then
        result.ShouldBe("Yes");
    }

    [Fact]
    public void Validate_BoolOverload_ShortCircuits()
    {
        // Given
        var prompt = new TextPrompt<string>("Enter:");
        var secondInvoked = false;

        prompt
            .Validate(s => s.Length >= 3, "too short")
            .Validate(s =>
            {
                secondInvoked = true;
                return s.Contains("a");
            }, "missing a");

        // When
        var result = prompt.Validator?.Invoke("ab");

        // Then
        result.ShouldBeEquivalentTo(ValidationResult.Error("too short"));
        secondInvoked.ShouldBeFalse();
    }

    [Fact]
    public void Validate_BoolOverload_Returns_Chained_Validation_Error()
    {
        // Given
        var prompt = new TextPrompt<string>("Enter:");
        var secondInvoked = false;

        prompt
            .Validate(s => s.Length >= 3, "too short")
            .Validate(s =>
            {
                secondInvoked = true;
                return s.Contains("a");
            }, "missing a");

        // When
        var result = prompt.Validator?.Invoke("bbc");

        // Then
        result.ShouldBeEquivalentTo(ValidationResult.Error("missing a"));
        secondInvoked.ShouldBeTrue();
    }

    [Fact]
    public void Validate_BoolOverload_Returns_Success_When_All_Validators_Pass()
    {
        // Given
        var prompt = new TextPrompt<string>("Enter:");

        prompt
            .Validate(s => s.Length >= 3, "too short")
            .Validate(s => s.Contains("a"), "missing a");

        // When
        var result = prompt.Validator?.Invoke("abc");

        // Then
        result.ShouldBeEquivalentTo(ValidationResult.Success());
    }

    [Fact]
    public void Validate_FuncOverload_ShortCircuits()
    {
        // Given
        var prompt = new TextPrompt<string>("Enter:");
        var secondInvoked = false;

        prompt
            .Validate(s => s.Length < 3 ? ValidationResult.Error("too short") : ValidationResult.Success())
            .Validate(s =>
            {
                secondInvoked = true;
                return s.Contains("a") ? ValidationResult.Success() : ValidationResult.Error("missing a");
            } );

        // When
        var result = prompt.Validator?.Invoke("ab");

        // Then
        result.ShouldBeEquivalentTo(ValidationResult.Error("too short"));
        secondInvoked.ShouldBeFalse();
    }

    [Fact]
    public void Validate_FuncOverload_Returns_Chained_Validation_Error()
    {
        // Given
        var prompt = new TextPrompt<string>("Enter:");
        var secondInvoked = false;

        prompt
            .Validate(s => s.Length < 3 ? ValidationResult.Error("too short") : ValidationResult.Success())
            .Validate(s =>
            {
                secondInvoked = true;
                return s.Contains("a") ? ValidationResult.Success() : ValidationResult.Error("missing a");
            } );

        // When
        var result = prompt.Validator?.Invoke("bbc");

        // Then
        result.ShouldBeEquivalentTo(ValidationResult.Error("missing a"));
        secondInvoked.ShouldBeTrue();
    }

    [Fact]
    public void Validate_FuncOverload_Returns_Success_When_All_Validators_Pass()
    {
        // Given
        var prompt = new TextPrompt<string>("Enter:");

        prompt
            .Validate(s => s.Length < 3 ? ValidationResult.Error("too short") : ValidationResult.Success())
            .Validate(s => s.Contains("a") ? ValidationResult.Success() : ValidationResult.Error("missing a") );

        // When
        var result = prompt.Validator?.Invoke("abc");

        // Then
        result.ShouldBeEquivalentTo(ValidationResult.Success());
    }

    [Fact]
    public void Validate_MixedOverloads_ShortCircuits()
    {
        // Given
        var prompt = new TextPrompt<string>("Enter:");
        var secondInvoked = false;

        prompt
            .Validate(s => s.Length >= 3, "too short")            .Validate(s =>
            {
                secondInvoked = true;
                return s.Contains("a") ? ValidationResult.Success() : ValidationResult.Error("missing a");
            } );

        // When
        var result = prompt.Validator?.Invoke("ab");

        // Then
        result.ShouldBeEquivalentTo(ValidationResult.Error("too short"));
        secondInvoked.ShouldBeFalse();
    }

    [Fact]
    public void Validate_MixedOverloads_Returns_Chained_Validation_Error()
    {
        // Given
        var prompt = new TextPrompt<string>("Enter:");
        var secondInvoked = false;

        prompt
            .Validate(s => s.Length >= 3, "too short")
            .Validate(s =>
            {
                secondInvoked = true;
                return s.Contains("a") ? ValidationResult.Success() : ValidationResult.Error("missing a");
            } );

        // When
        var result = prompt.Validator?.Invoke("bbc");

        // Then
        result.ShouldBeEquivalentTo(ValidationResult.Error("missing a"));
        secondInvoked.ShouldBeTrue();
    }

    [Fact]
    public void Validate_MixedOverloads_Returns_Success_When_All_Validators_Pass()
    {
        // Given
        var prompt = new TextPrompt<string>("Enter:");

        prompt
            .Validate(s => s.Length >= 3, "too short")
            .Validate(s => s.Contains("a") ? ValidationResult.Success() : ValidationResult.Error("missing a") );

        // When
        var result = prompt.Validator?.Invoke("abc");

        // Then
        result.ShouldBeEquivalentTo(ValidationResult.Success());
    }

    [Fact]
    public void Validate_MixedOverloads_WithThreeValidators_Returns_ThirdValidationError()
    {
        // Given
        var prompt = new TextPrompt<string>("Enter:");
        var secondInvoked = false;
        var thirdInvoked = false;

        prompt
            .Validate(s => s.Length >= 3, "too short")
            .Validate(s =>
            {
                secondInvoked = true;
                return s.Contains("a")
                    ? ValidationResult.Success()
                    : ValidationResult.Error("missing a");
            })
            .Validate(s =>
            {
                thirdInvoked = true;
                return s.EndsWith("z");
            }, "must end with z");

        // When
        var result = prompt.Validator?.Invoke("abc");

        // Then
        result.ShouldBeEquivalentTo(ValidationResult.Error("must end with z"));
        secondInvoked.ShouldBeTrue();
        thirdInvoked.ShouldBeTrue();
    }

    [Fact]
    public void Validate_MixedOverloads_WithThreeValidators_Returns_Success_When_All_Validators_Pass()
    {
        // Given
        var prompt = new TextPrompt<string>("Enter:");
        var secondInvoked = false;
        var thirdInvoked = false;

        prompt
            .Validate(s => s.Length >= 3, "too short")
            .Validate(s =>
            {
                secondInvoked = true;
                return s.Contains("a")
                    ? ValidationResult.Success()
                    : ValidationResult.Error("missing a");
            })
            .Validate(s =>
            {
                thirdInvoked = true;
                return s.EndsWith("z");
            }, "must end with z");

        // When
        var result = prompt.Validator?.Invoke("abz");

        // Then
        result.ShouldBeEquivalentTo(ValidationResult.Success());
        secondInvoked.ShouldBeTrue();
        thirdInvoked.ShouldBeTrue();
    }

    [Fact]
    public void Should_Accept_Non_Ascii_Default_Value_When_Editable()
    {
        // Given
        var console = new TestConsole();
        console.Input.PushKey(ConsoleKey.Enter);

        // When
        var result = console.Prompt(
            new TextPrompt<string>("Enter text:")
                .DefaultValue("ㅎ")
                .EditableDefaultValue(true));

        // Then
        result.ShouldBe("ㅎ");
    }

    [Fact]
    public void Should_Backspace_Remove_Surrogate_Pair_As_Single_Rune_Leaving_Preceding_Char()
    {
        // Given: "a😀" — 'a' then emoji (surrogate pair 😀)
        var console = new TestConsole();
        console.Input.PushKey(new ConsoleKeyInfo('a', ConsoleKey.A, false, false, false));
        console.Input.PushKey(new ConsoleKeyInfo('\uD83D', ConsoleKey.Packet, false, false, false));
        console.Input.PushKey(new ConsoleKeyInfo('\uDE00', ConsoleKey.Packet, false, false, false));
        console.Input.PushKey(ConsoleKey.Backspace);
        console.Input.PushKey(ConsoleKey.Enter);

        // When
        var result = console.Prompt(new TextPrompt<string>("Enter:"));

        // Then: both surrogate chars removed as one rune; 'a' remains
        result.ShouldBe("a");
    }

    [Fact]
    public void Should_Backspace_Remove_Lone_Emoji_Leaving_Empty_String()
    {
        // Given: only an emoji (surrogate pair); AllowEmpty so prompt accepts ""
        var console = new TestConsole();
        console.Input.PushKey(new ConsoleKeyInfo('\uD83D', ConsoleKey.Packet, false, false, false));
        console.Input.PushKey(new ConsoleKeyInfo('\uDE00', ConsoleKey.Packet, false, false, false));
        console.Input.PushKey(ConsoleKey.Backspace);
        console.Input.PushKey(ConsoleKey.Enter);

        // When
        var result = console.Prompt(new TextPrompt<string>("Enter:").AllowEmpty());

        // Then: both surrogate chars removed, nothing left
        result.ShouldBe(string.Empty);
    }

    [Fact]
    [Expectation("SecretValueBackspaceWideChar")]
    public Task Should_Erase_One_Mask_Cell_Per_Backspace_For_Wide_Char_In_Secret_Mode()
    {
        // Given: type '한' (double-width CJK), backspace, enter — in secret mode
        // Secret mode uses one mask char per input char regardless of source cell width,
        // so backspace must erase exactly one mask cell (not two).
        var console = new TestConsole();
        console.Input.PushKey(new ConsoleKeyInfo('한', ConsoleKey.Packet, false, false, false));
        console.Input.PushKey(ConsoleKey.Backspace);
        console.Input.PushKey(ConsoleKey.Enter);

        // When
        var result = console.Prompt(new TextPrompt<string>("Enter:").Secret().AllowEmpty());

        // Then
        result.ShouldBe(string.Empty);
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("AutocompleteWideCharErase")]
    public Task Should_Erase_By_Cell_Width_When_Autocomplete_Replaces_Wide_Char_Text()
    {
        // Given: type '한' (1 char, 2 cells wide), Tab to autocomplete to '한국어'
        // The erase before writing the replacement must repeat Cell.GetCellLength("한") = 2
        // times, not text.Length = 1 time, otherwise one ghost cell remains on screen.
        var console = new TestConsole();
        console.Input.PushKey(new ConsoleKeyInfo('한', ConsoleKey.Packet, false, false, false));
        console.Input.PushKey(ConsoleKey.Tab);
        console.Input.PushKey(ConsoleKey.Enter);

        // When
        console.Prompt(
            new TextPrompt<string>("Choose:")
                .AddChoice("한국어")
                .AddChoice("English"));

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    public void Should_Backspace_Through_Mixed_Width_Sequence_Peeling_One_Rune_At_A_Time()
    {
        // Given: "a한😀" — ASCII (1 cell) + CJK (1 char, 2 cells) + emoji (surrogate pair)
        // Two backspaces peel the emoji (2 chars) and the CJK char, leaving only 'a'.
        // Validates rune-by-rune removal without index or surrogate corruption.
        var console = new TestConsole();
        console.Input.PushKey(new ConsoleKeyInfo('a', ConsoleKey.A, false, false, false));
        console.Input.PushKey(new ConsoleKeyInfo('한', ConsoleKey.Packet, false, false, false));
        // 😀 = U+1F600 = surrogate pair 😀
        console.Input.PushKey(new ConsoleKeyInfo('\uD83D', ConsoleKey.Packet, false, false, false));
        console.Input.PushKey(new ConsoleKeyInfo('\uDE00', ConsoleKey.Packet, false, false, false));
        console.Input.PushKey(ConsoleKey.Backspace); // removes emoji surrogate pair as one rune
        console.Input.PushKey(ConsoleKey.Backspace); // removes '한'
        console.Input.PushKey(ConsoleKey.Enter);

        // When
        var result = console.Prompt(new TextPrompt<string>("Enter:"));

        // Then: 'a' remains; emoji and CJK each removed as single runes
        result.ShouldBe("a");
    }
}