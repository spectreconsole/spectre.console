using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Spectre.Console.Cli
{
    /// <summary>
    /// Base class for a command.
    /// </summary>
    /// <typeparam name="TSettings">The settings type.</typeparam>
    /// <seealso cref="AsyncCommand{TSettings}"/>
    public abstract class Command<TSettings> : ICommand<TSettings>
        where TSettings : CommandSettings
    {
        /// <summary>
        /// Validates the specified settings and remaining arguments.
        /// </summary>
        /// <param name="context">The command context.</param>
        /// <param name="settings">The settings.</param>
        /// <returns>The validation result.</returns>
        public virtual ValidationResult Validate([NotNull] CommandContext context, [NotNull] TSettings settings)
        {
            return ValidationResult.Success();
        }

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="context">The command context.</param>
        /// <param name="settings">The settings.</param>
        /// <returns>An integer indicating whether or not the command executed successfully.</returns>
        public abstract int Execute([NotNull] CommandContext context, [NotNull] TSettings settings);

        /// <inheritdoc/>
        ValidationResult ICommand.Validate(CommandContext context, CommandSettings settings)
        {
            return Validate(context, (TSettings)settings);
        }

        /// <inheritdoc/>
        Task<int> ICommand.Execute(CommandContext context, CommandSettings settings)
        {
            Debug.Assert(settings is TSettings, "Command settings is of unexpected type.");
            return Task.FromResult(Execute(context, (TSettings)settings));
        }

        /// <inheritdoc/>
        Task<int> ICommand<TSettings>.Execute(CommandContext context, TSettings settings)
        {
            return Task.FromResult(Execute(context, settings));
        }
    }
}
