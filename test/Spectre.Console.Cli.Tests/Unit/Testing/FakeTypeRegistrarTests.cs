namespace Spectre.Console.Tests.Unit.Cli.Testing;

public class FakeTypeRegistrarTests
{
    [Fact]
    public void TheFakeTypeRegistrarPassesAllTheTestsForARegistrar()
    {
        ITypeRegistrar Factory() => new FakeTypeRegistrar();
        var tester = new TypeRegistrarBaseTests(Factory);
        tester.RunAllTests();
    }
}