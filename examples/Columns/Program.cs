using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Spectre.Console;

namespace ColumnsExample
{
    public static class Program
    {
        public static async Task Main()
        {
            // Download some random users
            using var client = new HttpClient();
            dynamic users = JObject.Parse(
                await client.GetStringAsync("https://randomuser.me/api/?results=15"));

            // Create a card for each user
            var cards = new List<Panel>();
            foreach(var user in users.results)
            {
                cards.Add(new Panel(GetCard(user))
                    .SetHeader($"{user.location.country}")
                    .RoundedBorder().Expand());
            }

            // Render all cards in columns
            AnsiConsole.Render(new Columns(cards));
        }

        private static string GetCard(dynamic user)
        {
            var name = $"{user.name.first} {user.name.last}";
            var country = $"{user.location.city}";

            return $"[b]{name}[/]\n[yellow]{country}[/]";
        }
    }
}
