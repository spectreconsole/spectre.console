using System;
using Spectre.Console;

namespace ColorExample
{
    public static class Program
    {
        public static void Main()
        {
            if (AnsiConsole.Capabilities.ColorSystem == ColorSystem.NoColors)
            {
                /////////////////////////////////////////////////////////////////
                // No colors
                /////////////////////////////////////////////////////////////////

                AnsiConsole.WriteLine("No colors are supported.");
                return;
            }

            if (AnsiConsole.Capabilities.Supports(ColorSystem.Legacy))
            {
                /////////////////////////////////////////////////////////////////
                // 3-BIT
                /////////////////////////////////////////////////////////////////

                AnsiConsole.ResetColors();
                AnsiConsole.WriteLine();
                AnsiConsole.Render(new Rule("[yellow bold underline]3-bit Colors[/]").SetStyle("grey").LeftAligned());
                AnsiConsole.WriteLine();

                for (var i = 0; i < 8; i++)
                {
                    AnsiConsole.Background = Color.FromInt32(i);
                    AnsiConsole.Write(string.Format(" {0,-9}", AnsiConsole.Background.ToString()));
                    AnsiConsole.ResetColors();
                    if ((i + 1) % 8 == 0)
                    {
                        AnsiConsole.WriteLine();
                    }
                }
            }

            if (AnsiConsole.Capabilities.Supports(ColorSystem.Standard))
            {
                /////////////////////////////////////////////////////////////////
                // 4-BIT
                /////////////////////////////////////////////////////////////////

                AnsiConsole.ResetColors();
                AnsiConsole.WriteLine();
                AnsiConsole.Render(new Rule("[yellow bold underline]4-bit Colors[/]").SetStyle("grey").LeftAligned());
                AnsiConsole.WriteLine();

                for (var i = 0; i < 16; i++)
                {
                    AnsiConsole.Background = Color.FromInt32(i);
                    AnsiConsole.Write(string.Format(" {0,-9}", AnsiConsole.Background.ToString()));
                    AnsiConsole.ResetColors();
                    if ((i + 1) % 8 == 0)
                    {
                        AnsiConsole.WriteLine();
                    }
                }
            }

            if (AnsiConsole.Capabilities.Supports(ColorSystem.EightBit))
            {
                /////////////////////////////////////////////////////////////////
                // 8-BIT
                /////////////////////////////////////////////////////////////////

                AnsiConsole.ResetColors();
                AnsiConsole.WriteLine();
                AnsiConsole.Render(new Rule("[yellow bold underline]8-bit Colors[/]").SetStyle("grey").LeftAligned());
                AnsiConsole.WriteLine();

                for (var i = 0; i < 16; i++)
                {
                    for (var j = 0; j < 16; j++)
                    {
                        var number = i * 16 + j;
                        AnsiConsole.Background = Color.FromInt32(number);
                        AnsiConsole.Write(string.Format(" {0,-4}", number));
                        AnsiConsole.ResetColors();
                        if ((number + 1) % 16 == 0)
                        {
                            AnsiConsole.WriteLine();
                        }
                    }
                }
            }

            if (AnsiConsole.Capabilities.Supports(ColorSystem.TrueColor))
            {
                /////////////////////////////////////////////////////////////////
                // 24-BIT
                /////////////////////////////////////////////////////////////////

                AnsiConsole.ResetColors();
                AnsiConsole.WriteLine();
                AnsiConsole.Render(new Rule("[yellow bold underline]24-bit Colors[/]").SetStyle("grey").LeftAligned());
                AnsiConsole.WriteLine();

                var index = 0;
                for (var i = 0.0005; i < 1; i += 0.0025)
                {
                    index++;

                    var color = Utilities.HSL2RGB(i, 0.5, 0.5);
                    AnsiConsole.Background = new Color(color.R, color.G, color.B);
                    AnsiConsole.Write(" ");

                    if (index % 50 == 0)
                    {
                        AnsiConsole.WriteLine();
                    }
                }
            }
        }
    }
}
