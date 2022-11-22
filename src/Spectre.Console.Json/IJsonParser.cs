namespace Spectre.Console.Json;

/// <summary>
/// Represents a JSON parser.
/// </summary>
public interface IJsonParser
{
    /// <summary>
    /// Parses the provided JSON into an abstract syntax tree.
    /// </summary>
    /// <param name="json">The JSON to parse.</param>
    /// <returns>An <see cref="JsonSyntax"/> instance.</returns>
    JsonSyntax Parse(string json);
}
