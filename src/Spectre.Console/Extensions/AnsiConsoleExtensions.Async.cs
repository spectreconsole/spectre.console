namespace Spectre.Console;

/// <summary>
/// Provides extension methods for running tasks with a spinner animation.
/// </summary>
public static class SpinnerExtensions
{
    /// <summary>
    /// Runs a task with a spinner animation.
    /// </summary>
    /// <param name="task">The task to run.</param>
    /// <param name="spinner">The spinner to use.</param>
    /// <param name="style">The style to apply to the spinner.</param>
    /// <param name="ansiConsole">The console to write to.</param>
    /// <returns>The result of the task.</returns>
    public static async Task Spinner(this Task task, Spinner? spinner = null, Style? style = null,
        IAnsiConsole? ansiConsole = null)
    {
        ArgumentNullException.ThrowIfNull(task);

        await SpinnerInternal<object>(task, spinner ?? Console.Spinner.Known.Default, style, ansiConsole);
    }

    /// <summary>
    /// Runs a task with a spinner animation.
    /// </summary>
    /// <typeparam name="T">The type of the task result.</typeparam>
    /// <param name="task">The task to run.</param>
    /// <param name="spinner">The spinner to use.</param>
    /// <param name="style">The style to apply to the spinner.</param>
    /// <param name="ansiConsole">The console to write to.</param>
    /// <returns>The result of the task.</returns>
    public static async Task<T> Spinner<T>(this Task<T> task, Spinner? spinner = null, Style? style = null,
        IAnsiConsole? ansiConsole = null)
    {
        ArgumentNullException.ThrowIfNull(task);

        return (await SpinnerInternal<T>(task, spinner ?? Console.Spinner.Known.Default, style, ansiConsole))!;
    }

    private static async Task<T?> SpinnerInternal<T>(Task task, Spinner spinner, Style? style = null,
        IAnsiConsole? ansiConsole = null)
    {
        ansiConsole ??= AnsiConsole.Console;

        style ??= Style.Plain;
        var currentFrame = 0;
        var cancellationTokenSource = new CancellationTokenSource();
        var maxWidth = spinner.Frames.Max(frame => Cell.GetCellLength(frame));

        // Start spinner animation in background
        var spinnerTask = Task.Run(
            async () =>
            {
                while (!cancellationTokenSource.Token.IsCancellationRequested)
                {
                    ansiConsole.Cursor.Show(false);

                    var spinnerFrame = spinner.Frames[currentFrame];
                    var padding = maxWidth - Cell.GetCellLength(spinnerFrame);

                    // Write the spinner frame, padded to the widest frame so that
                    // shorter frames erase whatever a longer frame left behind
                    ansiConsole.Write(new Text(spinnerFrame, style));
                    ansiConsole.Write(new string(' ', padding));
                    ansiConsole.WriteAnsi(w => w.CursorLeft(maxWidth));

                    currentFrame = (currentFrame + 1) % spinner.Frames.Count;
                    await Task.Delay(spinner.Interval, cancellationTokenSource.Token);
                }
            }, cancellationTokenSource.Token);

        try
        {
            // Wait for the actual task to complete
            if (task is Task<T> taskWithResult)
            {
                var result = await taskWithResult;
                await cancellationTokenSource.CancelAsync();
                await spinnerTask.ContinueWith(_ => { }, TaskContinuationOptions.OnlyOnCanceled);

                return result;
            }
            else
            {
                await task;
                await cancellationTokenSource.CancelAsync();
                await spinnerTask.ContinueWith(_ => { }, TaskContinuationOptions.OnlyOnCanceled);

                return default;
            }
        }
        finally
        {
            // Every frame occupies maxWidth columns, so this clears
            // the spinner regardless of which frame is on screen
            ansiConsole.Write(new string(' ', maxWidth));
            ansiConsole.WriteAnsi(w => w.CursorLeft(maxWidth));
            ansiConsole.Cursor.Show();
            await cancellationTokenSource.CancelAsync();
        }
    }
}