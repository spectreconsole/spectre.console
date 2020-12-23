using Shouldly;
using Spectre.Console.Cli;
using Xunit;

namespace Spectre.Console.Tests.Unit.Cli.Annotations
{
    public sealed partial class CommandOptionAttributeTests
    {
        [Fact]
        public void Should_Parse_Short_Name_Correctly()
        {
            // Given, When
            var option = new CommandOptionAttribute("-o|--option <VALUE>");

            // Then
            option.ShortNames.ShouldContain("o");
        }

        [Fact]
        public void Should_Parse_Long_Name_Correctly()
        {
            // Given, When
            var option = new CommandOptionAttribute("-o|--option <VALUE>");

            // Then
            option.LongNames.ShouldContain("option");
        }

        [Theory]
        [InlineData("<VALUE>")]
        public void Should_Parse_Value_Correctly(string value)
        {
            // Given, When
            var option = new CommandOptionAttribute($"-o|--option {value}");

            // Then
            option.ValueName.ShouldBe("VALUE");
        }

        [Fact]
        public void Should_Parse_Only_Short_Name()
        {
            // Given, When
            var option = new CommandOptionAttribute("-o");

            // Then
            option.ShortNames.ShouldContain("o");
        }

        [Fact]
        public void Should_Parse_Only_Long_Name()
        {
            // Given, When
            var option = new CommandOptionAttribute("--option");

            // Then
            option.LongNames.ShouldContain("option");
        }

        [Theory]
        [InlineData("")]
        [InlineData("<VALUE>")]
        public void Should_Throw_If_Template_Is_Empty(string value)
        {
            // Given, When
            var option = Record.Exception(() => new CommandOptionAttribute(value));

            // Then
            option.ShouldBeOfType<CommandTemplateException>().And(e =>
                e.Message.ShouldBe("No long or short name for option has been specified."));
        }

        [Theory]
        [InlineData("--bar|-foo")]
        [InlineData("--bar|-f-b")]
        public void Should_Throw_If_Short_Name_Is_Invalid(string value)
        {
            // Given, When
            var option = Record.Exception(() => new CommandOptionAttribute(value));

            // Then
            option.ShouldBeOfType<CommandTemplateException>().And(e =>
                e.Message.ShouldBe("Short option names can not be longer than one character."));
        }

        [Theory]
        [InlineData("--o")]
        public void Should_Throw_If_Long_Name_Is_Invalid(string value)
        {
            // Given, When
            var option = Record.Exception(() => new CommandOptionAttribute(value));

            // Then
            option.ShouldBeOfType<CommandTemplateException>().And(e =>
                e.Message.ShouldBe("Long option names must consist of more than one character."));
        }

        [Theory]
        [InlineData("-")]
        [InlineData("--")]
        public void Should_Throw_If_Option_Have_No_Name(string template)
        {
            // Given, When
            var option = Record.Exception(() => new CommandOptionAttribute(template));

            // Then
            option.ShouldBeOfType<CommandTemplateException>().And(e =>
                e.Message.ShouldBe("Options without name are not allowed."));
        }

        [Theory]
        [InlineData("--foo|-foo[b", '[')]
        [InlineData("--foo|-f€b", '€')]
        [InlineData("--foo|-foo@b", '@')]
        public void Should_Throw_If_Option_Contains_Invalid_Name(string template, char invalid)
        {
            // Given, When
            var result = Record.Exception(() => new CommandOptionAttribute(template));

            // Then
            result.ShouldBeOfType<CommandTemplateException>().And(e =>
            {
                e.Message.ShouldBe($"Encountered invalid character '{invalid}' in option name.");
                e.Template.ShouldBe(template);
            });
        }

        [Theory]
        [InlineData("--foo <HELLO-WORLD>", "HELLO-WORLD")]
        [InlineData("--foo <HELLO_WORLD>", "HELLO_WORLD")]
        public void Should_Accept_Dash_And_Underscore_In_Value_Name(string template, string name)
        {
            // Given, When
            var result = new CommandOptionAttribute(template);

            // Then
            result.ValueName.ShouldBe(name);
        }

        [Theory]
        [InlineData("--foo|-1")]
        public void Should_Throw_If_First_Letter_Of_An_Option_Name_Is_A_Digit(string template)
        {
            // Given, When
            var result = Record.Exception(() => new CommandOptionAttribute(template));

            // Then
            result.ShouldBeOfType<CommandTemplateException>().And(e =>
            {
                e.Message.ShouldBe("Option names cannot start with a digit.");
                e.Template.ShouldBe(template);
            });
        }

        [Fact]
        public void Multiple_Short_Options_Are_Supported()
        {
            // Given, When
            var result = new CommandOptionAttribute("-f|-b");

            // Then
            result.ShortNames.Count.ShouldBe(2);
            result.ShortNames.ShouldContain("f");
            result.ShortNames.ShouldContain("b");
        }

        [Fact]
        public void Multiple_Long_Options_Are_Supported()
        {
            // Given, When
            var result = new CommandOptionAttribute("--foo|--bar");

            // Then
            result.LongNames.Count.ShouldBe(2);
            result.LongNames.ShouldContain("foo");
            result.LongNames.ShouldContain("bar");
        }

        [Theory]
        [InlineData("-f|--foo <BAR>")]
        [InlineData("--foo|-f <BAR>")]
        [InlineData("<BAR> --foo|-f")]
        [InlineData("<BAR> -f|--foo")]
        [InlineData("-f <BAR> --foo")]
        [InlineData("--foo <BAR> -f")]
        public void Template_Parts_Can_Appear_In_Any_Order(string template)
        {
            // Given, When
            var result = new CommandOptionAttribute(template);

            // Then
            result.LongNames.ShouldContain("foo");
            result.ShortNames.ShouldContain("f");
            result.ValueName.ShouldBe("BAR");
        }
    }
}
