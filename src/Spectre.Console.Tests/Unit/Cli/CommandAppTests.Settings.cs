using Shouldly;
using Spectre.Console.Cli;
using Xunit;

namespace Spectre.Console.Tests.Unit.Cli
{
    public sealed partial class CommandAppTests
    {
        [Fact]
        public void Should_Apply_Case_Sensitivity_For_Everything_By_Default()
        {
            // Given
            var app = new CommandApp();

            // When
            var defaultSensitivity = CaseSensitivity.None;
            app.Configure(config =>
            {
                defaultSensitivity = config.Settings.CaseSensitivity;
            });

            // Then
            defaultSensitivity.ShouldBe(CaseSensitivity.All);
        }
    }
}
