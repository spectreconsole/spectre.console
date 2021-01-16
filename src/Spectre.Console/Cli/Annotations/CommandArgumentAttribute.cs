using System;

namespace Spectre.Console.Cli
{
    /// <summary>
    /// An attribute representing a command argument.
    /// </summary>
    /// <seealso cref="Attribute" />
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class CommandArgumentAttribute : Attribute
    {
        /// <summary>
        /// Gets the argument position.
        /// </summary>
        /// <value>The argument position.</value>
        public int Position { get; }

        /// <summary>
        /// Gets the value name of the argument.
        /// </summary>
        /// <value>The value name of the argument.</value>
        public string ValueName { get; }

        /// <summary>
        /// Gets a value indicating whether the argument is required.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the argument is required; otherwise, <c>false</c>.
        /// </value>
        public bool IsRequired { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandArgumentAttribute"/> class.
        /// </summary>
        /// <param name="position">The argument position.</param>
        /// <param name="template">The argument template.</param>
        public CommandArgumentAttribute(int position, string template)
        {
            if (template == null)
            {
                throw new ArgumentNullException(nameof(template));
            }

            // Parse the option template.
            var result = TemplateParser.ParseArgumentTemplate(template);

            // Assign the result.
            Position = position;
            ValueName = result.Value;
            IsRequired = result.Required;
        }
    }
}
