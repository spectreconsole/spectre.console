using Shouldly;
using Spectre.Console.Cli;
using Xunit;

namespace Spectre.Console.Tests.Unit.Cli.Annotations
{
    public sealed partial class CommandArgumentAttributeTests
    {
        [Fact]
        public void Should_Not_Contain_Options()
        {
            // Given, When
            var result = Record.Exception(() => new CommandArgumentAttribute(0, "--foo <BAR>"));

            // Then
            result.ShouldNotBe(null);
            result.ShouldBeOfType<CommandTemplateException>().And(exception =>
                exception.Message.ShouldBe("Arguments can not contain options."));
        }

        [Theory]
        [InlineData("<FOO> <BAR>")]
        [InlineData("[FOO] [BAR]")]
        [InlineData("[FOO] <BAR>")]
        [InlineData("<FOO> [BAR]")]
        public void Should_Not_Contain_Multiple_Value_Names(string template)
        {
            // Given, When
            var result = Record.Exception(() => new CommandArgumentAttribute(0, template));

            // Then
            result.ShouldNotBe(null);
            result.ShouldBeOfType<CommandTemplateException>().And(exception =>
                exception.Message.ShouldBe("Multiple values are not supported."));
        }

        [Theory]
        [InlineData("<>")]
        [InlineData("[]")]
        public void Should_Not_Contain_Empty_Value_Name(string template)
        {
            // Given, When
            var result = Record.Exception(() => new CommandArgumentAttribute(0, template));

            // Then
            result.ShouldNotBe(null);
            result.ShouldBeOfType<CommandTemplateException>().And(exception =>
                exception.Message.ShouldBe("Values without name are not allowed."));
        }

        [Theory]
        [InlineData("<FOO>", true)]
        [InlineData("[FOO]", false)]
        public void Should_Parse_Valid_Options(string template, bool required)
        {
            // Given, When
            var result = new CommandArgumentAttribute(0, template);

            // Then
            result.ValueName.ShouldBe("FOO");
            result.IsRequired.ShouldBe(required);
        }
    }
}
