namespace Spectre.Console.Tests.Unit.Cli;

public sealed partial class CommandAppTests
{
    [Fact]
    public void Should_Pass_Case_1()
    {
        // Given
        var app = new CommandAppTester();
        app.Configure(config =>
        {
            config.PropagateExceptions();
            config.AddBranch<AnimalSettings>("animal", animal =>
            {
                animal.AddBranch<MammalSettings>("mammal", mammal =>
                {
                    mammal.AddCommand<DogCommand>("dog");
                    mammal.AddCommand<HorseCommand>("horse");
                });
            });
        });

        // When
        var result = app.Run(new[]
        {
            "animal", "--alive", "mammal", "--name",
            "Rufus", "dog", "12", "--good-boy",
        });

        // Then
        result.ExitCode.ShouldBe(0);
        result.Settings.ShouldBeOfType<DogSettings>().And(dog =>
        {
            dog.Age.ShouldBe(12);
            dog.GoodBoy.ShouldBe(true);
            dog.Name.ShouldBe("Rufus");
            dog.IsAlive.ShouldBe(true);
        });
    }

    [Fact]
    public void Should_Pass_Case_2()
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
            "dog", "12", "4", "--good-boy",
            "--name", "Rufus", "--alive",
        });

        // Then
        result.ExitCode.ShouldBe(0);
        result.Settings.ShouldBeOfType<DogSettings>().And(dog =>
        {
            dog.Legs.ShouldBe(12);
            dog.Age.ShouldBe(4);
            dog.GoodBoy.ShouldBe(true);
            dog.Name.ShouldBe("Rufus");
            dog.IsAlive.ShouldBe(true);
        });
    }

    [Fact]
    public void Should_Pass_Case_3()
    {
        // Given
        var app = new CommandAppTester();
        app.Configure(config =>
        {
            config.PropagateExceptions();
            config.AddBranch<AnimalSettings>("animal", animal =>
            {
                animal.AddCommand<DogCommand>("dog");
                animal.AddCommand<HorseCommand>("horse");
            });
        });

        // When
        var result = app.Run(new[]
        {
            "animal", "dog", "12", "--good-boy",
            "--name", "Rufus",
        });

        // Then
        result.ExitCode.ShouldBe(0);
        result.Settings.ShouldBeOfType<DogSettings>().And(dog =>
        {
            dog.Age.ShouldBe(12);
            dog.GoodBoy.ShouldBe(true);
            dog.Name.ShouldBe("Rufus");
            dog.IsAlive.ShouldBe(false);
        });
    }

    [Fact]
    public void Should_Pass_Case_4()
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
            "animal", "4", "dog", "12", "--good-boy",
            "--name", "Rufus",
        });

        // Then
        result.ExitCode.ShouldBe(0);
        result.Settings.ShouldBeOfType<DogSettings>().And(dog =>
        {
            dog.Legs.ShouldBe(4);
            dog.Age.ShouldBe(12);
            dog.GoodBoy.ShouldBe(true);
            dog.IsAlive.ShouldBe(false);
            dog.Name.ShouldBe("Rufus");
        });
    }

    [Fact]
    public void Should_Pass_Case_5()
    {
        // Given
        var app = new CommandAppTester();
        app.Configure(config =>
        {
            config.PropagateExceptions();
            config.AddCommand<OptionVectorCommand>("multi");
        });

        // When
        var result = app.Run(new[]
        {
            "multi", "--foo", "a", "--foo", "b",
            "--bar", "1", "--foo", "c", "--bar", "2",
        });

        // Then
        result.ExitCode.ShouldBe(0);
        result.Settings.ShouldBeOfType<OptionVectorSettings>().And(vec =>
        {
            vec.Foo.Length.ShouldBe(3);
            vec.Foo.ShouldBe(new[] { "a", "b", "c" });
            vec.Bar.Length.ShouldBe(2);
            vec.Bar.ShouldBe(new[] { 1, 2 });
        });
    }

    [Fact]
    public void Should_Pass_Case_6()
    {
        // Given
        var app = new CommandAppTester();
        app.Configure(config =>
        {
            config.PropagateExceptions();
            config.AddCommand<GenericCommand<ArgumentVectorSettings>>("multi");
        });

        // When
        var result = app.Run(new[]
        {
            "multi", "a", "b", "c",
        });

        // Then
        result.ExitCode.ShouldBe(0);
        result.Settings.ShouldBeOfType<ArgumentVectorSettings>().And(vec =>
        {
            vec.Foo.Length.ShouldBe(3);
            vec.Foo.ShouldBe(new[] { "a", "b", "c" });
        });
    }

    [Fact]
    public void Should_Be_Able_To_Use_Command_Alias()
    {
        // Given
        var app = new CommandAppTester();
        app.Configure(config =>
        {
            config.PropagateExceptions();
            config.AddCommand<OptionVectorCommand>("multi").WithAlias("multiple");
        });

        // When
        var result = app.Run(new[]
        {
            "multiple", "--foo", "a",
        });

        // Then
        result.ExitCode.ShouldBe(0);
        result.Settings.ShouldBeOfType<OptionVectorSettings>().And(vec =>
        {
            vec.Foo.Length.ShouldBe(1);
            vec.Foo.ShouldBe(new[] { "a" });
        });
    }

    [Fact]
    public void Should_Assign_Default_Value_To_Optional_Argument()
    {
        // Given
        var app = new CommandAppTester();
        app.SetDefaultCommand<GenericCommand<OptionalArgumentWithDefaultValueSettings>>();
        app.Configure(config =>
        {
            config.PropagateExceptions();
        });

        // When
        var result = app.Run(Array.Empty<string>());

        // Then
        result.ExitCode.ShouldBe(0);
        result.Settings.ShouldBeOfType<OptionalArgumentWithDefaultValueSettings>().And(settings =>
        {
            settings.Greeting.ShouldBe("Hello World");
        });
    }

    [Fact]
    public void Should_Assign_Property_Initializer_To_Optional_Argument()
    {
        // Given
        var app = new CommandAppTester();
        app.SetDefaultCommand<GenericCommand<OptionalArgumentWithPropertyInitializerSettings>>();
        app.Configure(config =>
        {
            config.PropagateExceptions();
        });

        // When
        var result = app.Run(Array.Empty<string>());

        // Then
        result.ExitCode.ShouldBe(0);
        result.Settings
            .ShouldBeOfType<OptionalArgumentWithPropertyInitializerSettings>()
            .And(settings => settings.Count.ShouldBe(1))
            .And(settings => settings.Value.ShouldBe(0))
            .And(settings => settings.Names.ShouldNotBeNull())
            .And(settings => settings.Names.ShouldNotBeNull())
            .And(settings => settings.Names.ShouldBeEmpty());
    }

    [Fact]
    public void Should_Overwrite_Property_Initializer_With_Argument_Value()
    {
        // Given
        var app = new CommandAppTester();
        app.SetDefaultCommand<GenericCommand<OptionalArgumentWithPropertyInitializerSettings>>();
        app.Configure(config =>
        {
            config.PropagateExceptions();
        });

        // When
        var result = app.Run("-c", "0", "-v", "50", "ABBA", "Herreys");

        // Then
        result.ExitCode.ShouldBe(0);
        result.Settings
            .ShouldBeOfType<OptionalArgumentWithPropertyInitializerSettings>()
            .And(settings => settings.Count.ShouldBe(0))
            .And(settings => settings.Value.ShouldBe(50))
            .And(settings => settings.Names.ShouldContain("ABBA"))
            .And(settings => settings.Names.ShouldContain("Herreys"));
    }

    [Fact]
    public void Should_Assign_Default_Value_To_Optional_Argument_Using_Converter_If_Necessary()
    {
        // Given
        var app = new CommandAppTester();
        app.SetDefaultCommand<GenericCommand<OptionalArgumentWithDefaultValueAndTypeConverterSettings>>();
        app.Configure(config =>
        {
            config.PropagateExceptions();
        });

        // When
        var result = app.Run(Array.Empty<string>());

        // Then
        result.ExitCode.ShouldBe(0);
        result.Settings.ShouldBeOfType<OptionalArgumentWithDefaultValueAndTypeConverterSettings>().And(settings =>
        {
            settings.Greeting.ShouldBe(5);
        });
    }

    [Fact]
    public void Should_Throw_If_Required_Argument_Have_Default_Value()
    {
        // Given
        var app = new CommandAppTester();
        app.SetDefaultCommand<GenericCommand<RequiredArgumentWithDefaultValueSettings>>();
        app.Configure(config =>
        {
            config.PropagateExceptions();
        });

        // When
        var result = Record.Exception(() => app.Run(Array.Empty<string>()));

        // Then
        result.ShouldBeOfType<CommandConfigurationException>().And(ex =>
        {
            ex.Message.ShouldBe("The required argument 'GREETING' cannot have a default value.");
        });
    }

    [Fact]
    public void Should_Throw_If_Alias_Conflicts_With_Another_Command()
    {
        // Given
        var app = new CommandApp();
        app.Configure(config =>
        {
            config.PropagateExceptions();
            config.AddCommand<DogCommand>("dog").WithAlias("cat");
            config.AddCommand<CatCommand>("cat");
        });

        // When
        var result = Record.Exception(() => app.Run(new[] { "dog", "4", "12" }));

        // Then
        result.ShouldBeOfType<CommandConfigurationException>().And(ex =>
        {
            ex.Message.ShouldBe("The alias 'cat' for 'dog' conflicts with another command.");
        });
    }

    [Fact]
    public void Should_Register_Commands_When_Configuring_Application()
    {
        // Given
        var registrar = new FakeTypeRegistrar();
        var app = new CommandApp(registrar);
        app.Configure(config =>
        {
            config.PropagateExceptions();
            config.AddCommand<GenericCommand<FooCommandSettings>>("foo");
            config.AddBranch<AnimalSettings>("animal", animal =>
            {
                animal.AddCommand<DogCommand>("dog");
                animal.AddCommand<HorseCommand>("horse");
            });
        });

        // When
        app.Run(new[]
        {
            "animal", "4", "dog", "12",
        });

        // Then
        registrar.Registrations.ContainsKey(typeof(GenericCommand<FooCommandSettings>)).ShouldBeTrue();
        registrar.Registrations.ContainsKey(typeof(DogCommand)).ShouldBeTrue();
        registrar.Registrations.ContainsKey(typeof(HorseCommand)).ShouldBeTrue();
        registrar.Registrations[typeof(GenericCommand<FooCommandSettings>)].ShouldContain(typeof(GenericCommand<FooCommandSettings>));
        registrar.Registrations[typeof(DogCommand)].ShouldContain(typeof(DogCommand));
        registrar.Registrations[typeof(HorseCommand)].ShouldContain(typeof(HorseCommand));
    }

    [Fact]
    public void Should_Register_Default_Command_When_Configuring_Application()
    {
        // Given
        var registrar = new FakeTypeRegistrar();
        var app = new CommandApp<DogCommand>(registrar);
        app.Configure(config =>
        {
            config.PropagateExceptions();
        });

        // When
        app.Run(new[]
        {
            "12", "4",
        });

        // Then
        registrar.Registrations.ContainsKey(typeof(DogCommand)).ShouldBeTrue();
        registrar.Registrations[typeof(DogCommand)].ShouldContain(typeof(DogCommand));
    }

    [Fact]
    public void Can_Register_Default_Command_Settings_When_Configuring_Application()
    {
        // Given
        var registrar = new FakeTypeRegistrar();
        registrar.Register(typeof(DogSettings), typeof(DogSettings));
        var app = new CommandApp<DogCommand>(registrar);
        app.Configure(config =>
        {
            config.PropagateExceptions();
        });

        // When
        app.Run(new[]
        {
            "12", "4",
        });

        // Then
        registrar.Registrations.ContainsKey(typeof(DogSettings)).ShouldBeTrue();
        registrar.Registrations[typeof(DogSettings)].Count.ShouldBe(1);
        registrar.Registrations[typeof(DogSettings)].ShouldContain(typeof(DogSettings));
    }

    [Fact]
    public void Can_Register_Command_Settings_When_Configuring_Application()
    {
        // Given
        var registrar = new FakeTypeRegistrar();
        registrar.Register(typeof(DogSettings), typeof(DogSettings));
        registrar.Register(typeof(MammalSettings), typeof(MammalSettings));
        var app = new CommandApp(registrar);
        app.Configure(config =>
        {
            config.PropagateExceptions();
            config.AddBranch<AnimalSettings>("animal", animal =>
            {
                animal.AddCommand<DogCommand>("dog");
                animal.AddCommand<HorseCommand>("horse");
            });
        });

        // When
        app.Run(new[]
        {
            "animal", "4", "dog", "12",
        });

        // Then
        registrar.Registrations.ContainsKey(typeof(DogSettings)).ShouldBeTrue();
        registrar.Registrations[typeof(DogSettings)].Count.ShouldBe(1);
        registrar.Registrations[typeof(DogSettings)].ShouldContain(typeof(DogSettings));
        registrar.Registrations.ContainsKey(typeof(MammalSettings)).ShouldBeTrue();
        registrar.Registrations[typeof(MammalSettings)].Count.ShouldBe(1);
        registrar.Registrations[typeof(MammalSettings)].ShouldContain(typeof(MammalSettings));
    }

    [Theory]
    [InlineData("true", true)]
    [InlineData("True", true)]
    [InlineData("false", false)]
    [InlineData("False", false)]
    public void Should_Accept_Explicit_Boolan_Flag(string value, bool expected)
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
            "dog", "12", "4", "--alive", value,
        });

        // Then
        result.ExitCode.ShouldBe(0);
        result.Settings.ShouldBeOfType<DogSettings>().And(dog =>
        {
            dog.IsAlive.ShouldBe(expected);
        });
    }

    [Fact]
    public void Should_Throw_When_Encountering_Unknown_Option_In_Strict_Mode()
    {
        // Given
        var app = new CommandApp();
        app.Configure(config =>
        {
            config.PropagateExceptions();
            config.UseStrictParsing();
            config.AddCommand<DogCommand>("dog");
        });

        // When
        var result = Record.Exception(() => app.Run(new[] { "dog", "--foo" }));

        // Then
        result.ShouldBeOfType<CommandParseException>().And(ex =>
        {
            ex.Message.ShouldBe("Unknown option 'foo'.");
        });
    }

    [Fact]
    public void Should_Add_Unknown_Option_To_Remaining_Arguments_In_Relaxed_Mode()
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
            "animal", "4", "dog", "12",
            "--foo", "bar",
        });

        // Then
        result.Context.ShouldNotBeNull();
        result.Context.Remaining.Parsed.Count.ShouldBe(1);
        result.Context.ShouldHaveRemainingArgument("foo", values: new[] { "bar" });
    }

    [Fact]
    public void Should_Add_Unknown_Option_To_Remaining_Arguments_In_Strict_Mode()
    {
        // Given
        var app = new CommandAppTester();
        app.Configure(config =>
        {
            config.UseStrictParsing();
            config.PropagateExceptions();
            config.AddBranch<AnimalSettings>("animal", animal =>
            {
                animal.AddCommand<DogCommand>("dog");
            });
        });

        // When
        var result = app.Run(new[]
        {
            "animal", "4", "dog", "12",
            "--",
            "--foo", "bar",
            "-f", "baz",
            "qux",
        });

        // Then
        result.Context.ShouldNotBeNull();
        result.Context.Remaining.Parsed.Count.ShouldBe(2);
        result.Context.ShouldHaveRemainingArgument("foo", values: new[] { "bar" });
        result.Context.ShouldHaveRemainingArgument("f", values: new[] { "baz" });
        result.Context.Remaining.Raw.Count.ShouldBe(5);
        result.Context.Remaining.Raw.ShouldBe(new[]
        {
            "--foo", "bar",
            "-f", "baz",
            "qux",
        });
    }

    [Fact]
    public void Should_Add_Unknown_Boolean_Option_To_Remaining_Arguments_In_Relaxed_Mode()
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
            "animal", "4", "dog", "12", "--foo",
        });

        // Then
        result.Context.ShouldNotBeNull();
        result.Context.Remaining.Parsed.Count.ShouldBe(1);
        result.Context.ShouldHaveRemainingArgument("foo", values: new[] { (string)null });
    }

    [Fact]
    public void Should_Be_Able_To_Set_The_Default_Command()
    {
        // Given
        var app = new CommandAppTester();
        app.SetDefaultCommand<DogCommand>();

        // When
        var result = app.Run(new[]
        {
            "4", "12", "--good-boy", "--name", "Rufus",
        });

        // Then
        result.ExitCode.ShouldBe(0);
        result.Settings.ShouldBeOfType<DogSettings>().And(dog =>
        {
            dog.Legs.ShouldBe(4);
            dog.Age.ShouldBe(12);
            dog.GoodBoy.ShouldBe(true);
            dog.Name.ShouldBe("Rufus");
        });
    }

    [Fact]
    public void Should_Set_Command_Name_In_Context()
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
            "animal", "4", "dog", "12",
        });

        // Then
        result.Context.ShouldNotBeNull();
        result.Context.Name.ShouldBe("dog");
    }

    [Fact]
    public void Should_Pass_Command_Data_In_Context()
    {
        // Given
        var app = new CommandAppTester();
        app.Configure(config =>
        {
            config.PropagateExceptions();
            config.AddBranch<AnimalSettings>("animal", animal =>
            {
                animal.AddCommand<DogCommand>("dog").WithData(123);
            });
        });

        // When
        var result = app.Run(new[]
        {
            "animal", "4", "dog", "12",
        });

        // Then
        result.Context.ShouldNotBeNull();
        result.Context.Data.ShouldBe(123);
    }

    public sealed class Delegate_Commands
    {
        [Fact]
        public void Should_Execute_Delegate_Command_At_Root_Level()
        {
            // Given
            var dog = default(DogSettings);
            var data = 0;

            var app = new CommandApp();
            app.Configure(config =>
            {
                config.PropagateExceptions();
                config.AddDelegate<DogSettings>(
                    "foo", (context, settings) =>
                    {
                        dog = settings;
                        data = (int)context.Data;
                        return 1;
                    }).WithData(2);
            });

            // When
            var result = app.Run(new[] { "foo", "4", "12" });

            // Then
            result.ShouldBe(1);
            dog.ShouldNotBeNull();
            dog.Age.ShouldBe(12);
            dog.Legs.ShouldBe(4);
            data.ShouldBe(2);
        }

        [Fact]
        public void Should_Execute_Nested_Delegate_Command()
        {
            // Given
            var dog = default(DogSettings);
            var data = 0;

            var app = new CommandApp();
            app.Configure(config =>
            {
                config.PropagateExceptions();
                config.AddBranch<AnimalSettings>("foo", foo =>
                {
                    foo.AddDelegate<DogSettings>(
                        "bar", (context, settings) =>
                        {
                            dog = settings;
                            data = (int)context.Data;
                            return 1;
                        }).WithData(2);
                });
            });

            // When
            var result = app.Run(new[] { "foo", "4", "bar", "12" });

            // Then
            result.ShouldBe(1);
            dog.ShouldNotBeNull();
            dog.Age.ShouldBe(12);
            dog.Legs.ShouldBe(4);
            data.ShouldBe(2);
        }
    }

    public sealed class Remaining_Arguments
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
                "-bar", "\"baz\"", "qux",
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
            });

            // Then
            result.Context.Remaining.Raw.Count.ShouldBe(5);
            result.Context.Remaining.Raw[0].ShouldBe("--foo");
            result.Context.Remaining.Raw[1].ShouldBe("bar");
            result.Context.Remaining.Raw[2].ShouldBe("-bar");
            result.Context.Remaining.Raw[3].ShouldBe("\"baz\"");
            result.Context.Remaining.Raw[4].ShouldBe("qux");
        }
    }
}
