using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace DemoAot.Utilities;

public class DependencyInjectionRegistrar(IServiceCollection services) : ITypeRegistrar, IDisposable
{
    private IServiceCollection Services { get; } = services;
    private List<IDisposable> BuiltProviders { get; } = [];

    public ITypeResolver Build()
    {
        var buildServiceProvider = Services.BuildServiceProvider();
        BuiltProviders.Add(buildServiceProvider);
        return new DependencyInjectionResolver(buildServiceProvider);
    }

    public void Register(Type service, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type implementation)
    {
        Services.AddSingleton(service, implementation);
    }

    public void RegisterInstance(Type service, object implementation)
    {
        Services.AddSingleton(service, implementation);
    }

    public void RegisterLazy(Type service, Func<object> factory)
    {
        Services.AddSingleton(service, _ => factory());
    }

    public void Dispose()
    {
        foreach (var provider in BuiltProviders)
        {
            provider.Dispose();
        }
    }
}

internal class DependencyInjectionResolver(ServiceProvider serviceProvider) : ITypeResolver, IDisposable
{
    public void Dispose() => serviceProvider.Dispose();

    public object Resolve(Type type)
    {
        return serviceProvider.GetService(type);
    }
}