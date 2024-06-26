using System.Threading.Tasks;

namespace Spectre.Console;

/// <summary>
/// Contains extension methods for <see cref="IAnsiConsole"/>.
/// </summary>
public static partial class AnsiConsoleExtensions
{
    /// <summary>
    /// Displays a prompt to the user.
    /// </summary>
    /// <typeparam name="T">The prompt result type.</typeparam>
    /// <param name="console">The console.</param>
    /// <param name="prompt">The prompt to display.</param>
    /// <returns>The prompt input result.</returns>
    public static T Prompt<T>(this IAnsiConsole console, IPrompt<T> prompt)
    {
        if (prompt is null)
        {
            throw new ArgumentNullException(nameof(prompt));
        }

        return prompt.Show(console);
    }

    /// <summary>
    /// Displays a prompt to the user.
    /// </summary>
    /// <typeparam name="T">The prompt result type.</typeparam>
    /// <param name="console">The console.</param>
    /// <param name="prompt">The prompt markup text.</param>
    /// <returns>The prompt input result.</returns>
    public static T Ask<T>(this IAnsiConsole console, string prompt)
    {
        return new TextPrompt<T>(prompt).Show(console);
    }

    /// <summary>
    /// Displays a prompt to the user.
    /// </summary>
    /// <typeparam name="T">The prompt result type.</typeparam>
    /// <param name="console">The console.</param>
    /// <param name="prompt">The prompt markup text.</param>
    /// <param name="culture">Specific CultureInfo to use when converting input.</param>
    /// <returns>The prompt input result.</returns>
    public static T Ask<T>(this IAnsiConsole console, string prompt, CultureInfo? culture)
    {
        var textPrompt = new TextPrompt<T>(prompt);
        textPrompt.Culture = culture;
        return textPrompt.Show(console);
    }

    /// <summary>
    /// Displays a prompt with two choices, yes or no.
    /// </summary>
    /// <param name="console">The console.</param>
    /// <param name="prompt">The prompt markup text.</param>
    /// <param name="defaultValue">Specifies the default answer.</param>
    /// <returns><c>true</c> if the user selected "yes", otherwise <c>false</c>.</returns>
    public static bool Confirm(this IAnsiConsole console, string prompt, bool defaultValue = true)
    {
        return new ConfirmationPrompt(prompt)
        {
            DefaultValue = defaultValue,
        }
        .Show(console);
    }

    /// <summary>
    /// Displays a prompt to the user.
    /// </summary>
    /// <typeparam name="T">The prompt result type.</typeparam>
    /// <param name="console">The console.</param>
    /// <param name="prompt">The prompt to display.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>The prompt input result.</returns>
    public static Task<T> PromptAsync<T>(this IAnsiConsole console, IPrompt<T> prompt, CancellationToken cancellationToken = default)
    {
        if (prompt is null)
        {
            throw new ArgumentNullException(nameof(prompt));
        }

        return prompt.ShowAsync(console, cancellationToken);
    }

    /// <summary>
    /// Displays a prompt to the user.
    /// </summary>
    /// <typeparam name="T">The prompt result type.</typeparam>
    /// <param name="console">The console.</param>
    /// <param name="prompt">The prompt markup text.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>The prompt input result.</returns>
    public static Task<T> AskAsync<T>(this IAnsiConsole console, string prompt, CancellationToken cancellationToken = default)
    {
        return new TextPrompt<T>(prompt).ShowAsync(console, cancellationToken);
    }

    /// <summary>
    /// Displays a prompt to the user.
    /// </summary>
    /// <typeparam name="T">The prompt result type.</typeparam>
    /// <param name="console">The console.</param>
    /// <param name="prompt">The prompt markup text.</param>
    /// <param name="culture">Specific CultureInfo to use when converting input.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>The prompt input result.</returns>
    public static Task<T> AskAsync<T>(this IAnsiConsole console, string prompt, CultureInfo? culture, CancellationToken cancellationToken = default)
    {
        var textPrompt = new TextPrompt<T>(prompt);
        textPrompt.Culture = culture;
        return textPrompt.ShowAsync(console, cancellationToken);
    }

    /// <summary>
    /// Displays a prompt with two choices, yes or no.
    /// </summary>
    /// <param name="console">The console.</param>
    /// <param name="prompt">The prompt markup text.</param>
    /// <param name="defaultValue">Specifies the default answer.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns><c>true</c> if the user selected "yes", otherwise <c>false</c>.</returns>
    public static Task<bool> ConfirmAsync(this IAnsiConsole console, string prompt, bool defaultValue = true, CancellationToken cancellationToken = default)
    {
        return new ConfirmationPrompt(prompt)
        {
            DefaultValue = defaultValue,
        }
        .ShowAsync(console, cancellationToken);
    }
}