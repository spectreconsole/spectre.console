namespace Spectre.Console.Cli
{
    /// <summary>
    /// Base class for command settings.
    /// </summary>
    public abstract class CommandSettings
    {
        /// <summary>
        /// Performs validation of the settings.
        /// </summary>
        /// <returns>The validation result.</returns>
        public virtual ValidationResult Validate()
        {
            return ValidationResult.Success();
        }
    }
}
