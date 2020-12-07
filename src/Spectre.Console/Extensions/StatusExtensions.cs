using System;

namespace Spectre.Console
{
    /// <summary>
    /// Contains extension methods for <see cref="Status"/>.
    /// </summary>
    public static class StatusExtensions
    {
        /// <summary>
        /// Sets whether or not auto refresh is enabled.
        /// If disabled, you will manually have to refresh the progress.
        /// </summary>
        /// <param name="status">The <see cref="Status"/> instance.</param>
        /// <param name="enabled">Whether or not auto refresh is enabled.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static Status AutoRefresh(this Status status, bool enabled)
        {
            if (status is null)
            {
                throw new ArgumentNullException(nameof(status));
            }

            status.AutoRefresh = enabled;
            return status;
        }

        /// <summary>
        /// Sets the spinner.
        /// </summary>
        /// <param name="status">The <see cref="Status"/> instance.</param>
        /// <param name="spinner">The spinner.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static Status Spinner(this Status status, Spinner spinner)
        {
            if (status is null)
            {
                throw new ArgumentNullException(nameof(status));
            }

            status.Spinner = spinner;
            return status;
        }

        /// <summary>
        /// Sets the spinner style.
        /// </summary>
        /// <param name="status">The <see cref="Status"/> instance.</param>
        /// <param name="style">The spinner style.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static Status SpinnerStyle(this Status status, Style? style)
        {
            if (status is null)
            {
                throw new ArgumentNullException(nameof(status));
            }

            status.SpinnerStyle = style;
            return status;
        }
    }
}
