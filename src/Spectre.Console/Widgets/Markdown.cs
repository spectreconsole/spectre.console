using System;
using System.Collections.Generic;
using System.Linq;
using Markdig.Extensions.Abbreviations;
using Markdig.Extensions.AutoIdentifiers;
using Markdig.Extensions.CustomContainers;
using Markdig.Extensions.DefinitionLists;
using Markdig.Extensions.Figures;
using Markdig.Extensions.Footers;
using Markdig.Extensions.Footnotes;
using Markdig.Extensions.Mathematics;
using Markdig.Extensions.Tables;
using Markdig.Extensions.Yaml;
using Markdig.Syntax;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    public class Markdown : Renderable
    {
        private readonly string _markdownText;

        public Markdown(string markdownText)
        {
            _markdownText = markdownText;
        }

        protected override IEnumerable<Segment> Render(RenderContext context, int maxWidth)
        {
            var result = new List<Segment>();
            var doc = Markdig.Markdown.Parse(_markdownText);

            foreach (var element in doc)
            {
                result.AddRange(this.RenderBlock(element, context, maxWidth));
            }

            result.Add(Segment.LineBreak);

            return result;
        }

        private IEnumerable<Segment> RenderBlock(Block block, RenderContext context, int maxWidth)
        {
            switch (block)
            {
                case BlankLineBlock blankLineBlock:
                    break;
                case HeadingLinkReferenceDefinition headingLinkReferenceDefinition:
                    break;
                case CustomContainer customContainer:
                    break;
                case DefinitionItem definitionItem:
                    break;
                case DefinitionList definitionList:
                    break;
                case DefinitionTerm definitionTerm:
                    break;
                case Figure figure:
                    break;
                case FigureCaption figureCaption:
                    break;
                case FooterBlock footerBlock:
                    break;
                case Footnote footnote:
                    break;
                case FootnoteGroup footnoteGroup:
                    break;
                case FootnoteLinkReferenceDefinition footnoteLinkReferenceDefinition:
                    break;
                case MathBlock mathBlock:
                    break;
                case Markdig.Extensions.Tables.Table table:
                    break;
                case TableCell tableCell:
                    break;
                case Markdig.Extensions.Tables.TableRow tableRow:
                    break;
                case YamlFrontMatterBlock yamlFrontMatterBlock:
                    break;
                case Abbreviation abbreviation:
                    break;
                case FencedCodeBlock fencedCodeBlock:
                    break;
                case CodeBlock codeBlock:
                    break;
                case LinkReferenceDefinitionGroup linkReferenceDefinitionGroup:
                    break;
                case ListBlock listBlock:
                    return this.RenderListBlock(listBlock, context, maxWidth);
                case ListItemBlock listItemBlock:
                    return this.RenderListItemBlock(listItemBlock, context, maxWidth);
                case MarkdownDocument markdownDocument:
                    break;
                case QuoteBlock quoteBlock:
                    break;
                case ContainerBlock containerBlock:
                    break;
                case HeadingBlock headingBlock:
                    return this.RenderHeadingBlock(headingBlock, context, maxWidth);
                case HtmlBlock htmlBlock:
                    break;
                case LinkReferenceDefinition linkReferenceDefinition:
                    break;
                case ParagraphBlock paragraphBlock:
                    return this.RenderParagraphBlock(paragraphBlock, context, maxWidth);
                case ThematicBreakBlock thematicBreakBlock:
                    break;
                case LeafBlock leafBlock:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(block));
            }

            return Enumerable.Empty<Segment>();
        }

        private IEnumerable<Segment> RenderListItemBlock(ListItemBlock listItemBlock, RenderContext context,
            int maxWidth)
        {
            return listItemBlock.SelectMany(block => this.RenderBlock(block, context, maxWidth));
        }

        private IEnumerable<Segment> RenderListBlock(ListBlock listBlock, RenderContext context,
            int maxWidth)
        {
            var bulletChar = listBlock.BulletType;
            foreach (var item in listBlock)
            {
                yield return new Segment($" {bulletChar} ");

                foreach (var segment in this.RenderBlock(item, context, maxWidth))
                {
                    yield return segment;
                }
            }
        }

        private IEnumerable<Segment> RenderParagraphBlock(ParagraphBlock paragraphBlock, RenderContext context,
            int maxWidth)
        {
            var text = new Text(string.Join(Environment.NewLine, paragraphBlock.Inline));
            text.Alignment = Justify.Left;

            foreach (var segment in ((IRenderable)text).Render(context, maxWidth))
            {
                yield return segment;
            }

            yield return Segment.LineBreak;
        }

        private IEnumerable<Segment> RenderHeadingBlock(HeadingBlock headingBlock, RenderContext context, int maxWidth)
        {
            var inline = string.Join("\n", headingBlock.Inline);
            if (headingBlock.Level <= 2)
            {
                foreach (var segment in ((IRenderable)new Rule(inline)).Render(context, maxWidth))
                {
                    yield return segment;
                }

                yield break;
            }

            var text = new Text(inline);
            text.Alignment = Justify.Center;
            foreach (var segment in ((IRenderable)text).Render(context, maxWidth))
            {
                yield return segment;
            }

            yield return Segment.LineBreak;
        }
    }
}