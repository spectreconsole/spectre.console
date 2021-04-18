using Spectre.Console.Rendering;

namespace Spectre.Console.Testing
{
    /// <summary>
    /// Represents fake capabilities useful in tests.
    /// </summary>
    public sealed class TestCapabilities : IReadOnlyCapabilities
    {
        /// <inheritdoc/>
        public ColorSystem ColorSystem { get; set; } = ColorSystem.TrueColor;

        /// <inheritdoc/>
        public bool Ansi { get; set; }

        /// <inheritdoc/>
        public bool Links { get; set; }

        /// <inheritdoc/>
        public bool Legacy { get; set; }

        /// <inheritdoc/>
        public bool IsTerminal { get; set; }

        /// <inheritdoc/>
        public bool Interactive { get; set; }

        /// <inheritdoc/>
        public bool Unicode { get; set; }

        /// <summary>
        /// Creates a <see cref="RenderContext"/> with the same capabilities as this instace.
        /// </summary>
        /// <returns>A <see cref="RenderContext"/> with the same capabilities as this instace.</returns>
        public RenderContext CreateRenderContext()
        {
            return new RenderContext(this);
        }
    }
}
