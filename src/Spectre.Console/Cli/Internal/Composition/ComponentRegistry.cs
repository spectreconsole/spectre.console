using System;
using System.Collections.Generic;
using System.Linq;

namespace Spectre.Console.Cli
{
    internal sealed class ComponentRegistry : IDisposable
    {
        private readonly Dictionary<Type, HashSet<ComponentRegistration>> _registrations;

        public ComponentRegistry()
        {
            _registrations = new Dictionary<Type, HashSet<ComponentRegistration>>();
        }

        public ComponentRegistry CreateCopy()
        {
            var registry = new ComponentRegistry();
            foreach (var registration in _registrations.SelectMany(p => p.Value))
            {
                registry.Register(registration.CreateCopy());
            }

            return registry;
        }

        public void Dispose()
        {
            foreach (var registration in _registrations)
            {
                registration.Value.Clear();
            }

            _registrations.Clear();
        }

        public void Register(ComponentRegistration registration)
        {
            foreach (var type in new HashSet<Type>(registration.RegistrationTypes))
            {
                if (!_registrations.ContainsKey(type))
                {
                    _registrations.Add(type, new HashSet<ComponentRegistration>());
                }

                _registrations[type].Add(registration);
            }
        }

        public ICollection<ComponentRegistration> GetRegistrations(Type type)
        {
            if (_registrations.ContainsKey(type))
            {
                return _registrations[type];
            }

            return new List<ComponentRegistration>();
        }
    }
}