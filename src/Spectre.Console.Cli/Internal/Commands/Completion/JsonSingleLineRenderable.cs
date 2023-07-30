#if NET5_0_OR_GREATER
using System.Text.Json;

namespace Spectre.Console.Cli.Internal.Commands.Completion;

internal sealed partial class CompleteCommand
{
    private class JsonSingleLineRenderable<T> : IRenderable
    {

        T _value;

        public JsonSingleLineRenderable(T value)
        {
            _value = value;
        }

        public Measurement Measure(RenderOptions options, int maxWidth)
        {
            return new Measurement(maxWidth, maxWidth);
        }

        public IEnumerable<Segment> Render(RenderOptions options, int maxWidth)
        {
            return new Segment[] { new Segment(JsonSerializer.Serialize(_value)) };
        }
    }

    private class JsonSingleLineRenderable
    {
        public static JsonSingleLineRenderable<T> Create<T>(T value)
        {
            return new JsonSingleLineRenderable<T>(value);
        }
    }
}

#endif