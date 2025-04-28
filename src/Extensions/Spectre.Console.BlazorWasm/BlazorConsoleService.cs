using System;

namespace Spectre.Console.BlazorWasm
{
    public class BlazorConsoleService
    {
        private string _output = string.Empty;
        public string Output => _output;

        public event Action? OutputChanged;

        public void Clear()
        {
            _output = string.Empty;
            OutputChanged?.Invoke();
        }

        public void Write(string value)
        {
            _output += value;
            OutputChanged?.Invoke();
        }

        public void Set(string value)
        {
            _output = value;
            OutputChanged?.Invoke();
        }
    }
}
