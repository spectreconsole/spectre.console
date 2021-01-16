using System;

namespace Spectre.Console.Cli
{
    /// <summary>
    /// Base class for a pair deconstructor.
    /// </summary>
    /// <typeparam name="TKey">The key type.</typeparam>
    /// <typeparam name="TValue">The value type.</typeparam>
    public abstract class PairDeconstuctor<TKey, TValue> : IPairDeconstructor
    {
        /// <summary>
        /// Deconstructs the provided <see cref="string"/> into a pair.
        /// </summary>
        /// <param name="value">The string to deconstruct into a pair.</param>
        /// <returns>The deconstructed pair.</returns>
        protected abstract (TKey Key, TValue Value) Deconstruct(string? value);

        /// <inheritdoc/>
        (object? Key, object? Value) IPairDeconstructor.Deconstruct(ITypeResolver resolver, Type keyType, Type valueType, string? value)
        {
            if (!keyType.IsAssignableFrom(typeof(TKey)) || !valueType.IsAssignableFrom(typeof(TValue)))
            {
                throw new InvalidOperationException("Pair destructor is not compatible.");
            }

            return Deconstruct(value);
        }
    }
}
