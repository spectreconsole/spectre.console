using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spectre.Console.Internal;

namespace Spectre.Console
{
    /// <summary>
    /// Represents a context that can be used to interact with a <see cref="Progress"/>.
    /// </summary>
    public sealed class ProgressContext
    {
        private readonly List<ProgressTask> _tasks;
        private readonly object _taskLock;
        private readonly IAnsiConsole _console;
        private readonly ProgressRenderer _renderer;
        private int _taskId;

        /// <summary>
        /// Gets a value indicating whether or not all tasks have completed.
        /// </summary>
        public bool IsFinished => _tasks.All(task => task.IsFinished);

        internal Encoding Encoding => _console.Encoding;

        internal ProgressContext(IAnsiConsole console, ProgressRenderer renderer)
        {
            _tasks = new List<ProgressTask>();
            _taskLock = new object();
            _console = console ?? throw new ArgumentNullException(nameof(console));
            _renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
        }

        /// <summary>
        /// Adds a task.
        /// </summary>
        /// <param name="description">The task description.</param>
        /// <param name="settings">The task settings.</param>
        /// <returns>The task's ID.</returns>
        public ProgressTask AddTask(string description, ProgressTaskSettings? settings = null)
        {
            lock (_taskLock)
            {
                settings ??= new ProgressTaskSettings();
                var task = new ProgressTask(_taskId++, description, settings.MaxValue, settings.AutoStart);

                _tasks.Add(task);
                return task;
            }
        }

        /// <summary>
        /// Refreshes the current progress.
        /// </summary>
        public void Refresh()
        {
            _renderer.Update(this);
            _console.Render(new ControlSequence(string.Empty));
        }

        internal IReadOnlyList<ProgressTask> GetTasks()
        {
            lock (_taskLock)
            {
                return new List<ProgressTask>(_tasks);
            }
        }
    }
}
