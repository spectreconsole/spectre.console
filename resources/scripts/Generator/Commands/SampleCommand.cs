using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using Generator.Commands.Samples;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Generator.Commands
{
    internal class SampleCommand : Command<SampleCommand.Settings>
    {
        public class Settings : CommandSettings
        {
            public Settings(string outputPath, string sample)
            {
                Sample = sample;
                OutputPath = outputPath ?? Environment.CurrentDirectory;
            }

            [CommandArgument(0, "[sample]")]
            public string Sample { get; }

            [CommandOption("-o|--output")]
            public string OutputPath { get; }
        }

        private readonly IAnsiConsole _console;

        public SampleCommand(IAnsiConsole console)
        {
            this._console = console;
        }

        public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
        {
            _console.Prompt(new ConfirmationPrompt("Some commands will mimic user input. Make sure this window has focus and press y"));

            var samples = typeof(BaseSample).Assembly
                .GetTypes()
                .Where(i => i.IsClass && i.IsAbstract == false && i.IsSubclassOf(typeof(BaseSample)))
                .Select(Activator.CreateInstance)
                .Cast<BaseSample>();

            if (!string.IsNullOrWhiteSpace(settings.Sample))
            {
                var desiredSample = samples.FirstOrDefault(i => i.Name().Equals(settings.Sample));
                if (desiredSample == null)
                {
                    _console.MarkupLine($"[red]Error:[/] could not find sample [blue]{settings.Sample}[/]");
                    return -1;
                }

                samples = new List<BaseSample> { desiredSample};
            }

            // from here on out everything we write will be recorded.
            var recorder = _console.WrapWithAsciiCastRecorder();

            foreach (var sample in samples)
            {
                var sampleName = sample.Name();

                var originalWidth = _console.Profile.Width;
                var originalHeight = _console.Profile.Height;

                _console.Profile.Encoding = Encoding.UTF8;
                _console.Profile.Width = sample.ConsoleSize.Cols;
                _console.Profile.Height = sample.ConsoleSize.Rows;

                foreach (var (capabilityName, action) in sample.GetCapabilities())
                {
                    action(_console.Profile.Capabilities);
                    sample.Run(_console);
                    var json = recorder.GetCastJson($"{sampleName} ({capabilityName})", sample.ConsoleSize.Cols + 2, sample.ConsoleSize.Rows);
                    File.WriteAllText(Path.Combine(settings.OutputPath, $"{sampleName}-{capabilityName}.cast"), json);
                }

                _console.Profile.Width = originalWidth;
                _console.Profile.Height = originalHeight;
            }

            return 0;
        }
    }
}