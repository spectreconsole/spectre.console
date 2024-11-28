namespace Spectre.Console.Tests.Unit.Cli;

public sealed class DefaultTypeRegistrarTests
{
    [Fact]
    public void Should_Pass_Base_Registrar_Tests()
    {
        var harness = new TypeRegistrarBaseTests(() => new DefaultTypeRegistrar());
        harness.RunAllTests();
    }
}
