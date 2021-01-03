using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using Markdig;
using Markdig.Extensions.Abbreviations;
using Markdig.Extensions.AutoIdentifiers;
using Markdig.Extensions.CustomContainers;
using Markdig.Extensions.DefinitionLists;
using Markdig.Extensions.Emoji;
using Markdig.Extensions.Figures;
using Markdig.Extensions.Footers;
using Markdig.Extensions.Footnotes;
using Markdig.Extensions.JiraLinks;
using Markdig.Extensions.Mathematics;
using Markdig.Extensions.SmartyPants;
using Markdig.Extensions.Tables;
using Markdig.Extensions.TaskLists;
using Markdig.Extensions.Yaml;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using Spectre.Console.Internal;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    public class Markdown : Renderable
    {
        private readonly string _markdownText;
        private HttpClient _httpClient;

        public Markdown(string markdownText)
        {
            _markdownText = markdownText;
            _httpClient = new HttpClient();
        }

        protected override IEnumerable<Segment> Render(RenderContext context, int maxWidth)
        {
            var result = new List<Segment>();
            var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().UseEmojiAndSmiley().Build();
            var doc = Markdig.Markdown.Parse(_markdownText, pipeline);

            foreach (var element in doc)
            {
                result.AddRange(this.RenderBlock(element).Render(context, maxWidth));
            }

            result.Add(Segment.LineBreak);

            return result;
        }

        private IRenderable RenderBlock(Block block, Justify alignment = Justify.Left)
        {
            switch (block)
            {
                case BlankLineBlock:
                    return new Text(Environment.NewLine);
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

                // TODO: These features don't tend to be used in practice - feel free to add support!
                case MathBlock mathBlock:
                case Footnote footnote:
                case FootnoteGroup footnoteGroup:
                case FootnoteLinkReferenceDefinition footnoteLinkReferenceDefinition:
                case Figure figure:
                case FigureCaption figureCaption:
                case YamlFrontMatterBlock yamlFrontMatterBlock:
                case Abbreviation abbreviation:
                    break;

                case FooterBlock footerBlock:
                    break;
                case Markdig.Extensions.Tables.Table table:
                    return this.RenderTableBlock(table);
                case FencedCodeBlock fencedCodeBlock:
                    break;
                case CodeBlock codeBlock:
                    var blockContents = codeBlock.Lines.ToString();
                    return new Panel(blockContents) { Expand = true };
                case LinkReferenceDefinitionGroup linkReferenceDefinitionGroup:
                    break;
                case ListBlock listBlock:
                    return this.RenderListBlock(listBlock);
                case ListItemBlock listItemBlock:
                    return this.RenderListItemBlock(listItemBlock);
                case QuoteBlock quoteBlock:
                    var compositeRenderable =
                        new CompositeRenderable(quoteBlock.Select(x => this.RenderBlock(x)));
                    return new Panel(compositeRenderable) { Border = new LeftHandSideBoxBorder() };
                case HeadingBlock headingBlock:
                    return this.RenderHeadingBlock(headingBlock);
                case HtmlBlock htmlBlock:
                    break;
                case LinkReferenceDefinition linkReferenceDefinition:
                    break;
                case ParagraphBlock paragraphBlock:
                    return this.RenderParagraphBlock(paragraphBlock, alignment);
                case ThematicBreakBlock:
                    return new Rule { Style = new Style(decoration: Decoration.Bold), Border = BoxBorder.Double };
                default:
                    throw new ArgumentOutOfRangeException(nameof(block));
            }

            return Text.Empty;
        }

        private IRenderable RenderListItemBlock(ListItemBlock listItemBlock)
        {
            return new CompositeRenderable(listItemBlock.Select(x => this.RenderBlock(x)));
        }

        private IRenderable RenderTableBlock(Markdig.Extensions.Tables.Table table)
        {
            if (table.IsValid())
            {
                var renderedTable = new Table();

                // Safe to unconditionally cast to TableRow as IsValid() ensures this is the case under the hood
                foreach (var tableRow in table.Cast<Markdig.Extensions.Tables.TableRow>())
                {
                    if (tableRow.IsHeader)
                    {
                        AddColumnsToTable(tableRow, table.ColumnDefinitions, renderedTable);
                    }
                    else
                    {
                        AddRowToTable(tableRow, table.ColumnDefinitions, renderedTable);
                    }
                }

                return renderedTable;
            }

            return new Text("Invalid table structure", new Style(Color.Red));
        }

        private void AddColumnsToTable(Markdig.Extensions.Tables.TableRow tableRow, List<TableColumnDefinition> columnDefinitions, Table renderedTable)
        {
            // Safe to unconditionally cast to TableCell as IsValid() ensures this is the case under the hood
            foreach (var (cell, def) in tableRow.Cast<TableCell>().Zip(columnDefinitions))
            {
                renderedTable.AddColumn(new TableColumn(this.RenderTableCell(cell, def.Alignment)));
            }
        }

        private void AddRowToTable(
            Markdig.Extensions.Tables.TableRow tableRow,
            List<TableColumnDefinition> columnDefinitions,
            Table renderedTable)
        {
            var renderedRow = new List<IRenderable>();

            // Safe to unconditionally cast to TableCell as IsValid() ensures this is the case under the hood
            foreach (var (cell, def) in tableRow.Cast<TableCell>().Zip(columnDefinitions))
            {
                renderedRow.Add(this.RenderTableCell(cell, def.Alignment));
            }

            renderedTable.AddRow(renderedRow);
        }

        private IRenderable RenderTableCell(TableCell tableCell, TableColumnAlign? markdownAlignment)
        {
            var consoleAlignment = markdownAlignment switch
            {
                TableColumnAlign.Left => Justify.Left,
                TableColumnAlign.Center => Justify.Center,
                TableColumnAlign.Right => Justify.Right,
                null => Justify.Left,
                _ => throw new ArgumentOutOfRangeException(nameof(markdownAlignment), markdownAlignment,
                    "Unable to convert between Markdig alignment and Spectre.Console alignment"),
            };

            return new CompositeRenderable(tableCell.Select(x => this.RenderBlock(x, consoleAlignment)));
        }

        private IRenderable RenderListBlock(ListBlock listBlock)
        {
            IEnumerable<string>? itemPrefixes;
            if (listBlock.IsOrdered)
            {
                var startNum = int.Parse(listBlock.OrderedStart);
                var orderedDelimiter = listBlock.OrderedDelimiter;
                itemPrefixes = Enumerable.Range(startNum, listBlock.Count).Select(num => $"{num}{orderedDelimiter}");
            }
            else
            {
                itemPrefixes = Enumerable.Repeat(new string(listBlock.BulletType, 1), listBlock.Count);
            }

            var listDepthWhitespace = new string(' ', listBlock.GetListDepth());
            var paddedItemPrefixes = itemPrefixes.Select(x => new Text($" {listDepthWhitespace}{x} "));

            return new CompositeRenderable(Interleave(paddedItemPrefixes, listBlock.Select(x => this.RenderBlock(x))));
        }

        private static IEnumerable<T> Interleave<T>(IEnumerable<T> seqA, IEnumerable<T> seqB)
        {
            using var enumeratorA = seqA.GetEnumerator();
            using var enumeratorB = seqB.GetEnumerator();

            while (enumeratorA.MoveNext())
            {
                yield return enumeratorA.Current;

                if (enumeratorB.MoveNext())
                {
                    yield return enumeratorB.Current;
                }
            }

            while (enumeratorB.MoveNext())
            {
                yield return enumeratorB.Current;
            }
        }

        private IRenderable RenderParagraphBlock(ParagraphBlock paragraphBlock, Justify alignment)
        {
            var text = this.RenderContainerInline(paragraphBlock.Inline, alignment: alignment);

            return new CompositeRenderable(new List<IRenderable> { text, new Text(Environment.NewLine) });
        }

        private IRenderable RenderHeadingBlock(HeadingBlock headingBlock)
        {
            var inline = this.RenderContainerInline(headingBlock.Inline);

            return new Rule(inline);
        }

        private IRenderable RenderContainerInline(ContainerInline inline, Style? style = null, Justify alignment = Justify.Left)
        {
            return new CompositeRenderable(inline.Select(x => this.RenderInline(x, style ?? Style.Plain, alignment)));
        }

        private IRenderable RenderInline(Inline inline, Style style, Justify alignment)
        {
            switch (inline)
            {
                case FootnoteLink footnoteLink:
                    break;
                case CustomContainerInline customContainerInline:
                    break;
                case EmojiInline emojiInline:
                    return new Text(Emoji.Replace(emojiInline.Content.ToString()), style){ Alignment = alignment };
                case AbbreviationInline abbreviationInline:
                    break;

                // TODO: Again, not commonly used, feel free to add support
                case JiraLink jiraLink:
                case SmartyPant smartyPant:
                case TaskList taskList:
                case MathInline mathInline:
                    break;

                case PipeTableDelimiterInline pipeTableDelimiterInline:
                    break;
                case AutolinkInline autolinkInline:
                    break;
                case CodeInline codeInline:
                    break;
                case EmphasisDelimiterInline emphasisDelimiterInline:
                    break;
                case LinkDelimiterInline linkDelimiterInline:
                    break;
                case EmphasisInline emphasisInline:
                    var styleDecoration =
                        emphasisInline.DelimiterChar == '~'
                            ? Decoration.Strikethrough
                            : emphasisInline.DelimiterCount switch
                            {
                                1 => Decoration.Italic,
                                2 => Decoration.Bold,
                                _ => Decoration.None,
                            };
                    var emphasisChildStyle = new Style(decoration: styleDecoration);
                    return this.RenderContainerInline(emphasisInline, emphasisChildStyle);
                case LinkInline linkInline:
                    if (linkInline.IsImage)
                    {
                        if (this.TryGetCanvasImageForUrl(linkInline.Url, out var canvasImage))
                        {
                            return canvasImage;
                        }

                        return Text.Empty;
                    }

                    var linkInlineChildStyle = new Style(link: linkInline.Url);
                    return this.RenderContainerInline(linkInline, linkInlineChildStyle);
                case ContainerInline containerInline:
                    break;
                case HtmlEntityInline htmlEntityInline:
                    break;
                case HtmlInline htmlInline:
                    break;
                case LineBreakInline lineBreakInline:
                    break;
                case LiteralInline literalInline:
                    return new Text(literalInline.Content.ToString(), style){ Alignment = alignment };
                default:
                    throw new ArgumentOutOfRangeException(nameof(inline));
            }

            return Text.Empty;
        }

        private bool TryGetCanvasImageForUrl(string url, out CanvasImage canvasImage)
        {
            // TODO: Refactor this for easier unit testing - could do an initial "get image" pass to parallelise the IO too?
            try
            {
                var imageStream = _httpClient.GetStreamAsync(url).Result;
                using var memoryStream = new MemoryStream();
                imageStream.CopyTo(memoryStream);

                // TODO: Make the canvas size a little more dynamic
                canvasImage = new CanvasImage(memoryStream.ToArray());
                return true;
            }
            catch (Exception)
            {
                canvasImage = null!;
                return false;
            }
        }

        private class CompositeRenderable : Renderable
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
}