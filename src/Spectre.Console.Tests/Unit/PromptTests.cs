using System;
using System.Threading.Tasks;
using Spectre.Console.Testing;
using Shouldly;
using VerifyXunit;
using Xunit;
using Spectre.Verify.Extensions;

namespace Spectre.Console.Tests.Unit
{
    [UsesVerify]
    [ExpectationPath("Widgets/Prompt")]
    public sealed class PromptTests
    {
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
        public Task Should_Chose_Masked_Default_Value_If_Nothing_Is_Entered_And_Prompt_Is_Secret()
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
    }
}
