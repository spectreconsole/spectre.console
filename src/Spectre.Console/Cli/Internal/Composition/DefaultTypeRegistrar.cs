using System;
using System.Collections.Generic;

namespace Spectre.Console.Cli
{
    internal sealed class DefaultTypeRegistrar : ITypeRegistrar
    {
        private readonly Queue<Action<ComponentRegistry>> _registry;

        public DefaultTypeRegistrar()
        {
            _registry = new Queue<Action<ComponentRegistry>>();
        }

        public ITypeResolver Build()
        {
            var container = new DefaultTypeResolver();
            while (_registry.Count > 0)
            {
                var action = _registry.Dequeue();
                action(container.Registry);
            }

            return container;
        }

        public void Register(Type service, Type implementation)
        {
            var registration = new ComponentRegistration(implementation, new ReflectionActivator(implementation), new[] { service });
            _registry.Enqueue(registry => registry.Register(registration));
        }

        public void RegisterInstance(Type service, object implementation)
        {
            var registration = new ComponentRegistration(service, new CachingActivator(new InstanceActivator(implementation)));
            _registry.Enqueue(registry => registry.Register(registration));
        }

        public void RegisterLazy(Type service, Func<object> factory)
        {
            if (factory is null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            _registry.Enqueue(registry =>
            {
                var activator = new CachingActivator(new InstanceActivator(factory()));
                var registration = new ComponentRegistration(service, activator);

                registry.Register(registration);
            });
        }
    }
}
