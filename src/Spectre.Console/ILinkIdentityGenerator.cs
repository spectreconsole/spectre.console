namespace Spectre.Console
{
    /// <summary>
    /// Represents a link identity generator.
    /// </summary>
    public interface ILinkIdentityGenerator
    {
        /// <summary>
        /// Generates an ID for the given link.
        /// </summary>
        /// <param name="link">The link.</param>
        /// <param name="text">The link text.</param>
        /// <returns>A unique ID for the link.</returns>
        public int GenerateId(string link, string text);
    }
}
