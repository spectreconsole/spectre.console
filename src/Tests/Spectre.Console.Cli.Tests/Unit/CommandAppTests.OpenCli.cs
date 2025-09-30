namespace Spectre.Console.Tests.Unit.Cli;

public sealed partial class CommandAppTests
{
    [ExpectationPath("OpenCli")]
    public sealed partial class OpenCli
    {
        [Fact]
        [Expectation("Generate")]
        public Task Should_Output_OpenCli_Description()
        {
            // Given
            var fixture = new CommandAppTester();
            fixture.Configure(config =>
            {
                config.SetApplicationName("my-app");
                config.SetApplicationVersion("1.2.3");

                config.AddBranch("animals", animals =>
                {
                    animals.AddCommand<DogCommand>("dog");
                    animals.AddCommand<CatCommand>("cat");
                });
            });

            // When
            var result = fixture.Run(Constants.OpenCliOption);

            // Then
            return Verifier.Verify(result.Output);
        }
    }
}
