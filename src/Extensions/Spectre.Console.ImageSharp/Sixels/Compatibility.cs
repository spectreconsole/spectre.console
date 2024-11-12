using Spectre.Console.ImageSharp.Sixels.Models;

namespace Spectre.Console.ImageSharp.Sixels;

/// <summary>
/// Sixel terminal compatibility helpers.
/// </summary>
public static class Compatibility
{
    /// <summary>
    /// Memory-caches the result of the terminal supporting sixel graphics.
    /// </summary>
    private static bool? _terminalSupportsSixel;

    /// <summary>
    /// Memory-caches the result of the terminal cell size, sending the control code is slow.
    /// </summary>
    private static CellSize? _cellSize;

    /// <summary>
    /// Get the cell size of the terminal in pixel-sixel size.
    /// The response to the command will look like [6;20;10t where the 20 is height and 10 is width.
    /// I think the 6 is the terminal class, which is not used here.
    /// </summary>
    /// <returns>The number of pixel sixels that will fit in a single character cell.</returns>
    public static CellSize GetCellSize()
    {
        if (_cellSize != null)
        {
            return _cellSize;
        }

        var response = GetControlSequenceResponse("[16t");

        try
        {
            var parts = response.Split(';', 't');
            _cellSize = new CellSize
            {
                PixelWidth = int.Parse(parts[2]),
                PixelHeight = int.Parse(parts[1]),
            };
        }
        catch
        {
            // Return the default Windows Terminal size if we can't get the size from the terminal.
            _cellSize = new CellSize
            {
                PixelWidth = 10,
                PixelHeight = 20,
            };
        }

        return _cellSize;
    }

    /// <summary>
    /// Check if the terminal supports sixel graphics.
    /// This is done by sending the terminal a Device Attributes request.
    /// If the terminal responds with a response that contains ";4;" then it supports sixel graphics.
    /// https://vt100.net/docs/vt510-rm/DA1.html.
    /// </summary>
    /// <returns>True if the terminal supports sixel graphics, false otherwise.</returns>
    public static bool TerminalSupportsSixel()
    {
        if (_terminalSupportsSixel.HasValue)
        {
            return _terminalSupportsSixel.Value;
        }

        _terminalSupportsSixel = GetControlSequenceResponse("[c").Contains(";4;");

        return _terminalSupportsSixel.Value;
    }

    /// <summary>
    /// Send a control sequence to the terminal and read back the response from STDIN.
    /// </summary>
    /// <param name="controlSequence">The control sequence to send to the terminal.</param>
    /// <returns>The response from the terminal.</returns>
    private static string GetControlSequenceResponse(string controlSequence)
    {
        char? c;
        var response = string.Empty;

        System.Console.Write(Constants.ESC + controlSequence);
        do
        {
            c = System.Console.ReadKey(true).KeyChar;
            response += c;
        }
        while (c != 'c' && System.Console.KeyAvailable);

        return response;
    }
}