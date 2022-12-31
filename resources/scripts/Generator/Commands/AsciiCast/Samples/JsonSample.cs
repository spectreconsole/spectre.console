using Spectre.Console;
using Spectre.Console.Json;

namespace Generator.Commands.Samples
{
    public class JsonSample : BaseSample
    {
        public override (int Cols, int Rows) ConsoleSize => (60, 20);

        public override void Run(IAnsiConsole console)
        {
            var json = new JsonText(
                """
                { 
                    "hello": 32, 
                    "world": { 
                        "foo": 21, 
                        "bar": 255,
                        "baz": [
                            0.32, 0.33e-32,
                            0.42e32, 0.55e+32,
                            {
                                "hello": "world",
                                "lol": null
                            }
                        ]
                    } 
                }
                """);

            AnsiConsole.Write(
                new Panel(json)
                    .Header("Some JSON in a panel")
                    .Collapse()
                    .RoundedBorder()
                    .BorderColor(Color.Yellow));
        }
    }
}