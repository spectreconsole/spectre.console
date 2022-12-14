using Spectre.Console;
using Spectre.Console.Examples;

namespace Colors;

public static class Program
{
    public static void Main()
    {
        /////////////////////////////////////////////////////////////////
        // No colors
        /////////////////////////////////////////////////////////////////
        if (AnsiConsole.Profile.Capabilities.ColorSystem == ColorSystem.NoColors)
        {
            AnsiConsole.WriteLine("No colors are supported.");
            return;
        }

        /////////////////////////////////////////////////////////////////
        // 3-BIT
        /////////////////////////////////////////////////////////////////
        if (AnsiConsole.Profile.Supports(ColorSystem.Legacy))
        {
            AnsiConsole.ResetColors();
            AnsiConsole.WriteLine();
            AnsiConsole.Write(new Rule("[yellow bold underline]3-bit Colors[/]").RuleStyle("grey").LeftJustified());
            AnsiConsole.WriteLine();

            for (var i = 0; i < 8; i++)
            {
                AnsiConsole.Background = Color.FromInt32(i);
                AnsiConsole.Foreground = AnsiConsole.Background.GetInvertedColor();
                AnsiConsole.Write(string.Format(" {0,-9}", AnsiConsole.Background.ToString()));
                AnsiConsole.ResetColors();
                if ((i + 1) % 8 == 0)
                {
                    AnsiConsole.WriteLine();
                }
            }
        }

        /////////////////////////////////////////////////////////////////
        // 4-BIT
        /////////////////////////////////////////////////////////////////
        if (AnsiConsole.Profile.Supports(ColorSystem.Standard))
        {
            AnsiConsole.ResetColors();
            AnsiConsole.WriteLine();
            AnsiConsole.Write(new Rule("[yellow bold underline]4-bit Colors[/]").RuleStyle("grey").LeftJustified());
            AnsiConsole.WriteLine();

            for (var i = 0; i < 16; i++)
            {
                AnsiConsole.Background = Color.FromInt32(i);
                AnsiConsole.Foreground = AnsiConsole.Background.GetInvertedColor();
                AnsiConsole.Write(string.Format(" {0,-9}", AnsiConsole.Background.ToString()));
                AnsiConsole.ResetColors();
                if ((i + 1) % 8 == 0)
                {
                    AnsiConsole.WriteLine();
                }
            }
        }

        /////////////////////////////////////////////////////////////////
        // 8-BIT
        /////////////////////////////////////////////////////////////////
        if (AnsiConsole.Profile.Supports(ColorSystem.EightBit))
        {
            AnsiConsole.ResetColors();
            AnsiConsole.WriteLine();
            AnsiConsole.Write(new Rule("[yellow bold underline]8-bit Colors[/]").RuleStyle("grey").LeftJustified());
            AnsiConsole.WriteLine();

            for (var i = 0; i < 16; i++)
            {
                for (var j = 0; j < 16; j++)
                {
                    var number = i * 16 + j;
                    AnsiConsole.Background = Color.FromInt32(number);
                    AnsiConsole.Foreground = AnsiConsole.Background.GetInvertedColor();
                    AnsiConsole.Write(string.Format(" {0,-4}", number));
                    AnsiConsole.ResetColors();
                    if ((number + 1) % 16 == 0)
                    {
                        AnsiConsole.WriteLine();
                    }
                }
            }
        }

        /////////////////////////////////////////////////////////////////
        // 24-BIT
        /////////////////////////////////////////////////////////////////
        if (AnsiConsole.Profile.Supports(ColorSystem.TrueColor))
        {
            AnsiConsole.ResetColors();
            AnsiConsole.WriteLine();
            AnsiConsole.Write(new Rule("[yellow bold underline]24-bit Colors[/]").RuleStyle("grey").LeftJustified());
            AnsiConsole.WriteLine();

            AnsiConsole.Write(new ColorBox(width: 80, height: 15));
        }
    }
}
