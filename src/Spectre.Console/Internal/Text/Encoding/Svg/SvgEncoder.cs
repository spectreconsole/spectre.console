using System.Web;

namespace Spectre.Console.Internal;

internal sealed class SvgEncoder : IAnsiConsoleEncoder
{
    private readonly SvgEncoderSettings _settings;

    public SvgEncoder(SvgEncoderSettings settings)
    {
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));
    }

    public string Encode(IAnsiConsole console, IEnumerable<IRenderable> renderable)
    {
        var template = GetTemplate();
        if (template == null)
        {
            throw new InvalidOperationException("Template could not be found");
        }

        var context = new RenderContext(new EncoderCapabilities(ColorSystem.TrueColor));
        var segmentLines = Segment.SplitLines(
            renderable.SelectMany(r => r.Render(context, console.Profile.Width)));

        var lines = new List<string>();
        foreach (var segmentLine in segmentLines)
        {
            var line = new List<string>();

            foreach (var segment in segmentLine)
            {
                if (segment.IsControlCode)
                {
                    continue;
                }

                var text = HttpUtility.HtmlEncode(segment.Text);

                if (segment.Style != Style.Plain)
                {
                    var rule = SvgCssBuilder.BuildCss(_settings.Theme, segment.Style);
                    text = $"<span style=\"color: #{_settings.Theme.ForegroundColor.ToHex()};{rule}; \">{text}</span>";
                }
                else
                {
                    text = $"<span style=\"color: #{_settings.Theme.ForegroundColor.ToHex()}; \">{text}</span>";
                }

                line.Add(text);
            }

            lines.Add($"<div>{string.Concat(line)}</div>");
        }

        var terminalPadding = 12;
        var requiredCodeHeight = _settings.LineHeight * segmentLines.Count;
        var terminalHeight = requiredCodeHeight + 60;
        var terminalWidth =
            (int)((console.Profile.Width * _settings.FontWidthScale * _settings.FontSize)
            + (2 * terminalPadding)
            + console.Profile.Width);

        var totalHeight = terminalHeight + (2 * _settings.Margin);
        var totalWidth = terminalWidth + (2 * _settings.Margin);

        return template
            .Replace("@(total_width)", totalWidth.ToString())
            .Replace("@(total_height)", totalHeight.ToString())
            .Replace("@(font_size)", _settings.FontSize.ToString())
            .Replace("@(theme_background_color)", "#" + _settings.Theme.BackgroundColor.ToHex())
            .Replace("@(theme_foreground_color)", "#" + _settings.Theme.ForegroundColor.ToHex())
            .Replace("@(line_height)", _settings.LineHeight.ToString())
            .Replace("@(code)", string.Concat(lines));
    }

    private static string? GetTemplate()
    {
        var stream = typeof(SvgEncoder).Assembly.GetManifestResourceStream("Spectre.Console.Svg.Templates.Simple.svg");
        if (stream != null)
        {
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        return null;
    }
}
