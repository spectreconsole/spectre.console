using System;

namespace Spectre.Console.Rendering
{
    /// <summary>
    /// Represents a render hook scope.
    /// </summary>
    public sealed class RenderHookScope : IDisposable
    {
        private readonly IAnsiConsole _console;
        private readonly IRenderHook _hook;

        /// <summary>
        /// Initializes a new instance of the <see cref="RenderHookScope"/> class.
        /// </summary>
        /// <param name="console">The console to attach the render hook to.</param>
        /// <param name="hook">The render hook.</param>
        public RenderHookScope(IAnsiConsole console, IRenderHook hook)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
            _hook = hook ?? throw new ArgumentNullException(nameof(hook));
            _console.Pipeline.Attach(_hook);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            _console.Pipeline.Detach(_hook);
        }
    }
}
