// Copyright (c) All contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#define SOURCE_GENERATOR
#pragma warning disable SA1625 // ElementDocumentationMustNotBeCopiedAndPasted
#pragma warning disable SA1649 // SA1649FileNameMustMatchTypeName
#if (NETSTANDARD2_1_OR_GREATER || NETCOREAPP1_0_OR_GREATER || NET5_0_OR_GREATER) && !NET9_0_OR_GREATER
using System.Runtime.CompilerServices;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace System.Threading;
#pragma warning restore IDE0130 // Namespace does not match folder structure

/// <summary>
/// A backport of .NET 9.0+'s System.Threading.Lock. Provides a way to get mutual exclusion in regions of code between different threads.
/// A lock may be held by one thread at a time.
/// </summary>
/// <remarks>
/// Threads that cannot immediately enter the lock may wait for the lock to be exited or until a specified timeout. A thread
/// that holds a lock may enter the lock repeatedly without exiting it, such as recursively, in which case the thread should
/// eventually exit the lock the same number of times to fully exit the lock and allow other threads to enter the lock.
/// </remarks>
#if SOURCE_GENERATOR
internal
#else
public
#endif
sealed class Lock
{
#pragma warning disable CS9216 // A value of type 'System.Threading.Lock' converted to a different type will use likely unintended monitor-based locking in 'lock' statement.
    /// <summary>
    /// Determines whether the current thread holds this lock.
    /// </summary>
    /// <returns>
    /// true if the current thread holds this lock; otherwise, false.
    /// </returns>
#pragma warning disable SA1623 // Property summary documentation should match accessors
    public bool IsHeldByCurrentThread => Monitor.IsEntered(this);
#pragma warning restore SA1623 // Property summary documentation should match accessors

    /// <summary>
    /// <inheritdoc cref="Monitor.Enter(object)"/>
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Enter() => Monitor.Enter(this);

    /// <summary>
    /// <inheritdoc cref="Monitor.TryEnter(object)"/>
    /// </summary>
    /// <returns>
    /// <inheritdoc cref="Monitor.TryEnter(object)"/>
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryEnter() => Monitor.TryEnter(this);

    /// <summary>
    /// <inheritdoc cref="Monitor.TryEnter(object, TimeSpan)"/>
    /// </summary>
    /// <returns>
    /// <inheritdoc cref="Monitor.TryEnter(object, TimeSpan)"/>
    /// </returns>
    /// <param name="timeout">A <see cref="TimeSpan" /> representing the amount of time to wait for the lock.
    /// A value of -1 millisecond specifies an infinite wait.</param>
    /// <exception cref="ArgumentOutOfRangeException">The value of timeout in milliseconds is negative and is not equal to <see cref="Timeout.Infinite"/>
    /// (-1 millisecond), or is greater than <see cref="int.MaxValue"/>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryEnter(TimeSpan timeout) => Monitor.TryEnter(this, timeout);

    /// <summary>
    /// <inheritdoc cref="Monitor.TryEnter(object, int)"/>
    /// </summary>
    /// <returns>
    /// <inheritdoc cref="Monitor.TryEnter(object, int)"/>
    /// </returns>
    /// <param name="millisecondsTimeout">The number of milliseconds to wait for the lock.</param>
    /// <exception cref="ArgumentOutOfRangeException">millisecondsTimeout is negative, and not equal to <see cref="Timeout.Infinite"/>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryEnter(int millisecondsTimeout) => Monitor.TryEnter(this, millisecondsTimeout);

    /// <summary>
    /// <inheritdoc cref="Monitor.Exit(object)"/>
    /// </summary>
    /// <exception cref="SynchronizationLockException">The current thread does not own the lock for the specified object.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Exit() => Monitor.Exit(this);
#pragma warning restore CS9216 // A value of type 'System.Threading.Lock' converted to a different type will use likely unintended monitor-based locking in 'lock' statement.

    /// <summary>
    /// Enters the lock and returns a <see cref="Scope"/> that may be disposed to exit the lock. Once the method returns,
    /// the calling thread would be the only thread that holds the lock. This method is intended to be used along with a
    /// language construct that would automatically dispose the <see cref="Scope"/>, such as with the C# using statement.
    /// </summary>
    /// <returns>
    /// A <see cref="Scope"/> that may be disposed to exit the lock.
    /// </returns>
    /// <remarks>
    /// If the lock cannot be entered immediately, the calling thread waits for the lock to be exited. If the lock is
    /// already held by the calling thread, the lock is entered again. The calling thread should exit the lock, such as by
    /// disposing the returned <see cref="Scope"/>, as many times as it had entered the lock to fully exit the lock and
    /// allow other threads to enter the lock.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Scope EnterScope()
    {
        this.Enter();
        return new Scope(this);
    }

    /// <summary>
    /// A disposable structure that is returned by <see cref="EnterScope()"/>, which when disposed, exits the lock.
    /// </summary>
    public ref struct Scope(Lock @lock)
    {
        /// <summary>
        /// Exits the lock.
        /// </summary>
        /// <remarks>
        /// If the calling thread holds the lock multiple times, such as recursively, the lock is exited only once. The
        /// calling thread should ensure that each enter is matched with an exit.
        /// </remarks>
        /// <exception cref="SynchronizationLockException">
        /// The calling thread does not hold the lock.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void Dispose() => @lock.Exit();
    }
}
#endif
#pragma warning restore SA1649 // SA1649FileNameMustMatchTypeName
#pragma warning restore SA1625 // ElementDocumentationMustNotBeCopiedAndPasted
