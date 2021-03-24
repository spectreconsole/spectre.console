using System;
using System.Collections.Generic;
using System.IO;
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

        private TextWriter _out;
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
        public TextWriter Out
        {
            get => _out;
            set
            {
                _out = value ?? throw new InvalidOperationException("Output buffer cannot be null");

                // Reset the width and height if not a TTY.
                if (!Capabilities.Tty)
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

                // Need to update the output encoding for stdout?
                if (_out.IsStandardOut() || _out.IsStandardError())
                {
                    System.Console.OutputEncoding = value;
                }

                _encoding = value;
            }
        }

        /// <summary>
        /// Gets or sets an explicit console width.
        /// </summary>
        public int Width
        {
            get => GetWidth();
            set
            {
                if (_width <= 0)
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
            get => GetHeight();
            set
            {
                if (_height <= 0)
                {
                    throw new InvalidOperationException("Console height must be greater than zero");
                }

                _height = value;
            }
        }

        /// <summary>
        /// Gets or sets the color system.
        /// </summary>
        public ColorSystem ColorSystem { get; set; }

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
        public Profile(TextWriter @out, Encoding encoding)
        {
            _enrichers = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            _out = @out ?? throw new ArgumentNullException(nameof(@out));
            _encoding = encoding ?? throw new ArgumentNullException(nameof(encoding));
            _capabilities = new Capabilities(this);
        }

        /// <summary>
        /// Checks whether the current profile supports
        /// the specified color system.
        /// </summary>
        /// <param name="colorSystem">The color system to check.</param>
        /// <returns><c>true</c> if the color system is supported, otherwise <c>false</c>.</returns>
        public bool Supports(ColorSystem colorSystem)
        {
            return (int)colorSystem <= (int)ColorSystem;
        }

        internal void AddEnricher(string name)
        {
            if (name is null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            _enrichers.Add(name);
        }

        private int GetWidth()
        {
            if (_width != null)
            {
                return _width.Value;
            }

            if (!Capabilities.Tty)
            {
                return ConsoleHelper.GetSafeWidth(Constants.DefaultTerminalWidth);
            }

            return Constants.DefaultTerminalWidth;
        }

        private int GetHeight()
        {
            if (_height != null)
            {
                return _height.Value;
            }

            if (!Capabilities.Tty)
            {
                return ConsoleHelper.GetSafeHeight(Constants.DefaultTerminalHeight);
            }

            return Constants.DefaultTerminalHeight;
        }
    }
}
