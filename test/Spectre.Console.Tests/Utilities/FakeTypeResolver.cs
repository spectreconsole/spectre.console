using System;
using System.Collections.Generic;
using System.Linq;
using Spectre.Console.Cli;

namespace Spectre.Console.Testing
{
    public sealed class FakeTypeResolver : ITypeResolver
    {
        private readonly Dictionary<Type, List<Type>> _registrations;
        private readonly Dictionary<Type, List<object>> _instances;

        public FakeTypeResolver(
            Dictionary<Type, List<Type>> registrations,
            Dictionary<Type, List<object>> instances)
        {
            _registrations = registrations ?? throw new ArgumentNullException(nameof(registrations));
            _instances = instances ?? throw new ArgumentNullException(nameof(instances));
        }

        public static Func<Dictionary<Type, List<Type>>, Dictionary<Type, List<object>>, ITypeResolver> Factory =>
            (r, i) => new FakeTypeResolver(r, i);

        public object Resolve(Type type)
        {
            if (_instances.TryGetValue(type, out var instances))
            {
                return instances.FirstOrDefault();
            }

            if (_registrations.TryGetValue(type, out var registrations))
            {
                return registrations.Count == 0
                    ? null
                    : Activator.CreateInstance(type);
            }

            return null;
        }
    }
}