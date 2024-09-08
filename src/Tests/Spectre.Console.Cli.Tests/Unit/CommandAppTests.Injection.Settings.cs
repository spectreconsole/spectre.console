#nullable enable

using Microsoft.Extensions.DependencyInjection;

namespace Spectre.Console.Tests.Unit.Cli;

public sealed partial class CommandAppTests
{
    public sealed partial class Injection
    {
        public sealed class Settings
        {
            public sealed class CustomInheritedCommand : Command<CustomInheritedCommandSettings>
            {
                private readonly SomeFakeDependency _dep;

                public CustomInheritedCommand(SomeFakeDependency dep)
                {
                    _dep = dep;
                }

                public override int Execute(CommandContext context, CustomInheritedCommandSettings settings, CancellationToken cancellationToken)
                {
                    return 0;
                }
            }

            public sealed class SomeFakeDependency
            {
            }

            public abstract class CustomBaseCommandSettings : CommandSettings
            {
            }

            public sealed class CustomInheritedCommandSettings : CustomBaseCommandSettings
            {
            }

            private sealed class CustomTypeRegistrar : ITypeRegistrar
            {
                private readonly IServiceCollection _services;

                public CustomTypeRegistrar(IServiceCollection services)
                {
                    _services = services;
                }

                public ITypeResolver Build()
                {
                    return new CustomTypeResolver(_services.BuildServiceProvider());
                }

                public void Register(Type service, Type implementation)
                {
                    _services.AddSingleton(service, implementation);
                }

                public void RegisterInstance(Type service, object implementation)
                {
                    _services.AddSingleton(service, implementation);
                }

                public void RegisterLazy(Type service, Func<object> func)
                {
                    _services.AddSingleton(service, provider => func());
                }
            }

            public sealed class CustomTypeResolver : ITypeResolver
            {
                private readonly IServiceProvider _provider;

                public CustomTypeResolver(IServiceProvider provider)
                {
                    _provider = provider ?? throw new ArgumentNullException(nameof(provider));
                }

                public object? Resolve(Type? type)
                {
                    ArgumentNullException.ThrowIfNull(type);
                    return _provider.GetRequiredService(type);
                }
            }

            [Fact]
            public void Should_Inject_Settings()
            {
                static CustomTypeRegistrar BootstrapServices()
                {
                    var services = new ServiceCollection();
                    services.AddSingleton<SomeFakeDependency, SomeFakeDependency>();
                    return new CustomTypeRegistrar(services);
                }

                // Given
                var app = new CommandAppTester(BootstrapServices());

                app.Configure(config =>
                {
                    config.PropagateExceptions();
                    config.AddBranch<CustomBaseCommandSettings>("foo", b =>
                    {
                        b.AddCommand<CustomInheritedCommand>("bar");
                    });
                });

                // When
                var result = app.Run("foo", "bar");

                // Then
                result.ExitCode.ShouldBe(0);
            }
        }
    }
}