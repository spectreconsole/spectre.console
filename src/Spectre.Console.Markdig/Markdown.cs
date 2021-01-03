using System.Collections.Generic;
using System.Net.Http;
using Markdig;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    public class Markdown : Renderable
    {
        private readonly string _markdownText;
        private MarkdownBlockRendering _blockRendering;

        public Markdown(string markdownText)
        {
            _markdownText = markdownText;

            var inlineRendering = new MarkdownInlineRendering(new HttpClient());
            _blockRendering = new MarkdownBlockRendering(inlineRendering);
        }

        protected override IEnumerable<Segment> Render(RenderContext context, int maxWidth)
        {
            var result = new List<Segment>();
            var doc = Markdig.Markdown.Parse(_markdownText, this.MakePipeline());

            foreach (var element in doc)
            {
                result.AddRange(_blockRendering.RenderBlock(element).Render(context, maxWidth));
            }

            result.Add(Segment.LineBreak);

            return result;
        }

        private MarkdownPipeline MakePipeline()
        {
            return new MarkdownPipelineBuilder()
                .UseAutoIdentifiers()
                .UseEmphasisExtras()
                .UseGridTables()
                .UseMediaLinks()
                .UsePipeTables()
                .UseListExtras()
                .UseDiagrams()
                .UseGenericAttributes()
                .UseEmojiAndSmiley()
                .Build();
        }
    }
}