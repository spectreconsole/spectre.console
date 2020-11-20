using System;
using System.Threading.Tasks;
using Shouldly;
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
            console.Input.PushText("ninety-nine");
            console.Input.PushText("99");

            // When
            console.Prompt(new TextPrompt<int>("Age?"));

            // Then
            return Verifier.Verify(console.Lines);
        }

        [Fact]
        public void Should_Chose_Default_Value_If_Nothing_Is_Entered()
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
            console.Lines.Count.ShouldBe(1);
            console.Lines[0].ShouldBe("Favorite fruit? [Banana/Orange] (Banana): Banana");
        }

        [Fact]
        public Task Should_Return_Error_If_An_Invalid_Choice_Is_Made()
        {
            // Given
            var console = new PlainConsole();
            console.Input.PushText("Apple");
            console.Input.PushText("Banana");

            // When
            console.Prompt(
                new TextPrompt<string>("Favorite fruit?")
                    .AddChoice("Banana")
                    .AddChoice("Orange")
                    .DefaultValue("Banana"));

            // Then
            return Verifier.Verify(console.Lines);
        }

        [Fact]
        public Task Should_Accept_Choice_In_List()
        {
            // Given
            var console = new PlainConsole();
            console.Input.PushText("Orange");

            // When
            console.Prompt(
                new TextPrompt<string>("Favorite fruit?")
                    .AddChoice("Banana")
                    .AddChoice("Orange")
                    .DefaultValue("Banana"));

            // Then
            return Verifier.Verify(console.Lines);
        }

        [Fact]
        public Task Should_Return_Error_If_Custom_Validation_Fails()
        {
            // Given
            var console = new PlainConsole();
            console.Input.PushText("22");
            console.Input.PushText("102");
            console.Input.PushText("ABC");
            console.Input.PushText("99");

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
            return Verifier.Verify(console.Lines);
        }
    }
}
