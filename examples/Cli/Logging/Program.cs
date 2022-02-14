using Logging.Commands;
using Logging.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Spectre.Console.Cli;

/*
 * Dynamically control serilog configuration via command line parameters
 *
 * This works around the chicken and egg situation with configuring serilog via the command line.
 * The logger needs to be configured prior to executing the parser, but the logger needs the parsed values
 * to be configured. By using serilog.sinks.map we can defer configuration. We use a LogLevelSwitch to control the
 * logging levels dynamically, and then we use a serilog enricher that has its state populated via a
 * Spectre.Console CommandInterceptor
 */

namespace Logging;

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
                    // log level will be dynamically be controlled by our log interceptor upon running
                    .MinimumLevel.ControlledBy(LogInterceptor.LogLevel)
                    // the log enricher will add a new property with the log file path from the settings
                    // that we can use to set the path dynamically
                    .Enrich.With<LoggingEnricher>()
                    // serilog.sinks.map will defer the configuration of the sink to be ondemand
                    // allowing us to look at the properties set by the enricher to set the path appropriately
                    .WriteTo.Map(LoggingEnricher.LogFilePathPropertyName,
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
