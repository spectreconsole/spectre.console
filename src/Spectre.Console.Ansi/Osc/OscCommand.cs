namespace Spectre.Console.Ansi;

public record OscCommand
{
    public record HyperLinkStart(string? Id, string Url) : OscCommand;
    public record HyperLinkEnd : OscCommand;
}