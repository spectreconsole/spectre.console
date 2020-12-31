using Shouldly;
using Spectre.Console.Cli;
using Spectre.Console.Testing;
using Spectre.Console.Tests.Data;
using Xunit;

namespace Spectre.Console.Tests.Unit.Cli
{
    public sealed partial class CommandAppTests
    {
        public sealed class Remaining
        {
            [Fact]
            public void Should_Register_Remaining_Parsed_Arguments_With_Context()
            {
                // Given
                var app = new CommandAppFixture();
                app.Configure(config =>
                {
                    config.PropagateExceptions();
                    config.AddBranch<AnimalSettings>("animal", animal =>
                    {
                        animal.AddCommand<DogCommand>("dog");
                    });
                });

                // When
                var (result, _, ctx, _) = app.Run(new[]
                {
                    "animal", "4", "dog", "12", "--",
                    "--foo", "bar", "--foo", "baz",
                    "-bar", "\"baz\"", "qux",
                    "foo bar baz qux",
                });

                // Then
                ctx.Remaining.Parsed.Count.ShouldBe(4);
                ctx.ShouldHaveRemainingArgument("foo", values: new[] { "bar", "baz" });
                ctx.ShouldHaveRemainingArgument("b", values: new[] { (string)null });
                ctx.ShouldHaveRemainingArgument("a", values: new[] { (string)null });
                ctx.ShouldHaveRemainingArgument("r", values: new[] { (string)null });
            }

            [Fact]
            public void Should_Register_Remaining_Raw_Arguments_With_Context()
            {
                // Given
                var app = new CommandAppFixture();
                app.Configure(config =>
                {
                    config.PropagateExceptions();
                    config.AddBranch<AnimalSettings>("animal", animal =>
                    {
                        animal.AddCommand<DogCommand>("dog");
                    });
                });

                // When
                var (result, _, ctx, _) = app.Run(new[]
                {
                    "animal", "4", "dog", "12", "--",
                    "--foo", "bar", "-bar", "\"baz\"", "qux",
                    "foo bar baz qux",
                });

                // Then
                ctx.Remaining.Raw.Count.ShouldBe(6);
                ctx.Remaining.Raw[0].ShouldBe("--foo");
                ctx.Remaining.Raw[1].ShouldBe("bar");
                ctx.Remaining.Raw[2].ShouldBe("-bar");
                ctx.Remaining.Raw[3].ShouldBe("baz");
                ctx.Remaining.Raw[4].ShouldBe("qux");
                ctx.Remaining.Raw[5].ShouldBe("foo bar baz qux");
            }
        }
    }
}
