namespace Spectre.Console.Cli;

internal sealed class DefaultTypeResolver : IDisposable, ITypeResolver
{
    public ComponentRegistry Registry { get; }

    public DefaultTypeResolver()
        : this(null)
    {
    }

    public DefaultTypeResolver(ComponentRegistry? registry)
    {
        Registry = registry ?? new ComponentRegistry();
    }

    public void Dispose()
    {
        Registry.Dispose();
    }

    public object? Resolve(Type? type)
    {
        if (type == null)
        {
            return null;
        }

        var isEnumerable = false;
        if (type.IsGenericType)
        {
            if (type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                isEnumerable = true;
                type = type.GenericTypeArguments[0];
            }
        }

        var registrations = Registry.GetRegistrations(type);
        if (registrations != null)
        {
            if (isEnumerable)
            {
                if (type.IsValueType)
                {
                    throw new InvalidOperationException($"The type {type.FullName} is a value type and not supported to be created as an array for dependency injection.");
                }

                var result = CreateInstanceHelpers.CreateArrayInstanceFromElementType(type, registrations.Count);
                for (var index = 0; index < registrations.Count; index++)
                {
                    var registration = registrations.ElementAt(index);
                    result.SetValue(Resolve(registration), index);
                }

                return result;
            }
        }

        return Resolve(registrations?.LastOrDefault());
    }

    public object? Resolve(ComponentRegistration? registration)
    {
        return registration?.Activator?.Activate(this);
    }
}