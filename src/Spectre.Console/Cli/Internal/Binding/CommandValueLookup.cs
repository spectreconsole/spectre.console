using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Spectre.Console.Cli
{
    internal sealed class CommandValueLookup : IEnumerable<(CommandParameter Parameter, object? Value)>
    {
        private readonly Dictionary<Guid, (CommandParameter Parameter, object? Value)> _lookup;

        public CommandValueLookup()
        {
            _lookup = new Dictionary<Guid, (CommandParameter, object?)>();
        }

        public IEnumerator<(CommandParameter Parameter, object? Value)> GetEnumerator()
        {
            foreach (var pair in _lookup)
            {
                yield return pair.Value;
            }
        }

        public bool HasParameterWithName(string? name)
        {
            if (name == null)
            {
                return false;
            }

            return _lookup.Values.Any(pair => pair.Parameter.PropertyName.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public bool TryGetParameterWithName(string? name, out (CommandParameter Parameter, object? Value) result)
        {
            if (HasParameterWithName(name))
            {
                result = _lookup.Values.FirstOrDefault(pair => pair.Parameter.PropertyName.Equals(name, StringComparison.OrdinalIgnoreCase));
                return true;
            }

            result = default;
            return false;
        }

        public object? GetValue(CommandParameter parameter)
        {
            _lookup.TryGetValue(parameter.Id, out var result);
            return result.Value;
        }

        public void SetValue(CommandParameter parameter, object? value)
        {
            _lookup[parameter.Id] = (parameter, value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
