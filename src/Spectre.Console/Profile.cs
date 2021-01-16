using System;
using System.IO;
using System.Text;

namespace Spectre.Console
{
    /// <summary>
    /// Represents a console profile.
    /// </summary>
    public sealed class Profile
    {
        private string _name;
        private TextWriter _out;
        private Encoding _encoding;
        private Capabilities _capabilities;
        private int? _width;
        private int? _height;

        /// <summary>
        /// Gets or sets the profile name.
        /// </summary>
        public string Name
        {
            get => _name;
            set
            {
                if (value == null)
                {
                    throw new InvalidOperationException("Profile name cannot be null");
                }

                _name = value;
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
                if (value == null)
                {
                    throw new InvalidOperationException("Output buffer cannot be null");
                }

                _out = value;

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
                if (_out.IsStandardOut())
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
                if (value == null)
                {
                    throw new InvalidOperationException("Profile capabilities cannot be null");
                }

                _capabilities = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Profile"/> class.
        /// </summary>
        /// <param name="name">The profile name.</param>
        /// <param name="out">The output buffer.</param>
        /// <param name="encoding">The output encoding.</param>
        public Profile(string name, TextWriter @out, Encoding encoding)
        {
            _name = name ?? throw new ArgumentNullException(nameof(name));
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
