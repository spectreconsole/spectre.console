using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Spectre.Console;

// ReSharper disable once CheckNamespace
namespace Generator.Commands.Samples;

// ReSharper disable once UnusedType.Global
public class ColumnsSample : BaseSample
{
    public override void Run(IAnsiConsole console)
    {
        var cards = Fruit
            .LoadFriuts()
            .Select(GetContent)
            .ToList();

        // Animate
        console.Live(new Text(""))
            .AutoClear(true)
            .Overflow(VerticalOverflow.Ellipsis)
            .Cropping(VerticalOverflowCropping.Top)
            .Start(ctx =>
            {
                for (var i = 1; i < cards.Count; i++)
                {
                    var toShow = cards.Take(i);
                    ctx.UpdateTarget(new Columns(toShow));
                    //ctx.Refresh();
                    Thread.Sleep(200);
                }
            });

        // Render all cards in columns
        AnsiConsole.Write(new Spectre.Console.Columns(cards));
    }

    private static string GetContent(Fruit fruit)
    {
        return $"[b][yellow]{fruit.Name}[/][/]";
    }

    private sealed class Fruit
    {
        public string Name { get; init; }

        public static List<Fruit> LoadFriuts()
        {
            return new []
            {
                "Apple",
                "Apricot",
                "Avocado",
                "Banana",
                "Blackberry",
                "Blueberry",
                "Boysenberry",
                "Breadfruit",
                "Cacao",
                "Cherry",
                "Cloudberry",
                "Coconut",
                "Dragonfruit",
                "Elderberry",
                "Grape",
                "Grapefruit",
                "Jackfruit",
                "Kiwifruit",
                "Lemon",
                "Lime",
                "Mango",
                "Melon",
                "Orange",
                "Blood orange",
                "Clementine",
                "Mandarine",
                "Tangerine",
                "Papaya",
                "Passionfruit",
                "Plum",
                "Pineapple",
                "Pomelo",
                "Raspberry",
                "Salmonberry",
                "Strawberry",
                "Ximenia",
                "Yuzu",
            }
                .Select(x => new Fruit{Name = x})
                .ToList();
        }
    }
}