using System;

namespace Spectre.Console.Cli
{
    /// <summary>
    /// Specifies what type to use as a pair deconstructor for
    /// the property this attribute is bound to.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class PairDeconstructorAttribute : Attribute
    {
        /// <summary>
        /// Gets the <see cref="string"/> that represents the type of the
        /// pair deconstructor class to use for data conversion for the
        /// object this attribute is bound to.
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PairDeconstructorAttribute"/> class.
        /// </summary>
        /// <param name="type">
        ///     A System.Type that represents the type of the pair deconstructor
        ///     class to use for data conversion for the object this attribute is bound to.
        /// </param>
        public PairDeconstructorAttribute(Type type)
        {
            Type = type ?? throw new ArgumentNullException(nameof(type));
        }
    }
}
