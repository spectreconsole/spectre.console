using System;
using System.Collections.Generic;
using System.Text;
using Spectre.Console;

namespace Generator.Commands.Samples
{
    public abstract class BaseSample
    {
        public abstract void Run(IAnsiConsole console);
        public virtual string Name() => PascalToKebab(GetType().Name.Replace("Sample",""));
        public virtual (int Cols, int Rows) ConsoleSize => (82, 24);
        public virtual IEnumerable<(string Name, Action<Capabilities> CapabilitiesAction)> GetCapabilities()
        {
            return new (string Name, Action<Capabilities> CapabilitiesAction)[]
            {
                ("plain", capabilities =>
                {
                    capabilities.Unicode = false;
                    capabilities.Ansi = true;
                    capabilities.Interactive = true;
                    capabilities.Legacy = false;
                    capabilities.Links = false;
                    capabilities.ColorSystem = ColorSystem.Legacy;
                }),
                ("rich", capabilities =>
                {
                    capabilities.Unicode = true;
                    capabilities.Ansi = true;
                    capabilities.Interactive = true;
                    capabilities.Legacy = false;
                    capabilities.Links = false;
                    capabilities.ColorSystem = ColorSystem.TrueColor;
                }),
            };
        }

        private string PascalToKebab(ReadOnlySpan<char> input)
        {
            var sb = new StringBuilder();
            var previousUpper = true;
            foreach (var chr in input)
            {
                if (char.IsUpper(chr) && previousUpper == false)
                {
                    sb.Append('-');
                    previousUpper = true;
                }
                else
                {
                    previousUpper = false;
                }

                sb.Append(char.ToLower(chr));
            }

            return sb.ToString();

        }
    }
}