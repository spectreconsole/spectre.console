using System.Collections.Generic;

namespace Generator.Models
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
