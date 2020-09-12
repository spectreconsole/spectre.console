namespace Spectre.Console.Tests.Tools
{
    public sealed class TestLinkIdentityGenerator : ILinkIdentityGenerator
    {
        public int GenerateId(string link, string text)
        {
            return 1024;
        }
    }
}
