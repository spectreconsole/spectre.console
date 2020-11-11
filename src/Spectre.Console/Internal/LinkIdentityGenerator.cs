using System;
using System.Runtime.CompilerServices;

namespace Spectre.Console.Internal
{
    internal sealed class LinkIdentityGenerator : ILinkIdentityGenerator
    {
        private readonly Random _random;

        public LinkIdentityGenerator()
        {
            _random = new Random(DateTime.Now.Millisecond);
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
