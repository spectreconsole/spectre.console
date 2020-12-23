using System;
using Spectre.Console.Cli;

namespace Spectre.Console.Testing
{
    public sealed class CommandAppFixture
    {
        private Action<CommandApp> _appConfiguration = _ => { };
        private Action<IConfigurator> _configuration;

        public CommandAppFixture()
        {
            _configuration = (_) => { };
        }

        public CommandAppFixture WithDefaultCommand<T>()
            where T : class, ICommand
        {
            _appConfiguration = (app) => app.SetDefaultCommand<T>();
            return this;
        }

        public void Configure(Action<IConfigurator> action)
        {
            _configuration = action;
        }

        public (string Message, string Output) RunAndCatch<T>(params string[] args)
            where T : Exception
        {
            CommandContext context = null;
            CommandSettings settings = null;

            using var console = new FakeConsole();

            var app = new CommandApp();
            _appConfiguration?.Invoke(app);

            app.Configure(_configuration);
            app.Configure(c => c.ConfigureConsole(console));
            app.Configure(c => c.SetInterceptor(new FakeCommandInterceptor((ctx, s) =>
            {
                context = ctx;
                settings = s;
            })));

            try
            {
                app.Run(args);
            }
            catch (T ex)
            {
                var output = console.Output
                    .NormalizeLineEndings()
                    .TrimLines()
                    .Trim();

                return (ex.Message, output);
            }

            throw new InvalidOperationException("No exception was thrown");
        }

        public (int ExitCode, string Output, CommandContext Context, CommandSettings Settings) Run(params string[] args)
        {
            CommandContext context = null;
            CommandSettings settings = null;

            using var console = new FakeConsole(width: int.MaxValue);

            var app = new CommandApp();
            _appConfiguration?.Invoke(app);

            app.Configure(_configuration);
            app.Configure(c => c.ConfigureConsole(console));
            app.Configure(c => c.SetInterceptor(new FakeCommandInterceptor((ctx, s) =>
            {
                context = ctx;
                settings = s;
            })));

            var result = app.Run(args);

            var output = console.Output
                .NormalizeLineEndings()
                .TrimLines()
                .Trim();

            return (result, output, context, settings);
        }
    }
}
