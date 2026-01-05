using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Spectre.Console.SourceGenerator;

/// <summary>
/// An immutable array wrapper with structural equality for incremental generator caching.
/// </summary>
/// <typeparam name="T">The element type, must implement IEquatable{T}.</typeparam>
internal readonly struct EquatableArray<T> : IEquatable<EquatableArray<T>>, IEnumerable<T>
    where T : IEquatable<T>
{
    /// <summary>
    /// An empty array.
    /// </summary>
    public static readonly EquatableArray<T> Empty = new(ImmutableArray<T>.Empty);

    private readonly ImmutableArray<T> _array;

    /// <summary>
    /// Creates a new EquatableArray from the given immutable array.
    /// </summary>
    public EquatableArray(ImmutableArray<T> array) => _array = array;

    /// <summary>
    /// Creates a new EquatableArray from the given enumerable.
    /// </summary>
    public EquatableArray(IEnumerable<T> items) => _array = items.ToImmutableArray();

    /// <summary>
    /// Gets the number of elements.
    /// </summary>
    public int Count => _array.IsDefault ? 0 : _array.Length;

    /// <summary>
    /// Gets whether the array is empty.
    /// </summary>
    public bool IsEmpty => _array.IsDefault || _array.Length == 0;

    /// <summary>
    /// Gets the element at the specified index.
    /// </summary>
    public T this[int index] => _array[index];

    /// <inheritdoc />
    public bool Equals(EquatableArray<T> other)
    {
        if (_array.IsDefault && other._array.IsDefault)
        {
            return true;
        }

        if (_array.IsDefault || other._array.IsDefault)
        {
            return false;
        }

        if (_array.Length != other._array.Length)
        {
            return false;
        }

        for (var i = 0; i < _array.Length; i++)
        {
            if (!_array[i].Equals(other._array[i]))
            {
                return false;
            }
        }

        return true;
    }

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is EquatableArray<T> other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode()
    {
        if (_array.IsDefault)
        {
            return 0;
        }

        var hash = 17;
        foreach (var item in _array)
        {
            hash = (hash * 31) + (item?.GetHashCode() ?? 0);
        }

        return hash;
    }

    /// <inheritdoc />
    public IEnumerator<T> GetEnumerator() =>
        ((IEnumerable<T>)(_array.IsDefault ? ImmutableArray<T>.Empty : _array)).GetEnumerator();

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <summary>
    /// Equality operator.
    /// </summary>
    public static bool operator ==(EquatableArray<T> left, EquatableArray<T> right) => left.Equals(right);

    /// <summary>
    /// Inequality operator.
    /// </summary>
    public static bool operator !=(EquatableArray<T> left, EquatableArray<T> right) => !left.Equals(right);
}
