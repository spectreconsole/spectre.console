using System.Diagnostics;
using System.Threading.Tasks;

namespace Spectre.Console.Cli
{
    /// <summary>
    /// Base class for an asynchronous command.
    /// </summary>
    /// <typeparam name="TSettings">The settings type.</typeparam>
    public abstract class AsyncCommand<TSettings> : ICommand<TSettings>
        where TSettings : CommandSettings
    {
        /// <summary>
        /// Validates the specified settings and remaining arguments.
        /// </summary>
        /// <param name="context">The command context.</param>
        /// <param name="settings">The settings.</param>
        /// <returns>The validation result.</returns>
        public virtual ValidationResult Validate(CommandContext context, TSettings settings)
        {
            return ValidationResult.Success();
        }

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="context">The command context.</param>
        /// <param name="settings">The settings.</param>
        /// <returns>An integer indicating whether or not the command executed successfully.</returns>
        public abstract Task<int> ExecuteAsync(CommandContext context, TSettings settings);

        /// <inheritdoc/>
        ValidationResult ICommand.Validate(CommandContext context, CommandSettings settings)
        {
            return Validate(context, (TSettings)settings);
        }

        /// <inheritdoc/>
        Task<int> ICommand.Execute(CommandContext context, CommandSettings settings)
        {
            Debug.Assert(settings is TSettings, "Command settings is of unexpected type.");
            return ExecuteAsync(context, (TSettings)settings);
        }

        /// <inheritdoc/>
        Task<int> ICommand<TSettings>.Execute(CommandContext context, TSettings settings)
        {
            return ExecuteAsync(context, settings);
        }
    }
}