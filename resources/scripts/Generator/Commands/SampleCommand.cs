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
            public Settings(string outputPath, string sample, bool list)
            {
                Sample = sample;
                OutputPath = outputPath ?? Environment.CurrentDirectory;
                List = list;
            }

            [CommandArgument(0, "[sample]")]
            public string Sample { get; }

            [CommandOption("-o|--output")]
            public string OutputPath { get; }

            [CommandOption("-l|--list")]
            public bool List { get; }
        }

        private readonly IAnsiConsole _console;

        public SampleCommand(IAnsiConsole console)
        {
            _console = new AsciiCastConsole(console);
        }

        public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
        {
            var samples = typeof(BaseSample).Assembly
                .GetTypes()
                .Where(i => i.IsClass && i.IsAbstract == false && i.IsSubclassOf(typeof(BaseSample)))
                .Select(Activator.CreateInstance)
                .Cast<BaseSample>();

            var selectedSample = settings.Sample;
            if (settings.List)
            {
                selectedSample = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Select an example to record")
                        .PageSize(25)
                        .AddChoices(samples.Select(x => x.Name())));
            }

            if (!string.IsNullOrWhiteSpace(selectedSample))
            {
                var desiredSample = samples.FirstOrDefault(i => i.Name().Equals(selectedSample, StringComparison.OrdinalIgnoreCase));
                if (desiredSample == null)
                {
                    _console.MarkupLine($"[red]Error:[/] could not find sample [blue]{selectedSample}[/]");
                    return -1;
                }

                samples = new List<BaseSample> { desiredSample };
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