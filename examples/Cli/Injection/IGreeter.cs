using System;

namespace Injection
{
    public interface IGreeter
    {
        void Greet(string name);
    }

    public sealed class HelloWorldGreeter : IGreeter
    {
        public void Greet(string name)
        {
            Console.WriteLine($"Hello {name}!");
        }
    }
}
