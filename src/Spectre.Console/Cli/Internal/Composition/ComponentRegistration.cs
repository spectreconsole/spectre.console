using System;
using System.Collections.Generic;

namespace Spectre.Console.Cli
{
    internal sealed class ComponentRegistration
    {
        public Type ImplementationType { get; }
        public ComponentActivator Activator { get; }
        public IReadOnlyList<Type> RegistrationTypes { get; }

        public ComponentRegistration(Type type, ComponentActivator activator, IEnumerable<Type>? registrationTypes = null)
        {
            var registrations = new List<Type>(registrationTypes ?? Array.Empty<Type>());
            if (registrations.Count == 0)
            {
                // Every registration needs at least one registration type.
                registrations.Add(type);
            }

            ImplementationType = type;
            RegistrationTypes = registrations;
            Activator = activator ?? throw new ArgumentNullException(nameof(activator));
        }

        public ComponentRegistration CreateCopy()
        {
            return new ComponentRegistration(ImplementationType, Activator.CreateCopy(), RegistrationTypes);
        }
    }
}