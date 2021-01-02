using System;
using System.IO;
using System.Reflection;
using Spectre.Console;

namespace MarkdownExample
{
    class Program
    {
        static void Main(string[] args)
        {
            var resource = Assembly.GetExecutingAssembly()
                .GetManifestResourceStream("Markdown.Sample.md");
            var markdown = new StreamReader(resource ?? throw new InvalidOperationException()).ReadToEnd();

            var markdownWidget = new Markdown(markdown);

            AnsiConsole.Render(markdownWidget);
        }
    }
}