namespace Spectre.Console.Cli
{
    /// <summary>
    /// Representation of a multi map.
    /// </summary>
    internal interface IMultiMap
    {
        /// <summary>
        /// Adds a key and a value to the multi map.
        /// </summary>
        /// <param name="pair">The pair to add.</param>
        void Add((object? Key, object? Value) pair);
    }
}
