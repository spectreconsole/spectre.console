using System.Collections.Generic;
using System.Linq;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    internal class CompositeRenderable : Renderable
    {
        private readonly IEnumerable<IRenderable> _renderables;

        public CompositeRenderable(IEnumerable<IRenderable> renderables)
        {
            this._renderables = renderables;
        }

        protected override IEnumerable<Segment> Render(RenderContext context, int maxWidth)
        {
            return this._renderables.SelectMany(x => x.Render(context, maxWidth));
        }
    }
}