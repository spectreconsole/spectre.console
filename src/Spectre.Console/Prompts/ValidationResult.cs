namespace Spectre.Console
{
    /// <summary>
    /// Represents a prompt validation result.
    /// </summary>
    public sealed class ValidationResult
    {
        /// <summary>
        /// Gets a value indicating whether or not validation was successful.
        /// </summary>
        public bool Successful { get; }

        /// <summary>
        /// Gets the validation error message.
        /// </summary>
        public string? Message { get; }

        private ValidationResult(bool successful, string? message)
        {
            Successful = successful;
            Message = message;
        }

        /// <summary>
        /// Returns a <see cref="ValidationResult"/> representing successful validation.
        /// </summary>
        /// <returns>The validation result.</returns>
        public static ValidationResult Success()
        {
            return new ValidationResult(true, null);
        }

        /// <summary>
        /// Returns a <see cref="ValidationResult"/> representing a validation error.
        /// </summary>
        /// <param name="message">The validation error message, or <c>null</c> to show the default validation error message.</param>
        /// <returns>The validation result.</returns>
        public static ValidationResult Error(string? message = null)
        {
            return new ValidationResult(false, message);
        }
    }
}
