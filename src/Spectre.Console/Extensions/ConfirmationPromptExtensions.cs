namespace Spectre.Console;

/// <summary>
/// Contains extension methods for <see cref="ConfirmationPrompt"/>.
/// </summary>
public static class ConfirmationPromptExtensions
{
    /// <param name="obj">The prompt.</param>
    extension(ConfirmationPrompt obj)
    {
        /// <summary>
        /// Show or hide choices.
        /// </summary>
        /// <param name="show">Whether or not the choices should be visible.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public ConfirmationPrompt ShowChoices(bool show)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            obj.ShowChoices = show;
            return obj;
        }
    }

    /// <param name="obj">The prompt.</param>
    extension(ConfirmationPrompt obj)
    {
        /// <summary>
        /// Shows choices.
        /// </summary>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public ConfirmationPrompt ShowChoices()
        {
            return ShowChoices(obj, true);
        }

        /// <summary>
        /// Hides choices.
        /// </summary>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public ConfirmationPrompt HideChoices()
        {
            return ShowChoices(obj, false);
        }

        /// <summary>
        /// Sets the style in which the list of choices is displayed.
        /// </summary>
        /// <param name="style">The style to use for displaying the choices or <see langword="null"/> to use the default style (blue).</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public ConfirmationPrompt ChoicesStyle(Style style)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            obj.ChoicesStyle = style;
            return obj;
        }

        /// <summary>
        /// Show or hide the default value.
        /// </summary>
        /// <param name="show">Whether or not the default value should be visible.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public ConfirmationPrompt ShowDefaultValue(bool show)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            obj.ShowDefaultValue = show;
            return obj;
        }

        /// <summary>
        /// Shows the default value.
        /// </summary>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public ConfirmationPrompt ShowDefaultValue()
        {
            return ShowDefaultValue(obj, true);
        }

        /// <summary>
        /// Hides the default value.
        /// </summary>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public ConfirmationPrompt HideDefaultValue()
        {
            return ShowDefaultValue(obj, false);
        }

        /// <summary>
        /// Sets the style in which the default value is displayed.
        /// </summary>
        /// <param name="style">The default value style or <see langword="null"/> to use the default style (green).</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public ConfirmationPrompt DefaultValueStyle(Style? style)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            obj.DefaultValueStyle = style;
            return obj;
        }

        /// <summary>
        /// Sets the "invalid choice" message for the prompt.
        /// </summary>
        /// <param name="message">The "invalid choice" message.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public ConfirmationPrompt InvalidChoiceMessage(string message)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            obj.InvalidChoiceMessage = message;
            return obj;
        }

        /// <summary>
        /// Sets the character to interpret as "yes".
        /// </summary>
        /// <param name="character">The character to interpret as "yes".</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public ConfirmationPrompt Yes(char character)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            obj.Yes = character;
            return obj;
        }

        /// <summary>
        /// Sets the character to interpret as "no".
        /// </summary>
        /// <param name="character">The character to interpret as "no".</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public ConfirmationPrompt No(char character)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            obj.No = character;
            return obj;
        }
    }
}