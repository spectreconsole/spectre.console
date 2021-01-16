using System;

namespace Spectre.Console.Cli
{
    /// <summary>
    /// Represents a pair deconstructor.
    /// </summary>
    internal interface IPairDeconstructor
    {
        /// <summary>
        /// Deconstructs the specified value into its components.
        /// </summary>
        /// <param name="resolver">The type resolver to use.</param>
        /// <param name="keyType">The key type.</param>
        /// <param name="valueType">The value type.</param>
        /// <param name="value">The value to deconstruct.</param>
        /// <returns>A deconstructed value.</returns>
        (object? Key, object? Value) Deconstruct(
            ITypeResolver resolver,
            Type keyType,
            Type valueType,
            string? value);
    }
}
