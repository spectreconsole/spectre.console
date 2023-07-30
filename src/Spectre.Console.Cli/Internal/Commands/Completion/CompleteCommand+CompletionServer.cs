#if NET5_0_OR_GREATER

using System.Text.Json;

#endif

namespace Spectre.Console.Cli.Internal.Commands.Completion;

internal partial class CompleteCommand
{
    private async Task<int> RunCompletionServer(Settings settings)
    {
        // Uncomment for some kind of debugging through pwsh
        // if (!Debugger.IsAttached)
        // {
        //     Debugger.Launch();
        // }
        while (true)
        {
            var line = await System.Console.In.ReadLineAsync();
            if (
                line is null
                || string.IsNullOrWhiteSpace(line)
                || string.Equals(line, "exit", StringComparison.OrdinalIgnoreCase))
            {
                return 0;
            }

            try
            {
                var args = GetLineParams(line);
                var parser = new CommandCompletionContextParser(_model, _configuration);
                var ctx = parser.Parse(
                    args.Command,
                    args.CursorPosition);

                var completions = await GetCompletionsAsync(ctx);
                await RenderCompletionAsync(completions, settings);
            }
            catch (Exception e)
            {
                // ignored
                System.Console.WriteLine(e.ToString().Replace("\n", "\\n"));
                return -1;
            }
        }
    }

    private static TabCompletionArgs GetLineParams(string line)
    {
        var result = new TabCompletionArgs(line);

#if NET5_0_OR_GREATER
        // When starts with { and ends with }, it's a json object
        var normalizedLine = line.Trim(' ', '\t', '\r', '\n');
        var couldBeJson = normalizedLine.StartsWith("{") && normalizedLine.EndsWith("}");
        if (couldBeJson)
        {
            try
            {
                var deserialized = JsonSerializer.Deserialize<TabCompletionArgs>(line, new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true,
                });
                if (deserialized != null)
                {
                    result = deserialized;
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }
#endif

        return result;
    }

#if NET5_0_OR_GREATER

    private class JsonSingleLineRenderable<T> : IRenderable
    {
        private T _value;

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

#endif
}