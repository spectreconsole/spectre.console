namespace Spectre.Console;

public static partial class AnsiConsoleExtensions
{
    extension(AnsiConsole)
    {
        /// <summary>
        /// Displays a prompt to the user.
        /// </summary>
        /// <typeparam name="T">The prompt result type.</typeparam>
        /// <param name="prompt">The prompt to display.</param>
        /// <returns>The prompt input result.</returns>
        public static T Prompt<T>(IPrompt<T> prompt)
        {
            if (prompt is null)
            {
                throw new ArgumentNullException(nameof(prompt));
            }

            return prompt.Show(AnsiConsole.Console);
        }

        /// <summary>
        /// Displays a prompt to the user.
        /// </summary>
        /// <typeparam name="T">The prompt result type.</typeparam>
        /// <param name="prompt">The prompt to display.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The prompt input result.</returns>
        public static Task<T> PromptAsync<T>(IPrompt<T> prompt, CancellationToken cancellationToken = default)
        {
            if (prompt is null)
            {
                throw new ArgumentNullException(nameof(prompt));
            }

            return prompt.ShowAsync(AnsiConsole.Console, cancellationToken);
        }

        /// <summary>
        /// Displays a prompt to the user.
        /// </summary>
        /// <typeparam name="T">The prompt result type.</typeparam>
        /// <param name="prompt">The prompt markup text.</param>
        /// <returns>The prompt input result.</returns>
        public static T Ask<T>(string prompt)
        {
            return new TextPrompt<T>(prompt).Show(AnsiConsole.Console);
        }

        /// <summary>
        /// Displays a prompt to the user.
        /// </summary>
        /// <typeparam name="T">The prompt result type.</typeparam>
        /// <param name="prompt">The prompt markup text.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The prompt input result.</returns>
        public static Task<T> AskAsync<T>(string prompt, CancellationToken cancellationToken = default)
        {
            return new TextPrompt<T>(prompt).ShowAsync(AnsiConsole.Console, cancellationToken);
        }

        /// <summary>
        /// Displays a prompt to the user with a given default.
        /// </summary>
        /// <typeparam name="T">The prompt result type.</typeparam>
        /// <param name="prompt">The prompt markup text.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The prompt input result.</returns>
        public static T Ask<T>(string prompt, T defaultValue)
        {
            return new TextPrompt<T>(prompt)
                .DefaultValue(defaultValue)
                .Show(AnsiConsole.Console);
        }

        /// <summary>
        /// Displays a prompt to the user with a given default.
        /// </summary>
        /// <typeparam name="T">The prompt result type.</typeparam>
        /// <param name="prompt">The prompt markup text.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The prompt input result.</returns>
        public static Task<T> AskAsync<T>(string prompt, T defaultValue, CancellationToken cancellationToken = default)
        {
            return new TextPrompt<T>(prompt)
                .DefaultValue(defaultValue)
                .ShowAsync(AnsiConsole.Console, cancellationToken);
        }

        /// <summary>
        /// Displays a prompt with two choices, yes or no.
        /// </summary>
        /// <param name="prompt">The prompt markup text.</param>
        /// <param name="defaultValue">Specifies the default answer.</param>
        /// <returns><c>true</c> if the user selected "yes", otherwise <c>false</c>.</returns>
        public static bool Confirm(string prompt, bool defaultValue = true)
        {
            return new ConfirmationPrompt(prompt)
            {
                DefaultValue = defaultValue,
            }
                .Show(AnsiConsole.Console);
        }

        /// <summary>
        /// Displays a prompt with two choices, yes or no.
        /// </summary>
        /// <param name="prompt">The prompt markup text.</param>
        /// <param name="defaultValue">Specifies the default answer.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns><c>true</c> if the user selected "yes", otherwise <c>false</c>.</returns>
        public static Task<bool> ConfirmAsync(string prompt, bool defaultValue = true,
            CancellationToken cancellationToken = default)
        {
            return new ConfirmationPrompt(prompt)
            {
                DefaultValue = defaultValue,
            }
                .ShowAsync(AnsiConsole.Console, cancellationToken);
        }
    }

    /// <param name="console">The console.</param>
    extension(IAnsiConsole console)
    {
        /// <summary>
        /// Displays a prompt to the user.
        /// </summary>
        /// <typeparam name="T">The prompt result type.</typeparam>
        /// <param name="prompt">The prompt to display.</param>
        /// <returns>The prompt input result.</returns>
        public T Prompt<T>(IPrompt<T> prompt)
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
        /// <param name="prompt">The prompt markup text.</param>
        /// <returns>The prompt input result.</returns>
        public T Ask<T>(string prompt)
        {
            return new TextPrompt<T>(prompt).Show(console);
        }

        /// <summary>
        /// Displays a prompt to the user.
        /// </summary>
        /// <typeparam name="T">The prompt result type.</typeparam>
        /// <param name="prompt">The prompt markup text.</param>
        /// <param name="culture">Specific CultureInfo to use when converting input.</param>
        /// <returns>The prompt input result.</returns>
        public T Ask<T>(string prompt, CultureInfo? culture)
        {
            var textPrompt = new TextPrompt<T>(prompt);
            textPrompt.Culture = culture;
            return textPrompt.Show(console);
        }

        /// <summary>
        /// Displays a prompt with two choices, yes or no.
        /// </summary>
        /// <param name="prompt">The prompt markup text.</param>
        /// <param name="defaultValue">Specifies the default answer.</param>
        /// <returns><c>true</c> if the user selected "yes", otherwise <c>false</c>.</returns>
        public bool Confirm(string prompt, bool defaultValue = true)
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
        /// <param name="prompt">The prompt to display.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The prompt input result.</returns>
        public Task<T> PromptAsync<T>(IPrompt<T> prompt, CancellationToken cancellationToken = default)
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
        /// <param name="prompt">The prompt markup text.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The prompt input result.</returns>
        public Task<T> AskAsync<T>(string prompt, CancellationToken cancellationToken = default)
        {
            return new TextPrompt<T>(prompt).ShowAsync(console, cancellationToken);
        }

        /// <summary>
        /// Displays a prompt to the user.
        /// </summary>
        /// <typeparam name="T">The prompt result type.</typeparam>
        /// <param name="prompt">The prompt markup text.</param>
        /// <param name="culture">Specific CultureInfo to use when converting input.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The prompt input result.</returns>
        public Task<T> AskAsync<T>(string prompt, CultureInfo? culture, CancellationToken cancellationToken = default)
        {
            var textPrompt = new TextPrompt<T>(prompt);
            textPrompt.Culture = culture;
            return textPrompt.ShowAsync(console, cancellationToken);
        }

        /// <summary>
        /// Displays a prompt with two choices, yes or no.
        /// </summary>
        /// <param name="prompt">The prompt markup text.</param>
        /// <param name="defaultValue">Specifies the default answer.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns><c>true</c> if the user selected "yes", otherwise <c>false</c>.</returns>
        public Task<bool> ConfirmAsync(string prompt, bool defaultValue = true,
            CancellationToken cancellationToken = default)
        {
            return new ConfirmationPrompt(prompt)
            {
                DefaultValue = defaultValue,
            }
                .ShowAsync(console, cancellationToken);
        }
    }
}