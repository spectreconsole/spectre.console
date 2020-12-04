namespace Spectre.Console.Tests
{
    public sealed class TestLinkIdentityGenerator : ILinkIdentityGenerator
    {
        public int GenerateId(string link, string text)
        {
            return 1024;
        }
    }
}
