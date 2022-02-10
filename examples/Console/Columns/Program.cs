using System.Collections.Generic;
using Spectre.Console;

namespace Columns;

public static class Program
{
    public static void Main()
    {
        var cards = new List<Panel>();
        foreach (var user in User.LoadUsers())
        {
            cards.Add(
                new Panel(GetCardContent(user))
                    .Header($"{user.Country}")
                    .RoundedBorder().Expand());
        }

        // Render all cards in columns
        AnsiConsole.Write(new Spectre.Console.Columns(cards));
    }

    private static string GetCardContent(User user)
    {
        var name = $"{user.FirstName} {user.LastName}";
        var city = $"{user.City}";

        return $"[b]{name}[/]\n[yellow]{city}[/]";
    }
}
