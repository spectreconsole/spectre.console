using System;

namespace Spectre.Console.Cli
{
    /// <summary>
    /// Represents a type registrar.
    /// </summary>
    public interface ITypeRegistrar
    {
        /// <summary>
        /// Registers the specified service.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="implementation">The implementation.</param>
        void Register(Type service, Type implementation);

        /// <summary>
        /// Registers the specified instance.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="implementation">The implementation.</param>
        void RegisterInstance(Type service, object implementation);

        /// <summary>
        /// Registers the specified instance lazily.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="factory">The factory that creates the implementation.</param>
        void RegisterLazy(Type service, Func<object> factory);

        /// <summary>
        /// Builds the type resolver representing the registrations
        /// specified in the current instance.
        /// </summary>
        /// <returns>A type resolver.</returns>
        ITypeResolver Build();
    }
}
