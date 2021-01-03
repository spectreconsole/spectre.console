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
using Spectre.Console.Internal;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    internal class MarkdownBlockRendering
    {
        private readonly MarkdownInlineRendering _inlineRendering;

        public MarkdownBlockRendering(MarkdownInlineRendering inlineRendering)
        {
            _inlineRendering = inlineRendering;
        }

        public IRenderable RenderBlock(Block block, Justify alignment = Justify.Left)
        {
            switch (block)
            {
                // TODO: These features are less adopted in practice and the MarkdownPipline isn't configured to generate them - feel free to add support!
                case CustomContainer customContainer:
                case MathBlock mathBlock:
                case Footnote footnote:
                case FootnoteGroup footnoteGroup:
                case FootnoteLinkReferenceDefinition footnoteLinkReferenceDefinition:
                case Figure figure:
                case FigureCaption figureCaption:
                case YamlFrontMatterBlock yamlFrontMatterBlock:
                case Abbreviation abbreviation:
                case FooterBlock footerBlock:
                case HtmlBlock htmlBlock:
                case DefinitionItem definitionItem:
                case DefinitionList definitionList:
                case DefinitionTerm definitionTerm:
                    break;

                case BlankLineBlock:
                    return new Text(Environment.NewLine);
                case HeadingLinkReferenceDefinition headingLinkReferenceDefinition:
                    break;
                case LinkReferenceDefinition linkReferenceDefinition:
                    break;
                case Markdig.Extensions.Tables.Table table:
                    return RenderTableBlock(table);
                case FencedCodeBlock fencedCodeBlock:
                    var fencedCodeBlockContents = fencedCodeBlock.Lines.ToString();
                    var header = fencedCodeBlock.Info == null ? null : new PanelHeader(fencedCodeBlock.Info);
                    return new Panel(fencedCodeBlockContents) {Header = header, Expand = true};
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
            var text = _inlineRendering.RenderContainerInline(paragraphBlock.Inline, alignment: alignment);

            return new CompositeRenderable(new List<IRenderable> { text, new Text(Environment.NewLine) });
        }

        private IRenderable RenderHeadingBlock(HeadingBlock headingBlock)
        {
            var inline = _inlineRendering.RenderContainerInline(headingBlock.Inline);

            return new Rule(inline);
        }
    }
}