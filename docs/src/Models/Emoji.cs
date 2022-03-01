using System.Collections.Generic;
using Newtonsoft.Json;

namespace Docs.Models
{
    public sealed class Emoji
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }

        public static List<Emoji> Parse(string json)
        {
            return JsonConvert.DeserializeObject<List<Emoji>>(json);
        }
    }
}
