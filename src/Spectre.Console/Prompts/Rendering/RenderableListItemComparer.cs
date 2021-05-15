using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Spectre.Console
{
    internal sealed class RenderableListItemComparer<T> : IEqualityComparer<RenderableListItem<T>>
        where T : notnull
    {
        public bool Equals([AllowNull] RenderableListItem<T> x, [AllowNull] RenderableListItem<T> y)
        {
            if (x == null && y == null)
            {
                return true;
            }
            else if (x == null || y == null)
            {
                return false;
            }
            else
            {
                return x.Index == y.Index;
            }
        }

        public int GetHashCode(RenderableListItem<T> obj)
        {
            return obj.Index.GetHashCode();
        }
    }
}
