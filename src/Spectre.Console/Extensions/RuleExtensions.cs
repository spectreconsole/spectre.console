namespace Spectre.Console;

/// <summary>
/// Contains extension methods for <see cref="RuleExtensions"/>.
/// </summary>
public static class RuleExtensions
{
    /// <param name="rule">The rule.</param>
    extension(Rule rule)
    {
        /// <summary>
        /// Sets the rule title.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public Rule RuleTitle(string title)
        {
            if (rule is null)
            {
                throw new ArgumentNullException(nameof(rule));
            }

            if (title is null)
            {
                throw new ArgumentNullException(nameof(title));
            }

            rule.Title = title;
            return rule;
        }

        /// <summary>
        /// Sets the rule style.
        /// </summary>
        /// <param name="style">The rule style.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public Rule RuleStyle(Style style)
        {
            if (rule is null)
            {
                throw new ArgumentNullException(nameof(rule));
            }

            if (style is null)
            {
                throw new ArgumentNullException(nameof(style));
            }

            rule.Style = style;
            return rule;
        }
    }
}