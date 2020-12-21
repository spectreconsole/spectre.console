using System;
using System.Threading.Tasks;
using VerifyXunit;
using Xunit;

namespace Spectre.Console.Tests.Unit
{
    [UsesVerify]
    public sealed class PromptTests
    {
        [Fact]
        public Task Should_Return_Validation_Error_If_Value_Cannot_Be_Converted()
        {
            // Given
            var console = new PlainConsole();
            console.Input.PushTextWithEnter("ninety-nine");
            console.Input.PushTextWithEnter("99");

            // When
            console.Prompt(new TextPrompt<int>("Age?"));

            // Then
            return Verifier.Verify(console.Lines);
        }

        [Fact]
        public Task Should_Chose_Default_Value_If_Nothing_Is_Entered()
        {
            // Given
            var console = new PlainConsole();
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
        public Task Should_Return_Error_If_An_Invalid_Choice_Is_Made()
        {
            // Given
            var console = new PlainConsole();
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
        public Task Should_Accept_Choice_In_List()
        {
            // Given
            var console = new PlainConsole();
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
        public Task Should_Auto_Complete_To_First_Choice_If_Pressing_Tab_On_Empty_String()
        {
            // Given
            var console = new PlainConsole();
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
        public Task Should_Auto_Complete_To_Best_Match()
        {
            // Given
            var console = new PlainConsole();
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
        public Task Should_Auto_Complete_To_Next_Choice_When_Pressing_Tab_On_A_Match()
        {
            // Given
            var console = new PlainConsole();
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
        public Task Should_Return_Error_If_Custom_Validation_Fails()
        {
            // Given
            var console = new PlainConsole();
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
    }
}
