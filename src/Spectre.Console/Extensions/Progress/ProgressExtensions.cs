using System;
using System.Linq;

namespace Spectre.Console
{
    /// <summary>
    /// Contains extension methods for <see cref="Progress"/>.
    /// </summary>
    public static class ProgressExtensions
    {
        /// <summary>
        /// Sets the columns to be used for an <see cref="Progress"/> instance.
        /// </summary>
        /// <param name="progress">The <see cref="Progress"/> instance.</param>
        /// <param name="columns">The columns to use.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static Progress Columns(this Progress progress, params ProgressColumn[] columns)
        {
            if (progress is null)
            {
                throw new ArgumentNullException(nameof(progress));
            }

            if (columns is null)
            {
                throw new ArgumentNullException(nameof(columns));
            }

            if (!columns.Any())
            {
                throw new InvalidOperationException("At least one column must be specified.");
            }

            progress.Columns.Clear();
            progress.Columns.AddRange(columns);

            return progress;
        }

        /// <summary>
        /// Sets whether or not auto refresh is enabled.
        /// If disabled, you will manually have to refresh the progress.
        /// </summary>
        /// <param name="progress">The <see cref="Progress"/> instance.</param>
        /// <param name="enabled">Whether or not auto refresh is enabled.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static Progress AutoRefresh(this Progress progress, bool enabled)
        {
            if (progress is null)
            {
                throw new ArgumentNullException(nameof(progress));
            }

            progress.AutoRefresh = enabled;

            return progress;
        }

        /// <summary>
        /// Sets whether or not auto clear is enabled.
        /// If enabled, the task tabled will be removed once
        /// all tasks have completed.
        /// </summary>
        /// <param name="progress">The <see cref="Progress"/> instance.</param>
        /// <param name="enabled">Whether or not auto clear is enabled.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static Progress AutoClear(this Progress progress, bool enabled)
        {
            if (progress is null)
            {
                throw new ArgumentNullException(nameof(progress));
            }

            progress.AutoClear = enabled;

            return progress;
        }

        /// <summary>
        /// Sets whether or not hide completed is enabled.
        /// If enabled, the task tabled will be removed once it is
        /// completed.
        /// </summary>
        /// <param name="progress">The <see cref="Progress"/> instance.</param>
        /// <param name="enabled">Whether or not hide completed is enabled.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static Progress HideCompleted(this Progress progress, bool enabled)
        {
            if (progress is null)
            {
                throw new ArgumentNullException(nameof(progress));
            }

            progress.HideCompleted = enabled;

            return progress;
        }
    }
}
