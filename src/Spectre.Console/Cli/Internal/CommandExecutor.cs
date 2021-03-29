using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Spectre.Console.Cli
{
    internal sealed class CommandExecutor
    {
        private readonly ITypeRegistrar _registrar;

        public CommandExecutor(ITypeRegistrar registrar)
        {
            _registrar = registrar ?? throw new ArgumentNullException(nameof(registrar));
            _registrar.Register(typeof(DefaultPairDeconstructor), typeof(DefaultPairDeconstructor));
        }

        public Task<int> Execute(IConfiguration configuration, IEnumerable<string> args)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            _registrar.RegisterInstance(typeof(IConfiguration), configuration);
            _registrar.RegisterLazy(typeof(IAnsiConsole), () => configuration.Settings.Console.GetConsole());

            // Create the command model.
            var model = CommandModelBuilder.Build(configuration);
            _registrar.RegisterInstance(typeof(CommandModel), model);
            _registrar.RegisterDependencies(model);

            // No default command?
            if (model.DefaultCommand == null)
            {
                // Got at least one argument?
                var firstArgument = args.FirstOrDefault();
                if (firstArgument != null)
                {
                    // Asking for version? Kind of a hack, but it's alright.
                    // We should probably make this a bit better in the future.
                    if (firstArgument.Equals("--version", StringComparison.OrdinalIgnoreCase) ||
                        firstArgument.Equals("-v", StringComparison.OrdinalIgnoreCase))
                    {
                        var console = configuration.Settings.Console.GetConsole();
                        console.WriteLine(ResolveApplicationVersion(configuration));
                        return Task.FromResult(0);
                    }
                }
            }

            // Parse and map the model against the arguments.
            var parser = new CommandTreeParser(model, configuration.Settings);
            var parsedResult = parser.Parse(args);
            _registrar.RegisterInstance(typeof(CommandTreeParserResult), parsedResult);

            // Currently the root?
            if (parsedResult.Tree == null)
            {
                // Display help.
                configuration.Settings.Console.SafeRender(HelpWriter.Write(model));
                return Task.FromResult(0);
            }

            // Get the command to execute.
            var leaf = parsedResult.Tree.GetLeafCommand();
            if (leaf.Command.IsBranch || leaf.ShowHelp)
            {
                // Branches can't be executed. Show help.
                configuration.Settings.Console.SafeRender(HelpWriter.WriteCommand(model, leaf.Command));
                return Task.FromResult(leaf.ShowHelp ? 0 : 1);
            }

            // Register the arguments with the container.
            _registrar.RegisterInstance(typeof(IRemainingArguments), parsedResult.Remaining);

            // Create the resolver and the context.
            using var resolver = new TypeResolverAdapter(_registrar.Build());
            var context = new CommandContext(parsedResult.Remaining, leaf.Command.Name, leaf.Command.Data);

            // Execute the command tree.
            return Execute(leaf, parsedResult.Tree, context, resolver, configuration);
        }

        private static string ResolveApplicationVersion(IConfiguration configuration)
        {
            return
                configuration.Settings.ApplicationVersion ?? // potential override
                VersionHelper.GetVersion(Assembly.GetEntryAssembly());
        }

        private static Task<int> Execute(
            CommandTree leaf,
            CommandTree tree,
            CommandContext context,
            ITypeResolver resolver,
            IConfiguration configuration)
        {
            // Bind the command tree against the settings.
            var settings = CommandBinder.Bind(tree, leaf.Command.SettingsType, resolver);
            configuration.Settings.Interceptor?.Intercept(context, settings);

            // Create and validate the command.
            var command = leaf.CreateCommand(resolver);
            var validationResult = command.Validate(context, settings);
            if (!validationResult.Successful)
            {
                throw CommandRuntimeException.ValidationFailed(validationResult);
            }

            // Execute the command.
            return command.Execute(context, settings);
        }
    }
}
