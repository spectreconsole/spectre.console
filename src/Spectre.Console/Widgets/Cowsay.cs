using System.Collections.Generic;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    /// <summary>
    /// Imitation of the default invocation of Tony Monroe's `cowsay` utility
    /// https://github.com/tnalpgge/rank-amateur-cowsay.
    /// </summary>
    public class Cowsay : Renderable
    {
        private const string CowBodyText =
@"        \   ^__^
         \  (oo)\_______
            (__)\       )\/\
                ||----w |
                ||     ||
";

        private readonly IRenderable _speechBubbleContents;

        /// <summary>
        /// Gets or sets style that will be adopted by the cow.
        /// </summary>
        public Style CowStyle { get; set; } = Style.Plain;

        /// <summary>
        /// Gets or sets style that will by adopted by the speech bubble.
        /// </summary>
        public Style SpeechBubbleStyle { get; set; } = Style.Plain;

        /// <summary>
        /// Gets or sets the border type to be used by the speech bubble.
        /// </summary>
        public BoxBorder SpeechBubbleBorder { get; set; } = BoxBorder.Rounded;

        /// <summary>
        /// Initializes a new instance of the <see cref="Cowsay"/> class.
        /// </summary>
        /// <param name="speechBubbleContents">Contents of the cow's speech bubble.</param>
        public Cowsay(IRenderable speechBubbleContents)
        {
            _speechBubbleContents = speechBubbleContents;
        }

        /// <inheritdoc />
        protected override IEnumerable<Segment> Render(RenderContext context, int maxWidth)
        {
            var result = new List<Segment>();
            var speechBubble =
                new Panel(_speechBubbleContents)
                {
                    Border = SpeechBubbleBorder,
                    BorderStyle = SpeechBubbleStyle,
                };

            result.AddRange(((IRenderable)speechBubble).Render(context, maxWidth));
            result.AddRange(((IRenderable)new Text(CowBodyText, CowStyle)).Render(context, maxWidth));

            return result;
        }
    }
}