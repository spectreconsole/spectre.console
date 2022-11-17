namespace Spectre.Console.Cli.Tests.Unit;

public sealed class CommandModelBuilderTests
{
    [Fact]
    public void Should_Build_CommandModel_Single_Command()
    {
        // Given
        var registrar = new DefaultTypeRegistrar();
        var configuration = new Configurator(registrar);
        configuration.AddCommand<EmptyCommand>("empty");

        // When
        var commandModel = CommandModelBuilder.Build(configuration);

        // Then
        commandModel.Commands.ShouldHaveSingleItem();
        commandModel.Commands[0].Name.ShouldBe("empty");
        commandModel.Commands[0].CommandType.ShouldBe(typeof(EmptyCommand));
        commandModel.Commands[0].IsDefaultCommand.ShouldBeFalse();
    }

    [Fact]
    public void Should_Build_CommandModel_Default_Command()
    {
        // Given
        var registrar = new DefaultTypeRegistrar();
        var configuration = new Configurator(registrar);
        configuration.SetDefaultCommand<EmptyCommand>();

        // When
        var commandModel = CommandModelBuilder.Build(configuration);

        // Then
        commandModel.Commands.ShouldHaveSingleItem();
        commandModel.Commands[0].ShouldNotBeNull();
        commandModel.Commands[0].Name.ShouldBe(CliConstants.DefaultCommandName);
        commandModel.Commands[0].CommandType.ShouldBe(typeof(EmptyCommand));
        commandModel.Commands[0].IsDefaultCommand.ShouldBeTrue();
    }

    [Fact]
    public void Should_Build_CommandModel_Single_Branch_Single_Command()
    {
        // Given
        var registrar = new DefaultTypeRegistrar();
        var configuration = new Configurator(registrar);
        configuration.AddBranch<AnimalSettings>("animal", animal =>
        {
            animal.AddCommand<DogCommand>("dog");
        });

        // When
        var commandModel = CommandModelBuilder.Build(configuration);

        // Then
        commandModel.Commands.ShouldHaveSingleItem();
        commandModel.Commands[0].Name.ShouldBe("animal");
        commandModel.Commands[0].CommandType.ShouldBeNull();
        commandModel.Commands[0].IsDefaultCommand.ShouldBeFalse();
        commandModel.Commands[0].IsBranch.ShouldBeTrue();

        commandModel.Commands[0].Children.ShouldHaveSingleItem();
        commandModel.Commands[0].Children[0].Name.ShouldBe("dog");
        commandModel.Commands[0].Children[0].CommandType.ShouldBe(typeof(DogCommand));
        commandModel.Commands[0].Children[0].IsDefaultCommand.ShouldBeFalse();
        commandModel.Commands[0].Children[0].IsBranch.ShouldBeFalse();
    }

    [Fact]
    public void Should_Build_CommandModel_Single_Branch_Default_Command()
    {
        // Given
        var registrar = new DefaultTypeRegistrar();
        var configuration = new Configurator(registrar);
        configuration.AddBranch<AnimalSettings>("animal", animal =>
        {
            animal.SetDefaultCommand<HorseCommand>();
        });

        // When
        var commandModel = CommandModelBuilder.Build(configuration);

        // Then
        commandModel.Commands.ShouldHaveSingleItem();
        commandModel.Commands[0].Name.ShouldBe("animal");
        commandModel.Commands[0].CommandType.ShouldBeNull();
        commandModel.Commands[0].IsDefaultCommand.ShouldBeFalse();
        commandModel.Commands[0].IsBranch.ShouldBeTrue();

        commandModel.Commands[0].Children.ShouldHaveSingleItem();
        commandModel.Commands[0].Children[0].Name.ShouldBe(CliConstants.DefaultCommandName);
        commandModel.Commands[0].Children[0].CommandType.ShouldBe(typeof(HorseCommand));
        commandModel.Commands[0].Children[0].IsDefaultCommand.ShouldBeTrue();
        commandModel.Commands[0].Children[0].IsBranch.ShouldBeFalse();
    }

