namespace Spectre.Console.Testing
{
    public sealed class FakeLinkIdentityGenerator : ILinkIdentityGenerator
    {
        private readonly int _linkId;

        public FakeLinkIdentityGenerator(int linkId)
        {
            _linkId = linkId;
        }

        public int GenerateId(string link, string text)
        {
            return _linkId;
        }
    }
}
