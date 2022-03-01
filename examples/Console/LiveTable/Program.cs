using System;
using System.Linq;
using System.Threading.Tasks;
using Spectre.Console;

namespace LiveTable;

public static class Program
{
    private const int NumberOfRows = 10;

    private static readonly Random _random = new();
    private static readonly string[] _exchanges = new string[]
    {
            "SGD", "SEK", "PLN",
            "MYR", "EUR", "USD",
            "AUD", "JPY", "CNH",
            "HKD", "CAD", "INR",
            "DKK", "GBP", "RUB",
            "NZD", "MXN", "IDR",
            "TWD", "THB", "VND",
    };

    public static async Task Main(string[] args)
    {
        var table = new Table().Expand().BorderColor(Color.Grey);
        table.AddColumn("[yellow]Source currency[/]");
        table.AddColumn("[yellow]Destination currency[/]");
        table.AddColumn("[yellow]Exchange rate[/]");

        AnsiConsole.MarkupLine("Press [yellow]CTRL+C[/] to exit");

        await AnsiConsole.Live(table)
            .AutoClear(false)
            .Overflow(VerticalOverflow.Ellipsis)
            .Cropping(VerticalOverflowCropping.Bottom)
            .StartAsync(async ctx =>
            {
                    // Add some initial rows
                    foreach (var _ in Enumerable.Range(0, NumberOfRows))
                {
                    AddExchangeRateRow(table);
                }

                    // Continously update the table
                    while (true)
                {
                        // More rows than we want?
                        if (table.Rows.Count > NumberOfRows)
                    {
                            // Remove the first one
                            table.Rows.RemoveAt(0);
                    }

                        // Add a new row
                        AddExchangeRateRow(table);

                        // Refresh and wait for a while
                        ctx.Refresh();
                    await Task.Delay(400);
                }
            });
    }

    private static void AddExchangeRateRow(Table table)
    {
        var (source, destination, rate) = GetExchangeRate();
        table.AddRow(
            source, destination,
            _random.NextDouble() > 0.35D ? $"[green]{rate}[/]" : $"[red]{rate}[/]");
    }

    private static (string Source, string Destination, double Rate) GetExchangeRate()
    {
        var source = _exchanges[_random.Next(0, _exchanges.Length)];
        var dest = _exchanges[_random.Next(0, _exchanges.Length)];
        var rate = 200 / ((_random.NextDouble() * 320) + 1);

        while (source == dest)
        {
            dest = _exchanges[_random.Next(0, _exchanges.Length)];
        }

        return (source, dest, rate);
    }
}
