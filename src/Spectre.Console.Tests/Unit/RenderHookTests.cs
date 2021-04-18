using System.Collections.Generic;
using System.Linq;
using Shouldly;
using Spectre.Console.Rendering;
using Spectre.Console.Testing;
using Xunit;

namespace Spectre.Console.Tests.Unit
{
    public sealed class RenderHookTests
    {
        private sealed class HelloRenderHook : IRenderHook
        {
            public IEnumerable<IRenderable> Process(RenderContext context, IEnumerable<IRenderable> renderables)
            {
                return new IRenderable[] { new Text("Hello\n") }.Concat(renderables);
            }
        }

        [Fact]
        public void Should_Inject_Renderable_Before_Writing_To_Console()
        {
            // Given
            var console = new TestConsole();
            console.Pipeline.Attach(new HelloRenderHook());

            // When
            console.Write(new Text("World"));

            // Then
            console.Lines[0].ShouldBe("Hello");
            console.Lines[1].ShouldBe("World");
        }
    }
}
