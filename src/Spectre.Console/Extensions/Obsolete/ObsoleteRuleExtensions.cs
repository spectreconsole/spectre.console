using System;
using System.ComponentModel;

namespace Spectre.Console
{
    /// <summary>
    /// Contains extension methods for <see cref="RuleExtensions"/>.
    /// </summary>
    public static class ObsoleteRuleExtensions
    {
        /// <summary>
        /// Sets the rule title.
        /// </summary>
        /// <param name="rule">The rule.</param>
        /// <param name="title">The title.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        [Obsolete("Use Title(..) instead.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static Rule SetTitle(this Rule rule, string title)
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
        /// <param name="rule">The rule.</param>
        /// <param name="style">The rule style.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        [Obsolete("Use Style(..) instead.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static Rule SetStyle(this Rule rule, Style style)
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
