using System;

namespace Spectre.Console
{
    /// <summary>
    /// Contains extension methods for <see cref="ConfirmationPrompt"/>.
    /// </summary>
    public static class ConfirmationPromptExtensions
    {
        /// <summary>
        /// Show or hide choices.
        /// </summary>
        /// <param name="obj">The prompt.</param>
        /// <param name="show">Whether or not the choices should be visible.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static ConfirmationPrompt ShowChoices(this ConfirmationPrompt obj, bool show)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            obj.ShowChoices = show;
            return obj;
        }

        /// <summary>
        /// Shows choices.
        /// </summary>
        /// <param name="obj">The prompt.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static ConfirmationPrompt ShowChoices(this ConfirmationPrompt obj)
        {
            return ShowChoices(obj, true);
        }

        /// <summary>
        /// Hides choices.
        /// </summary>
        /// <param name="obj">The prompt.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static ConfirmationPrompt HideChoices(this ConfirmationPrompt obj)
        {
            return ShowChoices(obj, false);
        }

        /// <summary>
        /// Show or hide the default value.
        /// </summary>
        /// <param name="obj">The prompt.</param>
        /// <param name="show">Whether or not the default value should be visible.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static ConfirmationPrompt ShowDefaultValue(this ConfirmationPrompt obj, bool show)
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
        /// <param name="obj">The prompt.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static ConfirmationPrompt ShowDefaultValue(this ConfirmationPrompt obj)
        {
            return ShowDefaultValue(obj, true);
        }

        /// <summary>
        /// Hides the default value.
        /// </summary>
        /// <param name="obj">The prompt.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static ConfirmationPrompt HideDefaultValue(this ConfirmationPrompt obj)
        {
            return ShowDefaultValue(obj, false);
        }

        /// <summary>
        /// Sets the "invalid choice" message for the prompt.
        /// </summary>
        /// <param name="obj">The prompt.</param>
        /// <param name="message">The "invalid choice" message.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static ConfirmationPrompt InvalidChoiceMessage(this ConfirmationPrompt obj, string message)
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
        /// <param name="obj">The confirmation prompt.</param>
        /// <param name="character">The character to interpret as "yes".</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static ConfirmationPrompt Yes(this ConfirmationPrompt obj, char character)
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
        /// <param name="obj">The confirmation prompt.</param>
        /// <param name="character">The character to interpret as "no".</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static ConfirmationPrompt No(this ConfirmationPrompt obj, char character)
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
