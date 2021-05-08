using System.Linq;
using Shouldly;
using Spectre.Console.Cli;
using Spectre.Console.Testing;
using Spectre.Console.Tests.Data;
using Xunit;

namespace Spectre.Console.Tests.Unit.Cli
{
    public sealed partial class CommandAppTests
    {
        public sealed class ValueProviders
        {
            public sealed class ValueProviderSettings : CommandSettings
            {
                [CommandOption("-f|--foo <VALUE>")]
                [IntegerValueProvider(32)]
                public int Foo { get; set; }
            }

            public sealed class IntegerValueProvider : ParameterValueProviderAttribute
            {
                private readonly int _value;

                public IntegerValueProvider(int value)
                {
                    _value = value;
                }

                public override bool TryGetValue(CommandParameterContext context, out object result)
                {
                    if (context.Parameter.ParameterType == typeof(int))
                    {
                        if (context.Value == null)
                        {
                            result = _value;
                            return true;
                        }
                    }

                    result = null;
                    return false;
                }
            }

            [Fact]
            public void Should_Use_Provided_Value_If_No_Value_Was_Specified()
            {
                // Given
                var app = new CommandAppTester();
                app.SetDefaultCommand<GenericCommand<ValueProviderSettings>>();
                app.Configure(config =>
                {
                    config.PropagateExceptions();
                });

                // When
                var result = app.Run();

                // Then
                result.Settings.ShouldBeOfType<ValueProviderSettings>().And(settings =>
                {
                    settings.Foo.ShouldBe(32);
                });
            }

            [Fact]
            public void Should_Not_Override_Value_If_Value_Was_Specified()
            {
                // Given
                var app = new CommandAppTester();
                app.SetDefaultCommand<GenericCommand<ValueProviderSettings>>();
                app.Configure(config =>
                {
                    config.PropagateExceptions();
                });

                // When
                var result = app.Run("--foo", "12");

                // Then
                result.Settings.ShouldBeOfType<ValueProviderSettings>().And(settings =>
                {
                    settings.Foo.ShouldBe(12);
                });
            }
        }
    }
}
