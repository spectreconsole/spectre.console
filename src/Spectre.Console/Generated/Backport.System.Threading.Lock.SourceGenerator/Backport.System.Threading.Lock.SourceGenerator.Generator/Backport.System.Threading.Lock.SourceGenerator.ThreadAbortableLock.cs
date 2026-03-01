// Copyright (c) All contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#define SOURCE_GENERATOR
#pragma warning disable SA1625 // ElementDocumentationMustNotBeCopiedAndPasted
#pragma warning disable SA1649 // SA1649FileNameMustMatchTypeName
#if !NET9_0_OR_GREATER
using System;
#if NET45_OR_GREATER || NETSTANDARD1_0_OR_GREATER || NETCOREAPP1_0_OR_GREATER || NET5_0_OR_GREATER
using System.Runtime.CompilerServices;
#endif
using System.Threading;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace Backport.System.Threading;
#pragma warning restore IDE0130 // Namespace does not match folder structure

/// <summary>
/// A backport of .NET 9.0+'s System.Threading.Lock. Provides a way to get mutual exclusion in regions of code between different threads.
/// A lock may be held by one thread at a time. Do not try and create an instance of this class; use <see cref="LockFactory.Create()"/>.
/// </summary>
/// <remarks>
/// Threads that cannot immediately enter the lock may wait for the lock to be exited or until a specified timeout. A thread
/// that holds a lock may enter the lock repeatedly without exiting it, such as recursively, in which case the thread should
/// eventually exit the lock the same number of times to fully exit the lock and allow other threads to enter the lock.
/// </remarks>
#pragma warning disable SA1649 // SA1649FileNameMustMatchTypeName
#if SOURCE_GENERATOR
internal
#else
public
#endif
sealed class Lock
#pragma warning restore SA1649 // SA1649FileNameMustMatchTypeName
{
#pragma warning disable SA1642 // ConstructorSummaryDocumentationMustBeginWithStandardText
    /// <summary>
    /// Use <see cref="LockFactory.Create()"/> instead.
    /// </summary>
#pragma warning restore SA1642 // ConstructorSummaryDocumentationMustBeginWithStandardText
    internal Lock()
    {
    }

#if NET45_OR_GREATER || NETSTANDARD1_0_OR_GREATER || NETCOREAPP1_0_OR_GREATER || NET5_0_OR_GREATER
    /// <summary>
    /// Determines whether the current thread holds this lock.
    /// </summary>
    /// <returns>
    /// true if the current thread holds this lock; otherwise, false.
    /// </returns>
    /// <exception cref="ArgumentNullException"><inheritdoc cref="ArgumentNullException"/></exception>
#pragma warning disable SA1623 // Property summary documentation should match accessors
    public bool IsHeldByCurrentThread => Monitor.IsEntered(this);
#pragma warning restore SA1623 // Property summary documentation should match accessors
#endif

    /// <summary>
    /// <inheritdoc cref="Monitor.Enter(object)"/>
    /// </summary>
#if NET45_OR_GREATER || NETSTANDARD1_0_OR_GREATER || NETCOREAPP1_0_OR_GREATER || NET5_0_OR_GREATER
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public void Enter() => Monitor.Enter(this);

    /// <summary>
    /// <inheritdoc cref="Monitor.TryEnter(object)"/>
    /// </summary>
    /// <returns>
    /// <inheritdoc cref="Monitor.TryEnter(object)"/>
    /// </returns>
#if NET45_OR_GREATER || NETSTANDARD1_0_OR_GREATER || NETCOREAPP1_0_OR_GREATER || NET5_0_OR_GREATER
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
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
#if NET45_OR_GREATER || NETSTANDARD1_0_OR_GREATER || NETCOREAPP1_0_OR_GREATER || NET5_0_OR_GREATER
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public bool TryEnter(TimeSpan timeout) => Monitor.TryEnter(this, timeout);

    /// <summary>
    /// <inheritdoc cref="Monitor.TryEnter(object, int)"/>
    /// </summary>
    /// <returns>
    /// <inheritdoc cref="Monitor.TryEnter(object, int)"/>
    /// </returns>
    /// <param name="millisecondsTimeout">The number of milliseconds to wait for the lock.</param>
    /// <exception cref="ArgumentOutOfRangeException">millisecondsTimeout is negative, and not equal to <see cref="Timeout.Infinite"/>.</exception>
#if NET45_OR_GREATER || NETSTANDARD1_0_OR_GREATER || NETCOREAPP1_0_OR_GREATER || NET5_0_OR_GREATER
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public bool TryEnter(int millisecondsTimeout) => Monitor.TryEnter(this, millisecondsTimeout);

    /// <summary>
    /// <inheritdoc cref="Monitor.Exit(object)"/>
    /// </summary>
    /// <exception cref="SynchronizationLockException"><inheritdoc cref="Monitor.Exit(object)"/></exception>
#if NET45_OR_GREATER || NETSTANDARD1_0_OR_GREATER || NETCOREAPP1_0_OR_GREATER || NET5_0_OR_GREATER
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public void Exit() => Monitor.Exit(this);

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
#if NET45_OR_GREATER || NETSTANDARD1_0_OR_GREATER || NETCOREAPP1_0_OR_GREATER || NET5_0_OR_GREATER
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
#if (NET40_OR_GREATER || NETSTANDARD2_0_OR_GREATER) && !NET5_0_OR_GREATER
    [Obsolete("This method is a best-effort at hardening against thread aborts, but can theoretically retain lock on pre-.NET 5.0. Use with caution.")]
    public Scope EnterScope()
    {
        bool lockTaken = false;
        try
        {
            Monitor.Enter(this, ref lockTaken);
            return new Scope(this);
        }
        catch (ThreadAbortException)
        {
            if (lockTaken)
            {
                Monitor.Exit(this);
            }

            throw;
        }
    }
#else
    public Scope EnterScope()
    {
        Monitor.Enter(this);
        return new Scope(this);
    }
#endif

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
#if NET45_OR_GREATER || NETSTANDARD1_0_OR_GREATER || NETCOREAPP1_0_OR_GREATER || NET5_0_OR_GREATER
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public readonly void Dispose() => @lock.Exit();
    }
}
#endif
#pragma warning restore SA1649 // SA1649FileNameMustMatchTypeName
#pragma warning restore SA1625 // ElementDocumentationMustNotBeCopiedAndPasted
