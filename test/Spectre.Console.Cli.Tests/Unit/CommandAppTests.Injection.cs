namespace Spectre.Console.Tests.Unit.Cli;

public sealed partial class CommandAppTests
{
    public sealed class Injection
    {
        public sealed class FakeDependency
        {
        }

        public sealed class InjectSettings : CommandSettings
        {
            public FakeDependency Fake { get; set; }

            [CommandOption("--name <NAME>")]
            public string Name { get; }

            [CommandOption("--age <AGE>")]
            public int Age { get; set; }

            public InjectSettings(FakeDependency fake, string name)
            {
                Fake = fake;
                Name = "Hello " + name;
            }
        }

        [Fact]
        public void Should_Inject_Parameters()
        {
            // Given
            var app = new CommandAppTester();
            var dependency = new FakeDependency();

            app.SetDefaultCommand<GenericCommand<InjectSettings>>();
            app.Configure(config =>
            {
                config.Settings.Registrar.RegisterInstance(dependency);
                config.PropagateExceptions();
            });

            // When
            var result = app.Run(new[]
            {
                "--name", "foo",
                "--age", "35",
            });

            // Then
            result.ExitCode.ShouldBe(0);
            result.Settings.ShouldBeOfType<InjectSettings>().And(injected =>
            {
                injected.ShouldNotBeNull();
                injected.Fake.ShouldBeSameAs(dependency);
                injected.Name.ShouldBe("Hello foo");
                injected.Age.ShouldBe(35);
            });
        }

        [Fact]
        public void Should_Inject_Dependency_Using_A_Given_Registrar()
        {
            // Given
            var dependency = new FakeDependency();
            var registrar = new FakeTypeRegistrar();
            registrar.RegisterInstance(typeof(FakeDependency), dependency);
            var app = new CommandAppTester(registrar);

            app.SetDefaultCommand<GenericCommand<InjectSettings>>();
            app.Configure(config => config.PropagateExceptions());

            // When
            var result = app.Run(new[]
            {
                "--name", "foo",
                "--age", "35",
            });

            // Then
            result.ExitCode.ShouldBe(0);
            result.Settings.ShouldBeOfType<InjectSettings>().And(injected =>
            {
                injected.ShouldNotBeNull();
                injected.Fake.ShouldBeSameAs(dependency);
                injected.Name.ShouldBe("Hello foo");
                injected.Age.ShouldBe(35);
            });
        }
    }
}
