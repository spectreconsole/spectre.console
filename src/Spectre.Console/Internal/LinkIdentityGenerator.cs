using System;

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
                    link.GetHashCode() +
                    _random.Next(0, int.MaxValue));
            }
        }
    }
}
