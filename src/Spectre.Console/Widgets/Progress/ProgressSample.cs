using System;

namespace Spectre.Console
{
    internal readonly struct ProgressSample
    {
        public double Value { get; }
        public DateTime Timestamp { get; }

        public ProgressSample(DateTime timestamp, double value)
        {
            Timestamp = timestamp;
            Value = value;
        }
    }
}
