namespace Spectre.Console;

/// <summary>
/// Contains extension methods for <see cref="ProgressTask"/>.
/// </summary>
public static class ProgressTaskExtensions
{
    /// <param name="task">The task.</param>
    extension(ProgressTask task)
    {
        /// <summary>
        /// Sets the task description.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public ProgressTask Description(string description)
        {
            if (task is null)
            {
                throw new ArgumentNullException(nameof(task));
            }

            task.Description = description;
            return task;
        }

        /// <summary>
        /// Sets the max value of the task.
        /// </summary>
        /// <param name="value">The max value.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public ProgressTask MaxValue(double value)
        {
            if (task is null)
            {
                throw new ArgumentNullException(nameof(task));
            }

            task.MaxValue = value;
            return task;
        }

        /// <summary>
        /// Sets the value of the task.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public ProgressTask Value(double value)
        {
            if (task is null)
            {
                throw new ArgumentNullException(nameof(task));
            }

            task.Value = value;
            return task;
        }

        /// <summary>
        /// Sets whether the task is considered indeterminate or not.
        /// </summary>
        /// <param name="indeterminate">Whether the task is considered indeterminate or not.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public ProgressTask IsIndeterminate(bool indeterminate = true)
        {
            if (task is null)
            {
                throw new ArgumentNullException(nameof(task));
            }

            task.IsIndeterminate = indeterminate;
            return task;
        }
    }
}