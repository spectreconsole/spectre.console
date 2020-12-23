using System;

namespace Spectre.Console.Cli
{
    /// <summary>
    /// Represents a type resolver.
    /// </summary>
    public interface ITypeResolver
    {
        /// <summary>
        /// Resolves an instance of the specified type.
        /// </summary>
        /// <param name="type">The type to resolve.</param>
        /// <returns>An instance of the specified type.</returns>
        object? Resolve(Type? type);
    }
}
