namespace Spectre.Console.Cli;

public interface IStartupInterceptor
{
    void Intercept(StartupContext context);
}