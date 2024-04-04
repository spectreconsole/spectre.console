namespace Spectre.Console.Cli;

internal sealed class TypeResolverAdapter : ITypeResolver, IDisposable
{
    private readonly ITypeResolver? _resolver;

    public TypeResolverAdapter(ITypeResolver? resolver)
    {
        _resolver = resolver;
    }

#if NET6_0_OR_GREATER
    [UnconditionalSuppressMessage("AssemblyLoadTrimming", "IL2067", Justification = TrimWarnings.SuppressMessage)]
#endif
    public object? Resolve(Type? type)
    {
        if (type == null)
        {
            throw new CommandRuntimeException("Cannot resolve null type.");
        }

        try
        {
            var obj = _resolver?.Resolve(type);
            if (obj != null)
            {
                return obj;
            }

            // Fall back to use the activator.
            return Activator.CreateInstance(type);
        }
        catch (CommandAppException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw CommandRuntimeException.CouldNotResolveType(type, ex);
        }
    }

    public void Dispose()
    {
        if (_resolver is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }
}