    [Fact]
    public void Should_Build_CommandModel_Single_Branch_Single_Branch_Default_Command()
    {
        // Given
        var registrar = new DefaultTypeRegistrar();
        var configuration = new Configurator(registrar);
        configuration.AddBranch<AnimalSettings>("animal", animal =>
        {
            animal.AddBranch<MammalSettings>("mammal", mammal =>
            {
                mammal.SetDefaultCommand<HorseCommand>();
            });
        });

        // When
        var commandModel = CommandModelBuilder.Build(configuration);

        // Then
        commandModel.Commands.ShouldHaveSingleItem();
        commandModel.Commands[0].Name.ShouldBe("animal");
        commandModel.Commands[0].CommandType.ShouldBeNull();
        commandModel.Commands[0].IsDefaultCommand.ShouldBeFalse();
        commandModel.Commands[0].IsBranch.ShouldBeTrue();

        commandModel.Commands[0].Children.ShouldHaveSingleItem();
        commandModel.Commands[0].Children[0].Name.ShouldBe("mammal");
        commandModel.Commands[0].Children[0].CommandType.ShouldBeNull();
        commandModel.Commands[0].Children[0].IsDefaultCommand.ShouldBeFalse();
        commandModel.Commands[0].Children[0].IsBranch.ShouldBeTrue();

        commandModel.Commands[0].Children[0].Children.ShouldHaveSingleItem();
        commandModel.Commands[0].Children[0].Children[0].Name.ShouldBe(CliConstants.DefaultCommandName);
        commandModel.Commands[0].Children[0].Children[0].CommandType.ShouldBe(typeof(HorseCommand));
        commandModel.Commands[0].Children[0].Children[0].IsDefaultCommand.ShouldBeTrue();
        commandModel.Commands[0].Children[0].Children[0].IsBranch.ShouldBeFalse();
    }

    [Fact]
    public void Should_Build_CommandModel_Single_Branch_Single_Command_Default_Command()
    {
        // Given
        var registrar = new DefaultTypeRegistrar();
        var configuration = new Configurator(registrar);
        configuration.AddBranch<AnimalSettings>("animal", animal =>
        {
            animal.AddCommand<DogCommand>("dog");

            animal.SetDefaultCommand<HorseCommand>();
        });

        // When
        var commandModel = CommandModelBuilder.Build(configuration);

        // Then
        commandModel.Commands.ShouldHaveSingleItem();
        commandModel.Commands[0].Name.ShouldBe("animal");
        commandModel.Commands[0].CommandType.ShouldBeNull();
        commandModel.Commands[0].IsDefaultCommand.ShouldBeFalse();
        commandModel.Commands[0].IsBranch.ShouldBeTrue();

        commandModel.Commands[0].Children.Count.ShouldBe(2);
        commandModel.Commands[0].Children[0].Name.ShouldBe("dog");
        commandModel.Commands[0].Children[0].CommandType.ShouldBe(typeof(DogCommand));
        commandModel.Commands[0].Children[0].IsDefaultCommand.ShouldBeFalse();
        commandModel.Commands[0].Children[0].IsBranch.ShouldBeFalse();

        commandModel.Commands[0].Children[1].Name.ShouldBe(CliConstants.DefaultCommandName);
        commandModel.Commands[0].Children[1].CommandType.ShouldBe(typeof(HorseCommand));
        commandModel.Commands[0].Children[1].IsDefaultCommand.ShouldBeTrue();
        commandModel.Commands[0].Children[1].IsBranch.ShouldBeFalse();
    }

    [Fact]
    public void Should_Build_CommandModel_Default_Command_Single_Branch_Single_Command_Default_Command()
    {
        // Given
        var registrar = new DefaultTypeRegistrar();
        var configuration = new Configurator(registrar);
        configuration.AddBranch<AnimalSettings>("animal", animal =>
        {
            animal.AddCommand<DogCommand>("dog");

            animal.SetDefaultCommand<HorseCommand>();
        });
        configuration.SetDefaultCommand<EmptyCommand>();

        // When
        var commandModel = CommandModelBuilder.Build(configuration);

        // Then
        commandModel.Commands.Count.ShouldBe(2);
        commandModel.Commands[0].Name.ShouldBe("animal");
        commandModel.Commands[0].CommandType.ShouldBeNull();
        commandModel.Commands[0].IsDefaultCommand.ShouldBeFalse();
        commandModel.Commands[0].IsBranch.ShouldBeTrue();

        commandModel.Commands[0].Children.Count.ShouldBe(2);
        commandModel.Commands[0].Children[0].Name.ShouldBe("dog");
        commandModel.Commands[0].Children[0].CommandType.ShouldBe(typeof(DogCommand));
        commandModel.Commands[0].Children[0].IsDefaultCommand.ShouldBeFalse();
        commandModel.Commands[0].Children[0].IsBranch.ShouldBeFalse();

        commandModel.Commands[0].Children[1].Name.ShouldBe(CliConstants.DefaultCommandName);
        commandModel.Commands[0].Children[1].CommandType.ShouldBe(typeof(HorseCommand));
        commandModel.Commands[0].Children[1].IsDefaultCommand.ShouldBeTrue();
        commandModel.Commands[0].Children[1].IsBranch.ShouldBeFalse();

        commandModel.Commands[1].ShouldNotBeNull();
        commandModel.Commands[1].Name.ShouldBe(CliConstants.DefaultCommandName);
        commandModel.Commands[1].CommandType.ShouldBe(typeof(EmptyCommand));
        commandModel.Commands[1].IsDefaultCommand.ShouldBeTrue();
    }
}
