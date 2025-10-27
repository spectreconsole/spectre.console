// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

// As this file is copied from dotnet/aspire we don't want to style it in our format to make diffing easier in the future
#pragma warning disable SA1512 // Single-line comments should not be followed by blank line
#pragma warning disable SA1513 // Closing brace should be followed by blank line
#pragma warning disable SA1515 // Single-line comment should be preceded by blank line
#pragma warning disable SA1028 // Code should not contain trailing whitespace
#pragma warning disable SA1401 // Field should be private
#pragma warning disable SA1128 // Put constructor initializers on their own line
#pragma warning disable RCS1079 // Implement the functionality instead of throwing new NotImplementedException

// This was taken from https://github.com/dotnet/aspire/blob/a99edf17f50cbd2717f708706448e33a53825476/src/Shared/CircularBuffer.cs its license is also MIT.  This is the same exact circularbuffer that VS uses in Microsoft.VisualStudio.Utilities (at least a cursory decompiling shows it to be such): https://learn.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.utilities.circularbuffer-1?view=visualstudiosdk-2022. Minor fix for .netstandard 2.0 

using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Spectre.Console;

/// <summary>
/// The circular buffer starts with an empty list and grows to a maximum size.
/// When the buffer is full, adding or inserting a new item removes the first item in the buffer.
/// </summary>
[DebuggerDisplay("Count = {Count}")]
[DebuggerTypeProxy(typeof(CircularBuffer<>.CircularBufferDebugView))]
internal sealed class CircularBuffer<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable
{
    // Internal for testing.
    internal readonly List<T> _buffer;
    internal int _start;
    internal int _end;

    public event Action<T>? ItemRemovedForCapacity;

    public CircularBuffer(int capacity) : this(new List<T>(), capacity, start: 0, end: 0)
    {
    }

    internal CircularBuffer(List<T> buffer, int capacity, int start, int end)
    {
        if (capacity < 1)
        {
            throw new ArgumentException("Circular buffer must have a capacity greater than 0.", nameof(capacity));
        }

        _buffer = buffer;
        Capacity = capacity;
        _start = start;
        _end = end;
    }

    public int Capacity { get; }

    public bool IsFull => Count == Capacity;

    public bool IsEmpty => Count == 0;

    public int Count => _buffer.Count;

    public bool IsReadOnly { get; }

    public bool IsFixedSize { get; } = true;

    public object SyncRoot { get; } = new object();

    public bool IsSynchronized { get; }

    public int IndexOf(T item)
    {
        for (var index = 0; index < Count; ++index)
        {
            if (Equals(this[index], item))
            {
                return index;
            }
        }
        return -1;
    }

    public void Insert(int index, T item)
    {
        // TODO: There are a lot of branches in this method. Look into simplifying it.
        if (index == Count)
        {
            Add(item);
            return;
        }

        ValidateIndexInRange(index);

        if (IsFull)
        {
            if (index == 0)
            {
                // When full, the item inserted at 0 is always the "last" in the buffer and is removed.
                ItemRemovedForCapacity?.Invoke(item);
                return;
            }

            var removedItem = this[0];

            var internalIndex = InternalIndex(index);

#if !NETSTANDARD2_0
            var data = CollectionsMarshal.AsSpan(_buffer);
#else
            var data = _buffer.ToArray().AsSpan();
#endif
            // Shift data to make remove for insert.
            if (internalIndex == 0)
            {
                data.Slice(0, _end).CopyTo(data.Slice(1));
            }
            else if (internalIndex > _end)
            {
                // Data is shifted forward so save the last item to copy to the front.
                var overflowItem = data[data.Length - 1];

                var shiftLength = data.Length - internalIndex - 1;
                data.Slice(internalIndex, shiftLength).CopyTo(data.Slice(internalIndex + 1));
                if (shiftLength > 0 || internalIndex == _buffer.Count - 1)
                {
                    data.Slice(0, _end).CopyTo(data.Slice(1));
                }
                data[0] = overflowItem;
            }
            else if (internalIndex < _end && _end < _buffer.Count - 1)
            {
                data.Slice(internalIndex, _end - internalIndex).CopyTo(data.Slice(internalIndex + 1));
            }
            else
            {
                data.Slice(internalIndex, data.Length - internalIndex - 1).CopyTo(data.Slice(internalIndex + 1));
            }

            // Set the actual item.
            data[internalIndex] = item;

            Increment(ref _end);
            _start = _end;

            Debug.Assert(!_buffer.Contains(removedItem), "Item was not correctly removed.");
            ItemRemovedForCapacity?.Invoke(removedItem);
        }
        else
        {
            var internalIndex = index + _start;
            if (internalIndex > _buffer.Count)
            {
                internalIndex = internalIndex % _buffer.Count;
            }

            _buffer.Insert(internalIndex, item);
            if (internalIndex < _end)
            {
                Increment(ref _end);
                if (_end != _buffer.Count)
                {
                    _start = _end;
                }
            }
        }
    }

