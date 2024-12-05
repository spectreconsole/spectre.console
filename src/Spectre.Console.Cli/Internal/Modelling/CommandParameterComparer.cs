namespace Spectre.Console.Cli;

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

            return x.Property.Equals(y.Property);
        }

        public int GetHashCode(CommandParameter? obj)
        {
            return obj?.Property?.GetHashCode() ?? 0;
        }
    }
}