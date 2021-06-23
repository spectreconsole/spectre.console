using System;
using System.Collections.Generic;
using Spectre.Console.Cli;

namespace Spectre.Console.Testing
{
    public sealed class FakeTypeRegistrar : ITypeRegistrar
    {
        public Dictionary<Type, List<Type>> Registrations { get; }
        public Dictionary<Type, List<object>> Instances { get; }

        public FakeTypeRegistrar()
        {
            Registrations = new Dictionary<Type, List<Type>>();
            Instances = new Dictionary<Type, List<object>>();
        }

        public void Register(Type service, Type implementation)
        {
            if (!Registrations.ContainsKey(service))
            {
                Registrations.Add(service, new List<Type> { implementation });
            }
            else
            {
                Registrations[service].Add(implementation);
            }
        }

        public void RegisterInstance(Type service, object implementation)
        {
            if (!Instances.ContainsKey(service))
            {
                Instances.Add(service, new List<object> { implementation });
            }
        }

        public void RegisterLazy(Type service, Func<object> factory)
        {
            if (factory is null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            if (!Instances.ContainsKey(service))
            {
                Instances.Add(service, new List<object> { factory() });
            }
        }

        public ITypeResolver Build()
        {
            return null;
        }
    }
}
