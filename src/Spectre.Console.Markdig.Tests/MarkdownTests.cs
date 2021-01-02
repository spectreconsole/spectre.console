using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Spectre.Console.Testing;
using VerifyXunit;
using Xunit;

namespace Spectre.Console.Markdig.Tests
{
    [UsesVerify]
    public class MarkdownTests
    {
        [Fact]
        public Task Representative()
        {
            Environment.SetEnvironmentVariable("Verify_DisableClipboard", "true");

            // Representative markdown sample, under MIT license from
            // https://github.com/markdown-it/markdown-it/blob/master/support/demo_template/sample.md
            var resource = Assembly.GetExecutingAssembly()
                .GetManifestResourceStream("Spectre.Console.Markdig.Tests.Data.Sample.md");

            var markdown = new StreamReader(resource ?? throw new InvalidOperationException()).ReadToEnd();

            var console = new FakeConsole();

            var markdownWidget = new Markdown(markdown);
            console.Render(markdownWidget);

            return Verifier.Verify(console.Output);
        }
    }
}