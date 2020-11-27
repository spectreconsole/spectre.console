using System.Collections.Generic;

namespace Spectre.Console.Rendering
{
    /// <summary>
    /// Represents the render pipeline.
    /// </summary>
    public sealed class RenderPipeline
    {
        private readonly List<IRenderHook> _hooks;
        private readonly object _lock;

        /// <summary>
        /// Initializes a new instance of the <see cref="RenderPipeline"/> class.
        /// </summary>
        public RenderPipeline()
        {
            _hooks = new List<IRenderHook>();
            _lock = new object();
        }

        /// <summary>
        /// Attaches a new render hook onto the pipeline.
        /// </summary>
        /// <param name="hook">The render hook to attach.</param>
        public void Attach(IRenderHook hook)
        {
            lock (_lock)
            {
                _hooks.Add(hook);
            }
        }

        /// <summary>
        /// Detaches a render hook from the pipeline.
        /// </summary>
        /// <param name="hook">The render hook to detach.</param>
        public void Detach(IRenderHook hook)
        {
            lock (_lock)
            {
                _hooks.Remove(hook);
            }
        }

        /// <summary>
        /// Processes the specified renderables.
        /// </summary>
        /// <param name="context">The render context.</param>
        /// <param name="renderables">The renderables to process.</param>
        /// <returns>The processed renderables.</returns>
        public IEnumerable<IRenderable> Process(RenderContext context, IEnumerable<IRenderable> renderables)
        {
            lock (_lock)
            {
                var current = renderables;
                for (var index = _hooks.Count - 1; index >= 0; index--)
                {
                    current = _hooks[index].Process(context, current);
                }

                return current;
            }
        }
    }
}
