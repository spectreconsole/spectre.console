using System;
using System.Globalization;

namespace Spectre.Console.Cli
{
    /// <summary>
    /// Implementation of a flag with an optional value.
    /// </summary>
    /// <typeparam name="T">The flag's element type.</typeparam>
    public sealed class FlagValue<T> : IFlagValue
    {
        /// <summary>
        /// Gets or sets a value indicating whether or not the flag was set or not.
        /// </summary>
        public bool IsSet { get; set; }

#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        /// <summary>
        /// Gets or sets the flag's value.
        /// </summary>
        public T Value { get; set; }
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

        /// <inheritdoc/>
        Type IFlagValue.Type => typeof(T);

        /// <inheritdoc/>
        object? IFlagValue.Value
        {
            get => Value;
            set
            {
#pragma warning disable CS8601 // Possible null reference assignment.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                Value = (T)value;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning restore CS8601 // Possible null reference assignment.
            }
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            var flag = (IFlagValue)this;
            if (flag.Value != null)
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    "Set={0}, Value={1}",
                    IsSet,
                    flag.Value.ToString());
            }

            return string.Format(
                CultureInfo.InvariantCulture,
                "Set={0}", IsSet);
        }
    }
}
