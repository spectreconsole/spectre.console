using System;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace Spectre.Console.Examples;

public sealed class TypeResolver : ITypeResolver
{
    private readonly IServiceProvider _provider;

    public TypeResolver(IServiceProvider provider)
    {
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
    }

    public object Resolve(Type type)
    {
        if (type == null)
        {
            return null;
        }

        return _provider.GetService(type);
    }
}
