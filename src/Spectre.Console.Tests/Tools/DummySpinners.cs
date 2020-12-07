using System;
using System.Collections.Generic;

namespace Spectre.Console.Tests
{
    public sealed class DummySpinner1 : Spinner
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(100);
        public override bool IsUnicode => true;
        public override IReadOnlyList<string> Frames => new List<string>
            {
                    "*",
            };
    }

    public sealed class DummySpinner2 : Spinner
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(100);
        public override bool IsUnicode => true;
        public override IReadOnlyList<string> Frames => new List<string>
            {
                    "-",
            };
    }
}
