using System.Collections;

namespace Spectre.Console.Testing;

/// <summary>
/// A fake type resolver suitable for testing.
/// </summary>
public sealed class FakeTypeResolver : ITypeResolver
{
    private readonly Dictionary<Type, List<Type>> _registrations;
    private readonly Dictionary<Type, List<object>> _instances;

    /// <summary>
    /// Initializes a new instance of the <see cref="FakeTypeResolver"/> class.
    /// </summary>
    /// <param name="registrations">The registrations.</param>
    /// <param name="instances">The singleton registrations.</param>
    public FakeTypeResolver(
        Dictionary<Type, List<Type>> registrations,
        Dictionary<Type, List<object>> instances)
    {
        _registrations = registrations ?? throw new ArgumentNullException(nameof(registrations));
        _instances = instances ?? throw new ArgumentNullException(nameof(instances));
    }

    /// <inheritdoc/>
    public object? Resolve(Type? type)
    {
        if (type == null)
        {
            return null;
        }

        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
        {
            // return all registrations
            type = type.GenericTypeArguments[0];
            var allRegistrations = Activator.CreateInstance(typeof(List<>).MakeGenericType(type));
            var castList = allRegistrations as IList;

            if (_instances.TryGetValue(type, out var listInstances))
            {
                listInstances.ForEach(i => castList!.Add(i));
            }

            if (_registrations.TryGetValue(type, out var listRegistrations))
            {
                listRegistrations
                    .Select<Type, object>(x => Activator.CreateInstance(x)!)
                    .ToList()
                    .ForEach(i => castList!.Add(i));
            }

            return allRegistrations;
        }

        if (_instances.TryGetValue(type, out var instances))
        {
            return instances.LastOrDefault();
        }

        if (_registrations.TryGetValue(type, out var registrations))
        {
            // The type might be an interface, but the registration should be a class.
            // So call CreateInstance on the first registration rather than the type.
            return registrations.Count == 0
                ? null
                : Activator.CreateInstance(registrations.Last());
        }

        return null;
    }
}
