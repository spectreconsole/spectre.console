using System;
using System.Diagnostics.CodeAnalysis;

namespace Spectre.Console.Internal
{
    [SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "Not used (yet)")]
    internal sealed class LineBreakElement : IRenderable
    {
        /// <inheritdoc/>
        public int Length => 0;

        /// <inheritdoc/>
        public void Render(IAnsiConsole renderer)
        {
            renderer.Write(Environment.NewLine);
        }
    }
}
