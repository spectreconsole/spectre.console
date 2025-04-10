namespace Spectre.Console.Tests.Unit.Cli;

public sealed partial class CommandAppTests
{
    public sealed class StartupInterceptor
    {
        public sealed class NoCommand : Command<NoCommand.Settings>
        {
            public sealed class Settings : CommandSettings
            {
            }

            public override int Execute(CommandContext context, Settings settings)
            {
                return 0;
            }
        }

        public sealed class MyInterceptor : IStartupInterceptor
        {
            private readonly Action<StartupContext> _action;

            public MyInterceptor(Action<StartupContext> action)
            {
                _action = action;
            }

            public void Intercept(StartupContext context)
            {
                _action(context);
            }
        }

        [Fact]
        public void Should_Run_The_Interceptor()
        {
            // Given
            var count = 0;
            var registrar = new DefaultTypeRegistrar();
            var interceptor = new MyInterceptor(_ =>
            {
                count += 1;
            });
            registrar.RegisterInstance(typeof(IStartupInterceptor), interceptor);
            var app = new CommandApp<NoCommand>(registrar);

            // When
            app.Run(Array.Empty<string>());

            // Then
            count.ShouldBe(1); // to be sure
        }
    }
}