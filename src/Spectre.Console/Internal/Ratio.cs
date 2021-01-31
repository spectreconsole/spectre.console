// Ported from Rich by Will McGugan, licensed under MIT.
// https://github.com/willmcgugan/rich/blob/527475837ebbfc427530b3ee0d4d0741d2d0fc6d/rich/_ratio.py

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Spectre.Console
{
    internal static class Ratio
    {
        public static List<int> Reduce(int total, List<int> ratios, List<int> maximums, List<int> values)
        {
            ratios = ratios.Zip(maximums, (a, b) => (ratio: a, max: b)).Select(a => a.max > 0 ? a.ratio : 0).ToList();
            var totalRatio = ratios.Sum();
            if (totalRatio <= 0)
            {
                return values;
            }

            var totalRemaining = total;
            var result = new List<int>();

            foreach (var (ratio, maximum, value) in ratios.Zip(maximums, values))
            {
                if (ratio != 0 && totalRatio > 0)
                {
                    var distributed = (int)Math.Min(maximum, Math.Round((double)(ratio * totalRemaining / totalRatio)));
                    result.Add(value - distributed);
                    totalRemaining -= distributed;
                    totalRatio -= ratio;
                }
                else
                {
                    result.Add(value);
                }
            }

            return result;
        }

        public static List<int> Distribute(int total, IList<int> ratios, IList<int>? minimums = null)
        {
            if (minimums != null)
            {
                ratios = ratios.Zip(minimums, (a, b) => (ratio: a, min: b)).Select(a => a.min > 0 ? a.ratio : 0).ToList();
            }

            var totalRatio = ratios.Sum();
            Debug.Assert(totalRatio > 0, "Sum or ratios must be > 0");

            var totalRemaining = total;
            var distributedTotal = new List<int>();

            minimums ??= ratios.Select(_ => 0).ToList();

            foreach (var (ratio, minimum) in ratios.Zip(minimums, (a, b) => (a, b)))
            {
                var distributed = (totalRatio > 0)
                    ? Math.Max(minimum, (int)Math.Ceiling(ratio * totalRemaining / (double)totalRatio))
                    : totalRemaining;

                distributedTotal.Add(distributed);
                totalRatio -= ratio;
                totalRemaining -= distributed;
            }

            return distributedTotal;
        }
    }
}