    public void RemoveAt(int index)
    {
        ValidateIndexInRange(index);

        var internalIndex = InternalIndex(index);
        _buffer.RemoveAt(internalIndex);
        if (internalIndex < _end)
        {
            Decrement(ref _end);
            if (_start > 0)
            {
                _start = _end;
            }
        }
    }

    private void ValidateIndexInRange(int index)
    {
        if (index >= Count)
        {
            throw new InvalidOperationException($"Cannot access index {index}. Buffer size is {Count}");
        }
    }

    public bool Remove(T item) => throw new NotImplementedException();

    public T this[int index]
    {
        get
        {
            ValidateIndexInRange(index);
            return _buffer[InternalIndex(index)];
        }
        set
        {
            ValidateIndexInRange(index);
            _buffer[InternalIndex(index)] = value;
        }
    }

    public void Add(T item)
    {
        if (IsFull)
        {
            var removedItem = this[0];

            _buffer[_end] = item;
            Increment(ref _end);
            _start = _end;

            Debug.Assert(!_buffer.Contains(removedItem), "Item was not correctly removed.");
            ItemRemovedForCapacity?.Invoke(removedItem);
        }
        else
        {
            _buffer.Insert(_end, item);
            Increment(ref _end);
            if (_end != _buffer.Count)
            {
                _start = _end;
            }
        }
    }

    public void Clear()
    {
        _start = 0;
        _end = 0;
        _buffer.Clear();
    }

    public bool Contains(T item) => IndexOf(item) != -1;

    public void CopyTo(T[] array, int arrayIndex)
    {
        if (array.Length - arrayIndex < Count)
        {
            throw new ArgumentException("Array does not contain enough space for items");
        }

        for (var index = 0; index < Count; ++index)
        {
            array[index + arrayIndex] = this[index];
        }
    }

    public T[] ToArray()
    {
        if (IsEmpty)
        {
            return Array.Empty<T>();
        }

        var array = new T[Count];
        for (var index = 0; index < Count; ++index)
        {
            array[index] = this[index];
        }

        return array;
    }

    public IEnumerator<T> GetEnumerator()
    {
        for (var i = 0; i < Count; ++i)
        {
            yield return this[i];
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    private int InternalIndex(int index)
    {
        return (_start + index) % _buffer.Count;
    }

    private void Increment(ref int index)
    {
        if (++index < Capacity)
        {
            return;
        }

        index = 0;
    }

    private void Decrement(ref int index)
    {
        if (index <= 0)
        {
            index = Capacity - 1;
        }

        --index;
    }

    public CircularBuffer<T> Clone()
    {
        var buffer = new CircularBuffer<T>(_buffer.ToList(), Capacity, _start, _end);
        buffer.ItemRemovedForCapacity = ItemRemovedForCapacity;

        return buffer;
    }

    private sealed class CircularBufferDebugView(CircularBuffer<T> collection)
    {
        private readonly CircularBuffer<T> _collection = collection;

        public T[] Items => _collection.ToArray();
        public int Start => _collection._start;
        public int End => _collection._end;
        public int Capacity => _collection.Capacity;
    }
}
