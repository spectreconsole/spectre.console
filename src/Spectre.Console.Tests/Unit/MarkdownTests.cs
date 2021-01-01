using System;
using System.Threading.Tasks;
using Spectre.Console.Testing;
using VerifyXunit;
using Xunit;

namespace Spectre.Console.Tests.Unit
{
    [UsesVerify]
    public class MarkdownTests
    {
        [Fact]
        public Task SimpleHeaderAndTextDoc()
        {
            Environment.SetEnvironmentVariable("Verify_DisableClipboard", "true");
            var markdown =
                @"# Heading level 1
Some text
## Heading level 2
 - a list
 - of things
### Heading level 3	
#### Heading level 4	
##### Heading level 5
Some text	
###### Heading level 6";

            var console = new FakeConsole();

            var markdownWidget = new Markdown(markdown);
            console.Render(markdownWidget);

            return Verifier.Verify(console.Output);
        }
    }
}