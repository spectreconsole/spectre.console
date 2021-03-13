using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Spectre.Console.Cli
{
    [SuppressMessage("Performance", "CA1812: Avoid uninstantiated internal classes")]
    internal sealed class MultiMap<TKey, TValue> : IMultiMap, ILookup<TKey, TValue>, IDictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue>
        where TKey : notnull
    {
        private readonly IDictionary<TKey, MultiMapGrouping> _lookup;
        private readonly IDictionary<TKey, TValue> _dictionary;

        public int Count => _lookup.Count;

        public bool IsReadOnly => false;

        public ICollection<TKey> Keys => _lookup.Keys;

        public ICollection<TValue> Values => _dictionary.Values;

        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => _lookup.Keys;

        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => _dictionary.Values;

        TValue IReadOnlyDictionary<TKey, TValue>.this[TKey key] => _dictionary[key];

        TValue IDictionary<TKey, TValue>.this[TKey key]
        {
            get
            {
                return _dictionary[key];
            }
            set
            {
                Add(key, value);
            }
        }

        public IEnumerable<TValue> this[TKey key]
        {
            get
            {
                if (_lookup.TryGetValue(key, out var group))
                {
                    return group;
                }

                return Array.Empty<TValue>();
            }
        }

        public MultiMap()
        {
            _lookup = new Dictionary<TKey, MultiMapGrouping>();
            _dictionary = new Dictionary<TKey, TValue>();
        }

        private sealed class MultiMapGrouping : IGrouping<TKey, TValue>
        {
            private readonly List<TValue> _items;

            public TKey Key { get; }

            public MultiMapGrouping(TKey key, List<TValue> items)
            {
                Key = key;
                _items = items;
            }

            public void Add(TValue value)
            {
                _items.Add(value);
            }

            public IEnumerator<TValue> GetEnumerator()
            {
                return _items.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        public bool Contains(TKey key)
        {
            return _lookup.ContainsKey(key);
        }

        public IEnumerator<IGrouping<TKey, TValue>> GetEnumerator()
        {
            foreach (var group in _lookup.Values)
            {
                yield return group;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(TKey key, TValue value)
        {
            if (!_lookup.ContainsKey(key))
            {
                _lookup[key] = new MultiMapGrouping(key, new List<TValue>());
            }

            _lookup[key].Add(value);
            _dictionary[key] = value;
        }

        public bool ContainsKey(TKey key)
        {
            return Contains(key);
        }

        public bool Remove(TKey key)
        {
            return _lookup.Remove(key);
        }

        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
        {
            return _dictionary.TryGetValue(key, out value);
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        public void Clear()
        {
            _lookup.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return Contains(item.Key);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            _dictionary.CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return Remove(item.Key);
        }

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        public void Add((object? Key, object? Value) pair)
        {
            if (pair.Key != null)
            {
#pragma warning disable CS8604 // Possible null reference argument of value.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                Add((TKey)pair.Key, (TValue)pair.Value);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning restore CS8604 // Possible null reference argument of value.
            }
        }
    }
}
