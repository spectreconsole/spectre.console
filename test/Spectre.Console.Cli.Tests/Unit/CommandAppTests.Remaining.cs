namespace Spectre.Console.Tests.Unit.Cli;

public sealed partial class CommandAppTests
{
    public sealed class Remaining
    {
        [Theory]
        [InlineData("-a")]
        [InlineData("--alive")]
        public void Should_Not_Add_Known_Flags_To_Remaining_Arguments_RelaxedParsing(string knownFlag)
        {
            // Given
            var app = new CommandAppTester();
            app.Configure(config =>
            {
                config.PropagateExceptions();
                config.AddCommand<DogCommand>("dog");
            });

            // When
            var result = app.Run(new[]
            {
                "dog", "12", "4",
                knownFlag,
            });

            // Then
            result.Settings.ShouldBeOfType<DogSettings>().And(dog =>
            {
                dog.IsAlive.ShouldBe(true);
            });

            result.Context.Remaining.Parsed.Count.ShouldBe(0);
            result.Context.Remaining.Raw.Count.ShouldBe(0);
        }

        [Theory]
        [InlineData("-r")]
        [InlineData("--romeo")]
        public void Should_Add_Unknown_Flags_To_Remaining_Arguments_RelaxedParsing(string unknownFlag)
        {
            // Given
            var app = new CommandAppTester();
            app.Configure(config =>
            {
                config.PropagateExceptions();
                config.AddCommand<DogCommand>("dog");
            });

            // When
            var result = app.Run(new[]
            {
                "dog", "12", "4",
                unknownFlag,
            });

            // Then
            result.Context.Remaining.Parsed.Count.ShouldBe(1);
            result.Context.ShouldHaveRemainingArgument(unknownFlag.TrimStart('-'), values: new[] { (string)null });
            result.Context.Remaining.Raw.Count.ShouldBe(0);
        }

        [Fact]
        public void Should_Add_Unknown_Flags_When_Grouped_To_Remaining_Arguments_RelaxedParsing()
        {
            // Given
            var app = new CommandAppTester();
            app.Configure(config =>
            {
                config.PropagateExceptions();
                config.AddCommand<DogCommand>("dog");
            });

            // When
            var result = app.Run(new[]
            {
                "dog", "12", "4",
                "-agr",
            });

            // Then
            result.Context.Remaining.Parsed.Count.ShouldBe(1);
            result.Context.ShouldHaveRemainingArgument("r", values: new[] { (string)null });
            result.Context.Remaining.Raw.Count.ShouldBe(0);
        }

        [Theory]
        [InlineData("-a")]
        [InlineData("--alive")]
        public void Should_Not_Add_Known_Flags_To_Remaining_Arguments_StrictParsing(string knownFlag)
        {
            // Given
            var app = new CommandAppTester();
            app.Configure(config =>
            {
                config.UseStrictParsing();
                config.PropagateExceptions();
                config.AddCommand<DogCommand>("dog");
            });

            // When
            var result = app.Run(new[]
            {
                "dog", "12", "4",
                knownFlag,
            });

            // Then
            result.Context.Remaining.Parsed.Count.ShouldBe(0);
            result.Context.Remaining.Raw.Count.ShouldBe(0);
        }

        [Theory]
        [InlineData("-r")]
        [InlineData("--romeo")]
        public void Should_Not_Add_Unknown_Flags_To_Remaining_Arguments_StrictParsing(string unknownFlag)
        {
            // Given
            var app = new CommandAppTester();
            app.Configure(config =>
            {
                config.UseStrictParsing();
                config.PropagateExceptions();
                config.AddCommand<DogCommand>("dog");
            });

            // When
            var result = Record.Exception(() => app.Run(new[]
            {
                "dog", "12", "4",
                unknownFlag,
            }));

            // Then
            result.ShouldBeOfType<CommandParseException>().And(ex =>
            {
                ex.Message.ShouldBe($"Unknown option '{unknownFlag.TrimStart('-')}'.");
            });
        }

        [Fact]
        public void Should_Not_Add_Unknown_Flags_When_Grouped_To_Remaining_Arguments_StrictParsing()
        {
            // Given
            var app = new CommandAppTester();
            app.Configure(config =>
            {
                config.UseStrictParsing();
                config.PropagateExceptions();
                config.AddCommand<DogCommand>("dog");
            });

            // When
            var result = Record.Exception(() => app.Run(new[]
            {
                "dog", "12", "4",
                "-agr",
            }));

            // Then
            result.ShouldBeOfType<CommandParseException>().And(ex =>
            {
                ex.Message.ShouldBe($"Unknown option 'r'.");
            });
        }

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
                "-bar", "\"baz\"", "qux",
                "foo bar baz qux",
            });

            // Then
            result.Context.Remaining.Parsed.Count.ShouldBe(4);
            result.Context.ShouldHaveRemainingArgument("foo", values: new[] { "bar", "baz" });
            result.Context.ShouldHaveRemainingArgument("b", values: new[] { (string)null });
            result.Context.ShouldHaveRemainingArgument("a", values: new[] { (string)null });
            result.Context.ShouldHaveRemainingArgument("r", values: new[] { (string)null });
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
                "--foo", "bar", "-bar", "\"baz\"", "qux",
                "foo bar baz qux",
            });

            // Then
            result.Context.Remaining.Raw.Count.ShouldBe(6);
            result.Context.Remaining.Raw[0].ShouldBe("--foo");
            result.Context.Remaining.Raw[1].ShouldBe("bar");
            result.Context.Remaining.Raw[2].ShouldBe("-bar");
            result.Context.Remaining.Raw[3].ShouldBe("\"baz\"");
            result.Context.Remaining.Raw[4].ShouldBe("qux");
            result.Context.Remaining.Raw[5].ShouldBe("foo bar baz qux");
        }

        [Fact]
        public void Should_Preserve_Quotes_Hyphen_Delimiters()
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
                "/c", "\"set && pause\"",
                "Name=\" -Rufus --' ",
            });

            // Then
            result.Context.Remaining.Raw.Count.ShouldBe(3);
            result.Context.Remaining.Raw[0].ShouldBe("/c");
            result.Context.Remaining.Raw[1].ShouldBe("\"set && pause\"");
            result.Context.Remaining.Raw[2].ShouldBe("Name=\" -Rufus --' ");
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Should_Convert_Flags_To_Remaining_Arguments_If_Cannot_Be_Assigned(bool useStrictParsing)
        {
            // Given
            var app = new CommandAppTester();
            app.Configure(config =>
            {
                config.Settings.ConvertFlagsToRemainingArguments = true;
                config.Settings.StrictParsing = useStrictParsing;
                config.PropagateExceptions();
                config.AddCommand<DogCommand>("dog");
            });

            // When
            var result = app.Run(new[]
            {
                "dog", "12", "4",
                "--good-boy=Please be good Rufus!",
            });

            // Then
            result.Context.Remaining.Parsed.Count.ShouldBe(1);
            result.Context.ShouldHaveRemainingArgument("good-boy", values: new[] { "Please be good Rufus!" });

            result.Context.Remaining.Raw.Count.ShouldBe(0); // nb. there are no "raw" remaining arguments on the command line
        }
    }
}
