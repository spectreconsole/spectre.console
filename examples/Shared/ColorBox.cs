using System;
using System.Collections.Generic;
using Spectre.Console.Rendering;

namespace Spectre.Console.Examples;

public sealed class ColorBox : Renderable
{
    private readonly int _height;
    private int? _width;

    public ColorBox(int height)
    {
        _height = height;
    }

    public ColorBox(int width, int height)
        : this(height)
    {
        _width = width;
    }

    protected override Measurement Measure(RenderOptions options, int maxWidth)
    {
        return new Measurement(1, GetWidth(maxWidth));
    }

    protected override IEnumerable<Segment> Render(RenderOptions options, int maxWidth)
    {
        maxWidth = GetWidth(maxWidth);

        for (var y = 0; y < _height; y++)
        {
            for (var x = 0; x < maxWidth; x++)
            {
                var h = x / (float)maxWidth;
                var l = 0.1f + ((y / (float)_height) * 0.7f);
                var (r1, g1, b1) = ColorFromHSL(h, l, 1.0f);
                var (r2, g2, b2) = ColorFromHSL(h, l + (0.7f / 10), 1.0f);

                var background = new Color((byte)(r1 * 255), (byte)(g1 * 255), (byte)(b1 * 255));
                var foreground = new Color((byte)(r2 * 255), (byte)(g2 * 255), (byte)(b2 * 255));

                yield return new Segment("▄", new Style(foreground, background));
            }

            yield return Segment.LineBreak;
        }
    }

    private int GetWidth(int maxWidth)
    {
        var width = maxWidth;
        if (_width != null)
        {
            width = Math.Min(_width.Value, width);
        }

        return width;
    }

    private static (float, float, float) ColorFromHSL(double h, double l, double s)
    {
        double r = 0, g = 0, b = 0;
        if (l != 0)
        {
            if (s == 0)
            {
                r = g = b = l;
            }
            else
            {
                double temp2;
                if (l < 0.5)
                {
                    temp2 = l * (1.0 + s);
                }
                else
                {
                    temp2 = l + s - (l * s);
                }

                var temp1 = 2.0 * l - temp2;

                r = GetColorComponent(temp1, temp2, h + 1.0 / 3.0);
                g = GetColorComponent(temp1, temp2, h);
                b = GetColorComponent(temp1, temp2, h - 1.0 / 3.0);
            }
        }

        return ((float)r, (float)g, (float)b);

    }

    private static double GetColorComponent(double temp1, double temp2, double temp3)
    {
        if (temp3 < 0.0)
        {
            temp3 += 1.0;
        }
        else if (temp3 > 1.0)
        {
            temp3 -= 1.0;
        }

        if (temp3 < 1.0 / 6.0)
        {
            return temp1 + (temp2 - temp1) * 6.0 * temp3;
        }
        else if (temp3 < 0.5)
        {
            return temp2;
        }
        else if (temp3 < 2.0 / 3.0)
        {
            return temp1 + ((temp2 - temp1) * ((2.0 / 3.0) - temp3) * 6.0);
        }
        else
        {
            return temp1;
        }
    }
}
