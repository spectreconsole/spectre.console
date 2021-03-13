using System;
using System.Threading.Tasks;

namespace Spectre.Console.Testing
{
    public sealed class FakeExclusivityMode : IExclusivityMode
    {
        public T Run<T>(Func<T> func)
        {
            return func();
        }

        public async Task<T> Run<T>(Func<Task<T>> func)
        {
            return await func();
        }
    }
}
