using System.Collections.Generic;

namespace Docs.Models
{
    public sealed class ColorModel
    {
        public List<Color> Colors { get; set; }

        public ColorModel(IEnumerable<Color> colors)
        {
            Colors = new List<Color>(colors);
        }
    }
}