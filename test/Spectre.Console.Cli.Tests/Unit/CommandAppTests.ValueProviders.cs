namespace Spectre.Console.Tests.Unit.Cli;

public sealed partial class CommandAppTests
{
    public sealed class ValueProviders
    {
        public sealed class ValueProviderSettings : CommandSettings
        {
            [CommandOption("-f|--foo <VALUE>")]
            [IntegerValueProvider(32)]
            [TypeConverter(typeof(HexConverter))]
            public string Foo { get; set; }
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
                if (context.Value == null)
                {
                    result = _value;
                    return true;
                }

                result = null;
                return false;
            }
        }

        public sealed class HexConverter : TypeConverter
        {
            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                if (value is int integer)
                {
                    return integer.ToString("X");
                }

                return value is string stringValue && int.TryParse(stringValue, out var intValue)
                    ? intValue.ToString("X")
                    : base.ConvertFrom(context, culture, value);
            }
        }

        [Fact]
        public void Should_Use_Provided_Value_If_No_Value_Was_Specified()
        {
            // Given
            var app = new CommandAppTester();
            app.SetDefaultCommand<GenericCommand<ValueProviderSettings>>();
            app.Configure(config => config.PropagateExceptions());

            // When
            var result = app.Run();

            // Then
            result.Settings.ShouldBeOfType<ValueProviderSettings>().And(settings =>
            {
                settings.Foo.ShouldBe("20"); // 32 is 0x20
                });
        }

        [Fact]
        public void Should_Not_Override_Value_If_Value_Was_Specified()
        {
            // Given
            var app = new CommandAppTester();
            app.SetDefaultCommand<GenericCommand<ValueProviderSettings>>();
            app.Configure(config => config.PropagateExceptions());

            // When
            var result = app.Run("--foo", "12");

            // Then
            result.Settings.ShouldBeOfType<ValueProviderSettings>().And(settings =>
            {
                settings.Foo.ShouldBe("C"); // 12 is 0xC
                });
        }
    }
}
