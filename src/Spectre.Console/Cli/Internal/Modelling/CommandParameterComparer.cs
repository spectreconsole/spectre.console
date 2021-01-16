using System.Collections.Generic;

namespace Spectre.Console.Cli
{
    internal static class CommandParameterComparer
    {
        public static readonly ByBackingPropertyComparer ByBackingProperty = new ByBackingPropertyComparer();

        public sealed class ByBackingPropertyComparer : IEqualityComparer<CommandParameter?>
        {
            public bool Equals(CommandParameter? x, CommandParameter? y)
            {
                if (x is null || y is null)
                {
                    return false;
                }

                if (ReferenceEquals(x, y))
                {
                    return true;
                }

                return x.Property.MetadataToken == y.Property.MetadataToken;
            }

            public int GetHashCode(CommandParameter? obj)
            {
                return obj?.Property?.MetadataToken.GetHashCode() ?? 0;
            }
        }
    }
}
