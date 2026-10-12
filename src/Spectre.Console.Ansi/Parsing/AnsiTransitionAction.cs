namespace Spectre.Console.Ansi;

internal enum AnsiTransitionAction
{
    None = 0,
    Ignore,
    Print,
    Execute,
    Collect,
    Param,
    EscDispatch,
    CsiDispatch,
    OscPut,
    DscPut,
}