using System.ComponentModel;

namespace Spectre.Console.Cli
{
    internal enum ParameterKind
    {
        [Description("flag")]
        Flag = 0,

        [Description("scalar")]
        Scalar = 1,

        [Description("vector")]
        Vector = 2,

        [Description("flagvalue")]
        FlagWithValue = 3,

        [Description("pair")]
        Pair = 4,
    }
}