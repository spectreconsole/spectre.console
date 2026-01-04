namespace Spectre.Console;

/// <summary>
/// Represents a context that can be used to interact with a <see cref="Status"/>.
/// </summary>
public sealed class StatusContext
{
    private readonly ProgressContext _context;
    private readonly ProgressTask _task;
    private readonly SpinnerColumn _spinnerColumn;

    /// <summary>
    /// Gets or sets the current status.
    /// </summary>
    public string Status
    {
        get => _task.Description;
        set => SetStatus(value);
    }

    /// <summary>
    /// Gets or sets the current spinner.
    /// </summary>
    public Spinner Spinner
    {
        get => _spinnerColumn.Spinner;
        set => SetSpinner(value);
    }

    /// <summary>
    /// Gets or sets the current spinner style.
    /// </summary>
    public Style? SpinnerStyle
    {
        get => _spinnerColumn.Style;
        set => _spinnerColumn.Style = value;
    }

    internal StatusContext(ProgressContext context, ProgressTask task, SpinnerColumn spinnerColumn)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _task = task ?? throw new ArgumentNullException(nameof(task));
        _spinnerColumn = spinnerColumn ?? throw new ArgumentNullException(nameof(spinnerColumn));
    }

    /// <summary>
    /// Refreshes the status.
    /// </summary>
    public void Refresh()
    {
        _context.Refresh();
    }

    private void SetStatus(string status)
    {
        ArgumentNullException.ThrowIfNull(status);

        _task.Description = status;
    }

    private void SetSpinner(Spinner spinner)
    {
        ArgumentNullException.ThrowIfNull(spinner);

        _spinnerColumn.Spinner = spinner;
    }
}

/// <summary>
/// Contains extension methods for <see cref="StatusContext"/>.
/// </summary>
public static class StatusContextExtensions
{
    /// <param name="context">The status context.</param>
    extension(StatusContext context)
    {
        /// <summary>
        /// Sets the status message.
        /// </summary>
        /// <param name="status">The status message.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public StatusContext Status(string status)
        {
            ArgumentNullException.ThrowIfNull(context);

            context.Status = status;
            return context;
        }
    }

    /// <summary>
    /// Sets the spinner.
    /// </summary>
    /// <param name="context">The status context.</param>
    /// <param name="spinner">The spinner.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static StatusContext Spinner(this StatusContext context, Spinner spinner)
    {
        ArgumentNullException.ThrowIfNull(context);

        context.Spinner = spinner;
        return context;
    }

    /// <summary>
    /// Sets the spinner style.
    /// </summary>
    /// <param name="context">The status context.</param>
    /// <param name="style">The spinner style.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static StatusContext SpinnerStyle(this StatusContext context, Style? style)
    {
        ArgumentNullException.ThrowIfNull(context);

        context.SpinnerStyle = style;
        return context;
    }
}