using System;

namespace Spectre.Console.Cli
{
    internal sealed class TypeRegistrar : ITypeRegistrarFrontend
    {
        private readonly ITypeRegistrar _registrar;

        internal TypeRegistrar(ITypeRegistrar registrar)
        {
            _registrar = registrar ?? throw new ArgumentNullException(nameof(registrar));
        }

        public void Register<TService, TImplementation>()
            where TImplementation : TService
        {
            _registrar.Register(typeof(TService), typeof(TImplementation));
        }

        public void RegisterInstance<TImplementation>(TImplementation instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            _registrar.RegisterInstance(typeof(TImplementation), instance);
        }

        public void RegisterInstance<TService, TImplementation>(TImplementation instance)
            where TImplementation : TService
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            _registrar.RegisterInstance(typeof(TService), instance);
        }
    }
}
