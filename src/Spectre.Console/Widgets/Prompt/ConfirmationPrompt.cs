namespace Spectre.Console
{
    /// <summary>
    /// A prompt that is answered with a yes or no.
    /// </summary>
    public sealed class ConfirmationPrompt : IPrompt<bool>
    {
        private readonly string _prompt;

        /// <summary>
        /// Gets or sets the character that represents "yes".
        /// </summary>
        public char Yes { get; set; } = 'y';

        /// <summary>
        /// Gets or sets the character that represents "no".
        /// </summary>
        public char No { get; set; } = 'n';

        /// <summary>
        /// Gets or sets a value indicating whether "yes" is the default answer.
        /// </summary>
        public bool DefaultValue { get; set; } = true;

        /// <summary>
        /// Gets or sets the message for invalid choices.
        /// </summary>
        public string InvalidChoiceMessage { get; set; } = "[red]Please select one of the available options[/]";

        /// <summary>
        /// Gets or sets a value indicating whether or not
        /// choices should be shown.
        /// </summary>
        public bool ShowChoices { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether or not
        /// default values should be shown.
        /// </summary>
        public bool ShowDefaultValue { get; set; } = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfirmationPrompt"/> class.
        /// </summary>
        /// <param name="prompt">The prompt markup text.</param>
        public ConfirmationPrompt(string prompt)
        {
            _prompt = prompt ?? throw new System.ArgumentNullException(nameof(prompt));
        }

        /// <inheritdoc/>
        public bool Show(IAnsiConsole console)
        {
            var prompt = new TextPrompt<char>(_prompt)
                .InvalidChoiceMessage(InvalidChoiceMessage)
                .ValidationErrorMessage(InvalidChoiceMessage)
                .ShowChoices(ShowChoices)
                .ShowDefaultValue(ShowDefaultValue)
                .DefaultValue(DefaultValue ? Yes : No)
                .AddChoice(Yes)
                .AddChoice(No);

            var result = prompt.Show(console);
            return result == Yes;
        }
    }
}
