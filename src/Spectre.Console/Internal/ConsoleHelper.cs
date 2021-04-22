namespace Spectre.Console;

internal static class ConsoleHelper
{
    private static int InvokeTPut(string arg)
    {
        var startInfo = new ProcessStartInfo("tput", arg);
        startInfo.RedirectStandardOutput = true;
        startInfo.RedirectStandardError = true;
        var process = Process.Start(startInfo);
        if (process == null)
        {
            throw new Exception("Failed to start tput");
        }

        var output = new StringBuilder();
        var error = new StringBuilder();
        process.OutputDataReceived += (obj, data) => output.AppendLine(data.Data);
        process.ErrorDataReceived += (obj, data) => error.AppendLine(data.Data);
        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
        process.WaitForExit();
        if (process.ExitCode == 0)
        {
            return int.Parse(output.ToString());
        }

        throw new Exception(error.ToString());
    }

    public static int GetSafeWidth(int defaultValue = Constants.DefaultTerminalWidth)
    {
        var width = 0;
        try
        {
            width = System.Console.BufferWidth;
        }
        catch (IOException)
        {
        }

        if (width == 0)
        {
            try
            {
                width = InvokeTPut("cols");
            }
            catch
            {
                width = defaultValue;
            }
        }

        return width;
    }

    public static int GetSafeHeight(int defaultValue = Constants.DefaultTerminalHeight)
    {
        var height = 0;
        try
        {
            height = System.Console.WindowHeight;
        }
        catch (IOException)
        {
        }

        if (height == 0)
        {
            try
            {
                height = InvokeTPut("lines");
            }
            catch
            {
                height = defaultValue;
            }
        }

        return height;
    }
}