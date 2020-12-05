using System.Collections.Generic;
using System.Linq;
using Humanizer;
using Newtonsoft.Json;

namespace Generator.Models
{
    public sealed class Spinner
    {
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public int Interval { get; set; }
        public bool Unicode { get; set; }
        public List<string> Frames { get; set; }

        public static IEnumerable<Spinner> Parse(string json)
        {
            var data = JsonConvert.DeserializeObject<Dictionary<string, Spinner>>(json);
            foreach (var item in data)
            {
                item.Value.Name = item.Key;
                item.Value.NormalizedName = item.Value.Name.Pascalize();

                var frames = item.Value.Frames;
                item.Value.Frames = frames.Select(f => f.Replace("\\", "\\\\")).ToList();
            }

            return data.Values;
        }
    }
}
