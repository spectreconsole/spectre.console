namespace Spectre.Console.Tests.Unit.Cli;

public sealed partial class CommandAppTests
{
    public sealed class Interceptor
    {
        public sealed class NoCommand : Command<NoCommand.Settings>
        {
            public sealed class Settings : CommandSettings
            {
            }

            public override int Execute(CommandContext context, Settings settings, CancellationToken cancellationToken)
            {
                return 0;
            }
        }

        public sealed class MyInterceptor : ICommandInterceptor
        {
            private readonly Action<CommandContext, CommandSettings> _action;

            public MyInterceptor(Action<CommandContext, CommandSettings> action)
            {
                _action = action;
            }

            public void Intercept(CommandContext context, CommandSettings settings)
            {
                _action(context, settings);
            }
        }

        public sealed class MyResultInterceptor : ICommandInterceptor
        {
            private readonly Func<CommandContext, CommandSettings, int, int> _function;

            public MyResultInterceptor(Func<CommandContext, CommandSettings, int, int> function)
            {
                _function = function;
            }

            public void InterceptResult(CommandContext context, CommandSettings settings, ref int result)
            {
                result = _function(context, settings, result);
            }
        }

        [Fact]
        public void Should_Run_The_Interceptor()
        {
            // Given
            var count = 0;
            var app = new CommandApp<NoCommand>();
            var interceptor = new MyInterceptor((_, _) =>
            {
                count += 1;
            });
            app.Configure(config => config.SetInterceptor(interceptor));

            // When
            app.Run(Array.Empty<string>());

            // Then
            count.ShouldBe(1); // to be sure
        }

        [Fact]
        public void Should_Run_The_ResultInterceptor()
        {
            // Given
            var count = 0;
            const int Expected = 123;
            var app = new CommandApp<NoCommand>();
            var interceptor = new MyResultInterceptor((_, _, _) =>
            {
                count += 1;
                return Expected;
            });
            app.Configure(config => config.SetInterceptor(interceptor));

            // When
            var actual = app.Run(Array.Empty<string>());

            // Then
            count.ShouldBe(1);
            actual.ShouldBe(Expected);
        }
    }
}