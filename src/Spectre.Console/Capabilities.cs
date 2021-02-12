using System;

namespace Spectre.Console
{
    /// <summary>
    /// Represents console capabilities.
    /// </summary>
    public sealed class Capabilities
    {
        private readonly Profile _profile;

        /// <summary>
        /// Gets or sets a value indicating whether or not
        /// the console supports Ansi.
        /// </summary>
        public bool Ansi { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not
        /// the console support links.
        /// </summary>
        /// <remarks>
        /// There is probably a lot of room for improvement here
        /// once we have more information about the terminal
        /// we're running inside.
        /// </remarks>
        public bool Links { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not
        /// this is a legacy console (cmd.exe) on an OS
        /// prior to Windows 10.
        /// </summary>
        /// <remarks>
        /// Only relevant when running on Microsoft Windows.
        /// </remarks>
        public bool Legacy { get; set; }

        /// <summary>
        /// Gets a value indicating whether console output
        /// has been redirected.
        /// </summary>
        public bool Tty
        {
            get
            {
                if (_profile.Out.IsStandardOut())
                {
                    return System.Console.IsOutputRedirected;
                }

                if (_profile.Out.IsStandardError())
                {
                    return System.Console.IsErrorRedirected;
                }

                // Not stdout, so must be a TTY.
                return true;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether
        /// or not the console supports interaction.
        /// </summary>
        public bool Interactive { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Capabilities"/> class.
        /// </summary>
        internal Capabilities(Profile profile)
        {
            _profile = profile ?? throw new ArgumentNullException(nameof(profile));
        }
    }
}
