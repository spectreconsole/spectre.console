using System;
using System.Collections.Generic;
using System.Text;

namespace Spectre.Console
{
    /// <summary>
    /// Represents a console profile.
    /// </summary>
    public sealed class Profile
    {
        private readonly HashSet<string> _enrichers;
        private static readonly string[] _defaultEnricher = new[] { "Default" };

        private IAnsiConsoleOutput _out;
        private Encoding _encoding;
        private Capabilities _capabilities;
        private int? _width;
        private int? _height;

        /// <summary>
        /// Gets the enrichers used to build this profile.
        /// </summary>
        public IReadOnlyCollection<string> Enrichers
        {
            get
            {
                if (_enrichers.Count > 0)
                {
                    return _enrichers;
                }

                return _defaultEnricher;
            }
        }

        /// <summary>
        /// Gets or sets the out buffer.
        /// </summary>
        public IAnsiConsoleOutput Out
        {
            get => _out;
            set
            {
                _out = value ?? throw new InvalidOperationException("Output buffer cannot be null");

                // Reset the width and height if this is a terminal.
                if (value.IsTerminal)
                {
                    _width = null;
                    _height = null;
                }
            }
        }

        /// <summary>
        /// Gets or sets the console output encoding.
        /// </summary>
        public Encoding Encoding
        {
            get => _encoding;
            set
            {
                if (value == null)
                {
                    throw new InvalidOperationException("Encoding cannot be null");
                }

                _out.SetEncoding(value);
                _encoding = value;
            }
        }

        /// <summary>
        /// Gets or sets an explicit console width.
        /// </summary>
        public int Width
        {
            get => _width ?? _out.Width;
            set
            {
                if (value <= 0)
                {
                    throw new InvalidOperationException("Console width must be greater than zero");
                }

                _width = value;
            }
        }

        /// <summary>
        /// Gets or sets an explicit console height.
        /// </summary>
        public int Height
        {
            get => _height ?? _out.Height;
            set
            {
                if (value <= 0)
                {
                    throw new InvalidOperationException("Console height must be greater than zero");
                }

                _height = value;
            }
        }

        /// <summary>
        /// Gets or sets the capabilities of the profile.
        /// </summary>
        public Capabilities Capabilities
        {
            get => _capabilities;
            set
            {
                _capabilities = value ?? throw new InvalidOperationException("Profile capabilities cannot be null");
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Profile"/> class.
        /// </summary>
        /// <param name="out">The output buffer.</param>
        /// <param name="encoding">The output encoding.</param>
        public Profile(IAnsiConsoleOutput @out, Encoding encoding)
        {
            _enrichers = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            _out = @out ?? throw new ArgumentNullException(nameof(@out));
            _encoding = encoding ?? throw new ArgumentNullException(nameof(encoding));
            _capabilities = new Capabilities(_out);
        }

        /// <summary>
        /// Checks whether the current profile supports
        /// the specified color system.
        /// </summary>
        /// <param name="colorSystem">The color system to check.</param>
        /// <returns><c>true</c> if the color system is supported, otherwise <c>false</c>.</returns>
        public bool Supports(ColorSystem colorSystem)
        {
            return (int)colorSystem <= (int)Capabilities.ColorSystem;
        }

        internal void AddEnricher(string name)
        {
            if (name is null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            _enrichers.Add(name);
        }
    }
}
