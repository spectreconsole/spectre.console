using System;
using System.Collections.Generic;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    internal abstract class ProgressRenderer : IRenderHook
    {
        public abstract TimeSpan RefreshRate { get; }

        public virtual void Started()
        {
        }

        public virtual void Completed(bool clear)
        {
        }

        public abstract void Update(ProgressContext context);
        public abstract IEnumerable<IRenderable> Process(RenderContext context, IEnumerable<IRenderable> renderables);
    }
}
