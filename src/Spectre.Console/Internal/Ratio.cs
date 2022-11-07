// Ported from Rich by Will McGugan, licensed under MIT.
// https://github.com/willmcgugan/rich/blob/527475837ebbfc427530b3ee0d4d0741d2d0fc6d/rich/_ratio.py

namespace Spectre.Console;

internal static class Ratio
{
    public static List<int> Resolve(int total, IEnumerable<IRatioResolvable> edges)
    {
        static (int Div, float Mod) DivMod(float x, float y)
        {
            return ((int)(x / y), x % y);
        }

        static int? GetEdgeWidth(IRatioResolvable edge)
        {
            if (edge.Size != null && edge.Size < edge.MinimumSize)
            {
                return edge.MinimumSize;
            }

            return edge.Size;
        }

        var sizes = edges.Select(x => GetEdgeWidth(x)).ToArray();

        while (sizes.Any(s => s == null))
        {
            // Get all edges and map them back to their index.
            // Ignore edges which have a explicit size.
            var flexibleEdges = sizes.Zip(edges, (a, b) => (Size: a, Edge: b))
                .Enumerate()
                .Select(x => (x.Index, x.Item.Size, x.Item.Edge))
                .Where(x => x.Size == null)
                .ToList();

            // Get the remaining space
            var remaining = total - sizes.Select(size => size ?? 0).Sum();
            if (remaining <= 0)
            {
                // No more room for flexible edges.
                return sizes
                    .Zip(edges, (size, edge) => (Size: size, Edge: edge))
                    .Select(zip => zip.Size ?? zip.Edge.MinimumSize)
                    .Select(size => size > 0 ? size : 1)
                    .ToList();
            }

            var portion = (float)remaining / flexibleEdges.Sum(x => Math.Max(1, x.Edge.Ratio));

            var invalidate = false;
            foreach (var (index, size, edge) in flexibleEdges)
            {
                if (portion * edge.Ratio <= edge.MinimumSize)
                {
                    sizes[index] = edge.MinimumSize;

                    // New fixed size will invalidate calculations,
                    // so we need to repeat the process
                    invalidate = true;
                    break;
                }
            }

            if (!invalidate)
            {
                var remainder = 0f;
                foreach (var flexibleEdge in flexibleEdges)
                {
                    var (div, mod) = DivMod((portion * flexibleEdge.Edge.Ratio) + remainder, 1);
                    remainder = mod;
                    sizes[flexibleEdge.Index] = div;
                }
            }
        }

        return sizes.Select(x => x ?? 1).ToList();
    }

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