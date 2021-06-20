using Spectre.Console;

namespace AnalyzerTester
{
    class Program
    {
        static void Main(string[] args)
        {
            AnsiConsole.WriteLine("Hello World!");
        }
    }

    class Dependency
    {
        private readonly IAnsiConsole _ansiConsole;

        public Dependency(IAnsiConsole ansiConsole)
        {
            _ansiConsole = ansiConsole;
        }

        public void DoIt()
        {
            _ansiConsole.WriteLine("Hey mom!");
        }

        public void DoIt(IAnsiConsole thisConsole)
        {
            thisConsole.WriteLine("Hey mom!");
        }
    }
}
