using System;
using Shouldly;
using Xunit;

namespace Spectre.Console.Tests.Unit
{
    public sealed class PromptTests
    {
        [Fact]
        public void Should_Return_Validation_Error_If_Value_Cannot_Be_Converted()
        {
            // Given
            var console = new PlainConsole();
            console.Input.PushText("ninety-nine");
            console.Input.PushText("99");

            // When
            console.Prompt(new TextPrompt<int>("Age?"));

            // Then
            console.Lines.Count.ShouldBe(3);
            console.Lines[0].ShouldBe("Age? ninety-nine");
            console.Lines[1].ShouldBe("Invalid input");
            console.Lines[2].ShouldBe("Age? 99");
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
        public void Should_Return_Error_If_An_Invalid_Choice_Is_Made()
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
            console.Lines.Count.ShouldBe(3);
            console.Lines[0].ShouldBe("Favorite fruit? [Banana/Orange] (Banana): Apple");
            console.Lines[1].ShouldBe("Please select one of the available options");
            console.Lines[2].ShouldBe("Favorite fruit? [Banana/Orange] (Banana): Banana");
        }

        [Fact]
        public void Should_Accept_Choice_In_List()
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
            console.Lines.Count.ShouldBe(1);
            console.Lines[0].ShouldBe("Favorite fruit? [Banana/Orange] (Banana): Orange");
        }

        [Fact]
        public void Should_Return_Error_If_Custom_Validation_Fails()
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
            console.Lines.Count.ShouldBe(7);
            console.Lines[0].ShouldBe("Guess number: 22");
            console.Lines[1].ShouldBe("Too low");
            console.Lines[2].ShouldBe("Guess number: 102");
            console.Lines[3].ShouldBe("Too high");
            console.Lines[4].ShouldBe("Guess number: ABC");
            console.Lines[5].ShouldBe("Invalid input");
            console.Lines[6].ShouldBe("Guess number: 99");
        }
    }
}
