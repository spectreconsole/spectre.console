namespace Spectre.Console
{
    /// <summary>
    /// Represents settings for a progress task.
    /// </summary>
    public sealed class ProgressTaskSettings
    {
        /// <summary>
        /// Gets or sets the task's max value.
        /// Defaults to <c>100</c>.
        /// </summary>
        public double MaxValue { get; set; } = 100;

        /// <summary>
        /// Gets or sets a value indicating whether or not the task
        /// will be auto started. Defaults to <c>true</c>.
        /// </summary>
        public bool AutoStart { get; set; } = true;

        /// <summary>
        /// Gets the default progress task settings.
        /// </summary>
        internal static ProgressTaskSettings Default { get; } = new ProgressTaskSettings();
    }
}
