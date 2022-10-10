namespace Spectre.Console.Tests.Unit.Cli;

public sealed partial class CommandAppTests
{
    public sealed class Remaining
    {
        [Fact]
        public void Should_Register_Remaining_Parsed_Arguments_With_Context()
        {
            // Given
            var app = new CommandAppTester();
            app.Configure(config =>
            {
                config.PropagateExceptions();
                config.AddBranch<AnimalSettings>("animal", animal =>
                {
                    animal.AddCommand<DogCommand>("dog");
                });
            });

            // When
            var result = app.Run(new[]
            {
                "animal", "4", "dog", "12", "--",
                "--foo", "bar", "--foo", "baz",
                "-xyz", "\"baz\"", "qux",
                "foo bar baz qux",
                "cmd", "/c", "set && pause",
            });

            // Then
            result.Context.Remaining.Parsed.Count.ShouldBe(4);
            result.Context.ShouldHaveRemainingArgument("foo", values: new[] { "bar", "baz" });
            result.Context.ShouldHaveRemainingArgument("x", values: new[] { (string)null });
            result.Context.ShouldHaveRemainingArgument("y", values: new[] { (string)null });
            result.Context.ShouldHaveRemainingArgument("z", values: new[] { (string)null });
        }

        [Fact]
        public void Should_Register_Remaining_Raw_Arguments_With_Context()
        {
            // Given
            var app = new CommandAppTester();
            app.Configure(config =>
            {
                config.PropagateExceptions();
                config.AddBranch<AnimalSettings>("animal", animal =>
                {
                    animal.AddCommand<DogCommand>("dog");
                });
            });

            // When
            var result = app.Run(new[]
            {
                "animal", "4", "dog", "12", "--",
                "--foo", "bar", "--foo", "baz",
                "-xyz", "\"baz\"", "qux",
                "foo bar baz qux",
                "cmd", "/c", "set && pause",
            });

            // Then
            result.Context.Remaining.Raw.Count.ShouldBe(11);
            result.Context.Remaining.Raw[0].ShouldBe("--foo");
            result.Context.Remaining.Raw[1].ShouldBe("bar");
            result.Context.Remaining.Raw[2].ShouldBe("--foo");
            result.Context.Remaining.Raw[3].ShouldBe("baz");
            result.Context.Remaining.Raw[4].ShouldBe("-xyz");
            result.Context.Remaining.Raw[5].ShouldBe("baz");
            result.Context.Remaining.Raw[6].ShouldBe("qux");
            result.Context.Remaining.Raw[7].ShouldBe("foo bar baz qux");
            result.Context.Remaining.Raw[8].ShouldBe("cmd");
            result.Context.Remaining.Raw[9].ShouldBe("/c");
            result.Context.Remaining.Raw[10].ShouldBe("set && pause");
        }
    }
}
