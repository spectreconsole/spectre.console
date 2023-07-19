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

        if (_instances.TryGetValue(type, out var instances))
        {
            return instances.FirstOrDefault();
        }

        if (_registrations.TryGetValue(type, out var registrations))
        {
            // The type might be an interface, but the registration should be a class.
            // So call CreateInstance on the first registration rather than the type.
            return registrations.Count == 0
                ? null
                : Activator.CreateInstance(registrations[0]);
        }

        return null;
    }
}
