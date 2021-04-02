using System;
using System.Runtime.CompilerServices;

namespace Spectre.Console
{
    internal sealed class AnsiLinkHasher
    {
        private readonly Random _random;

        public AnsiLinkHasher()
        {
            _random = new Random(Environment.TickCount);
        }

        public int GenerateId(string link, string text)
        {
            if (link is null)
            {
                throw new ArgumentNullException(nameof(link));
            }

            link += text ?? string.Empty;

            unchecked
            {
                return Math.Abs(
                    GetLinkHashCode(link) +
                    _random.Next(0, int.MaxValue));
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int GetLinkHashCode(string link)
        {
#if NET5_0
            return link.GetHashCode(StringComparison.Ordinal);
#else
            return link.GetHashCode();
#endif
        }
    }
}
