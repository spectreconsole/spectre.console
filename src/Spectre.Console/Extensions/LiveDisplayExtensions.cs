namespace Spectre.Console;

/// <summary>
/// Contains extension methods for <see cref="LiveDisplay"/>.
/// </summary>
public static class LiveDisplayExtensions
{
    /// <summary>
    /// Sets whether or not auto clear is enabled.
    /// If enabled, the live display will be cleared when done.
    /// </summary>
    /// <param name="live">The <see cref="LiveDisplay"/> instance.</param>
    /// <param name="enabled">Whether or not auto clear is enabled.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static LiveDisplay AutoClear(this LiveDisplay live, bool enabled)
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
    /// <param name="live">The <see cref="LiveDisplay"/> instance.</param>
    /// <param name="overflow">The overflow strategy to use.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static LiveDisplay Overflow(this LiveDisplay live, VerticalOverflow overflow)
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
    /// <param name="live">The <see cref="LiveDisplay"/> instance.</param>
    /// <param name="cropping">The overflow cropping strategy to use.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static LiveDisplay Cropping(this LiveDisplay live, VerticalOverflowCropping cropping)
    {
        if (live is null)
        {
            throw new ArgumentNullException(nameof(live));
        }

        live.Cropping = cropping;

        return live;
    }
}