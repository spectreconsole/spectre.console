using System;
using System.Collections.Generic;

namespace Spectre.Console
{
    /// <summary>
    /// Represents progress task state.
    /// </summary>
    public sealed class ProgressTaskState
    {
        private readonly Dictionary<string, object> _state;
        private readonly object _lock;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgressTaskState"/> class.
        /// </summary>
        public ProgressTaskState()
        {
            _state = new Dictionary<string, object>();
            _lock = new object();
        }

        /// <summary>
        /// Gets the state value for the specified key.
        /// </summary>
        /// <typeparam name="T">The state value type.</typeparam>
        /// <param name="key">The state key.</param>
        /// <returns>The value for the specified key.</returns>
        public T Get<T>(string key)
            where T : struct
        {
            lock (_lock)
            {
                if (!_state.TryGetValue(key, out var value))
                {
                    return default;
                }

                if (!(value is T))
                {
                    throw new InvalidOperationException("State value is of the wrong type.");
                }

                return (T)value;
            }
        }

        /// <summary>
        /// Updates a task state value.
        /// </summary>
        /// <typeparam name="T">The state value type.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="func">The transformation function.</param>
        /// <returns>The updated value.</returns>
        public T Update<T>(string key, Func<T, T> func)
            where T : struct
        {
            lock (_lock)
            {
                if (func is null)
                {
                    throw new ArgumentNullException(nameof(func));
                }

                var old = default(T);
                if (_state.TryGetValue(key, out var value))
                {
                    if (!(value is T))
                    {
                        throw new InvalidOperationException("State value is of the wrong type.");
                    }

                    old = (T)value;
                }

                _state[key] = func(old);
                return (T)_state[key];
            }
        }
    }
}
