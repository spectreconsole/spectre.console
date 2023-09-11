using System.Threading;
using Spectre.Console;

namespace Generator.Commands.Samples
{
    public class ColumnsSample : BaseSample
    {
        public override void Run(IAnsiConsole console)
        {
            for (var i = 0; i <= 10; i++)
            {
                var n = 3 * i + 1;
                console.Write(new Columns(
                    new Text($"Item {n}", new Style(Color.Red, Color.Black)),
                    new Text($"Item {n+1}", new Style(Color.Green, Color.Black)),
                    new Text($"Item {n+2}", new Style(Color.Blue, Color.Black))
                ));
                Thread.Sleep(200);
            }
        }
    }
}