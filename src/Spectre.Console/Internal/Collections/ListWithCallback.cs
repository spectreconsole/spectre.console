using System;
using System.Collections;
using System.Collections.Generic;

namespace Spectre.Console
{
    internal sealed class ListWithCallback<T> : IList<T>, IReadOnlyList<T>
    {
        private readonly List<T> _list;
        private readonly Action _callback;

        public T this[int index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        public int Count => _list.Count;
        public bool IsReadOnly => false;

        public ListWithCallback(Action callback)
        {
            _list = new List<T>();
            _callback = callback ?? throw new ArgumentNullException(nameof(callback));
        }

        public void Add(T item)
        {
            _list.Add(item);
            _callback();
        }

        public void Clear()
        {
            _list.Clear();
            _callback();
        }

        public bool Contains(T item)
        {
            return _list.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
            _callback();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        public int IndexOf(T item)
        {
            return _list.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            _list.Insert(index, item);
            _callback();
        }

        public bool Remove(T item)
        {
            var result = _list.Remove(item);
            if (result)
            {
                _callback();
            }

            return result;
        }

        public void RemoveAt(int index)
        {
            _list.RemoveAt(index);
            _callback();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
