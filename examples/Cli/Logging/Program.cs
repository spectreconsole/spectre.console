using Logging.Commands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Spectre.Console.Cli;
using Spectre.Console.Rendering;

namespace Logging
{
    public class Program
    {
        static int Main(string[] args)
        {
            // to retrieve the log file name, we must first parse the command settings
            // this will require us to delay setting the file path for the file writer.
            // With serilog we can use an enricher and Serilog.Sinks.Map to dynamically
            // pull this setting.
            var serviceCollection = new ServiceCollection()
                .AddLogging(configure =>
                    configure.AddSerilog(new LoggerConfiguration()
                        .MinimumLevel.Debug()
                        .Enrich.With<LogFileNameEnricher>()
                        .WriteTo.Map(LogFileNameEnricher.LogFilePathPropertyName,
                            (logFilePath, wt) => wt.File($"{logFilePath}"), 1)
                        .CreateLogger()
                    )
                );

            var registrar = new TypeRegistrar(serviceCollection);
            var app = new CommandApp(registrar);

            app.Configure(config =>
            {
                config.SetInterceptor(new LogInterceptor()); // add the interceptor
                config.AddCommand<HelloCommand>("hello");
            });

            return app.Run(args);
        }
    }
}