namespace Spectre.Console;

/// <summary>
/// Contains extension methods for <see cref="LiveDisplay"/>.
/// </summary>
public static class LiveDisplayExtensions
{
    /// <param name="live">The <see cref="LiveDisplay"/> instance.</param>
    extension(LiveDisplay live)
    {
        /// <summary>
        /// Sets whether or not auto clear is enabled.
        /// If enabled, the live display will be cleared when done.
        /// </summary>
        /// <param name="enabled">Whether or not auto clear is enabled.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public LiveDisplay AutoClear(bool enabled)
        {
            if (live is null)
            {
                throw new ArgumentNullException(nameof(live));
            }

            live.AutoClear = enabled;

            return live;
        }

        /// <summary>
        /// Sets the vertical overflow strategy.
        /// </summary>
        /// <param name="overflow">The overflow strategy to use.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public LiveDisplay Overflow(VerticalOverflow overflow)
        {
            if (live is null)
            {
                throw new ArgumentNullException(nameof(live));
            }

            live.Overflow = overflow;

            return live;
        }

        /// <summary>
        /// Sets the vertical overflow cropping strategy.
        /// </summary>
        /// <param name="cropping">The overflow cropping strategy to use.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public LiveDisplay Cropping(VerticalOverflowCropping cropping)
        {
            if (live is null)
            {
                throw new ArgumentNullException(nameof(live));
            }

            live.Cropping = cropping;

            return live;
        }
    }
}