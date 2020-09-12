using System;
using System.Diagnostics.CodeAnalysis;
using Shouldly;
using Xunit;

namespace Spectre.Console.Tests.Unit
{
    public partial class AnsiConsoleTests
    {
        [SuppressMessage("Naming", "CA1724:Type names should not match namespaces")]
        public sealed class Markup
        {
            [Theory]
            [InlineData("[yellow]Hello[/]", "[93mHello[0m")]
            [InlineData("[yellow]Hello [italic]World[/]![/]", "[93mHello [0m[3;93mWorld[0m[93m![0m")]
            [InlineData("[link=https://patriksvensson.se]Click to visit my blog[/]", "]8;id=1024;https://patriksvensson.se\\Click to visit my blog]8;;\\")]
            [InlineData("[link]https://patriksvensson.se[/]", "]8;id=1024;https://patriksvensson.se\\https://patriksvensson.se]8;;\\")]
            public void Should_Output_Expected_Ansi_For_Markup(string markup, string expected)
            {
                // Given
                var console = new TestableAnsiConsole(ColorSystem.Standard, AnsiSupport.Yes);

                // When
                console.Markup(markup);

                // Then
                console.Output.ShouldBe(expected);
            }

            [Theory]
            [InlineData("[yellow]Hello [[ World[/]", "[93mHello [ World[0m")]
            public void Should_Be_Able_To_Escape_Tags(string markup, string expected)
            {
                // Given
                var console = new TestableAnsiConsole(ColorSystem.Standard, AnsiSupport.Yes);

                // When
                console.Markup(markup);

                // Then
                console.Output.ShouldBe(expected);
            }

            [Theory]
            [InlineData("[yellow]Hello[", "Encountered malformed markup tag at position 14.")]
            [InlineData("[yellow]Hello[/", "Encountered malformed markup tag at position 15.")]
            [InlineData("[yellow]Hello[/foo", "Encountered malformed markup tag at position 15.")]
            [InlineData("[yellow Hello", "Encountered malformed markup tag at position 13.")]
            public void Should_Throw_If_Encounters_Malformed_Tag(string markup, string expected)
            {
                // Given
                var console = new TestableAnsiConsole(ColorSystem.Standard, AnsiSupport.Yes);

                // When
                var result = Record.Exception(() => console.Markup(markup));

                // Then
                result.ShouldBeOfType<InvalidOperationException>()
                    .Message.ShouldBe(expected);
            }

            [Fact]
            public void Should_Throw_If_Tags_Are_Unbalanced()
            {
                // Given
                var console = new TestableAnsiConsole(ColorSystem.Standard, AnsiSupport.Yes);

                // When
                var result = Record.Exception(() => console.Markup("[yellow][blue]Hello[/]"));

                // Then
                result.ShouldBeOfType<InvalidOperationException>()
                    .Message.ShouldBe("Unbalanced markup stack. Did you forget to close a tag?");
            }

            [Fact]
            public void Should_Throw_If_Encounters_Closing_Tag()
            {
                // Given
                var console = new TestableAnsiConsole(ColorSystem.Standard, AnsiSupport.Yes);

                // When
                var result = Record.Exception(() => console.Markup("Hello[/]World"));

                // Then
                result.ShouldBeOfType<InvalidOperationException>()
                    .Message.ShouldBe("Encountered closing tag when none was expected near position 5.");
            }
        }
    }
}
