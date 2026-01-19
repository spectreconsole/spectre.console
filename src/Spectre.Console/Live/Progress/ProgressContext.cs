namespace Spectre.Console;

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
    /// Gets a value indicating whether or not all started tasks have completed.
    /// </summary>
    public bool IsFinished
    {
        get
        {
            lock (_taskLock)
            {
                return _tasks.Where(x => x.IsStarted).All(task => task.IsFinished);
            }
        }
    }

    internal ProgressContext(IAnsiConsole console, ProgressRenderer renderer)
    {
        _tasks = [];
        _taskLock = new object();
        _console = console ?? throw new ArgumentNullException(nameof(console));
        _renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
    }

    /// <summary>
    /// Adds a task.
    /// </summary>
    /// <param name="description">The task description.</param>
    /// <param name="autoStart">Whether or not the task should start immediately.</param>
    /// <param name="maxValue">The task's max value.</param>
    /// <returns>The newly created task.</returns>
    public ProgressTask AddTask(string description, bool autoStart = true, double maxValue = 100)
    {
        lock (_taskLock)
        {
            var settings = new ProgressTaskSettings { AutoStart = autoStart, MaxValue = maxValue, };

            return AddTaskAtInternal(description, settings, _tasks.Count);
        }
    }

    /// <summary>
    /// Adds a task.
    /// </summary>
    /// <param name="description">The task description.</param>
    /// <param name="index">The index at which the task should be inserted.</param>
    /// <param name="autoStart">Whether or not the task should start immediately.</param>
    /// <param name="maxValue">The task's max value.</param>
    /// <returns>The newly created task.</returns>
    public ProgressTask AddTaskAt(string description, int index, bool autoStart = true, double maxValue = 100)
    {
        lock (_taskLock)
        {
            var settings = new ProgressTaskSettings { AutoStart = autoStart, MaxValue = maxValue, };

            return AddTaskAtInternal(description, settings, index);
        }
    }

    /// <summary>
    /// Adds a task before the reference task.
    /// </summary>
    /// <param name="description">The task description.</param>
    /// <param name="referenceProgressTask">The reference task to add before.</param>
    /// <param name="autoStart">Whether or not the task should start immediately.</param>
    /// <param name="maxValue">The task's max value.</param>
    /// <returns>The newly created task.</returns>
    public ProgressTask AddTaskBefore(string description, ProgressTask referenceProgressTask, bool autoStart = true, double maxValue = 100)
    {
        lock (_taskLock)
        {
            var settings = new ProgressTaskSettings { AutoStart = autoStart, MaxValue = maxValue, };
            var indexOfReference = _tasks.IndexOf(referenceProgressTask);

            return AddTaskAtInternal(description, settings, indexOfReference);
        }
    }

    /// <summary>
    /// Adds a task after the reference task.
    /// </summary>
    /// <param name="description">The task description.</param>
    /// <param name="referenceProgressTask">The reference task to add after.</param>
    /// <param name="autoStart">Whether or not the task should start immediately.</param>
    /// <param name="maxValue">The task's max value.</param>
    /// <returns>The newly created task.</returns>
    public ProgressTask AddTaskAfter(string description, ProgressTask referenceProgressTask, bool autoStart = true, double maxValue = 100)
    {
        lock (_taskLock)
        {
            var settings = new ProgressTaskSettings { AutoStart = autoStart, MaxValue = maxValue, };
            var indexOfReference = _tasks.IndexOf(referenceProgressTask);

            return AddTaskAtInternal(description, settings, indexOfReference + 1);
        }
    }

    /// <summary>
    /// Adds a task.
    /// </summary>
    /// <param name="description">The task description.</param>
    /// <param name="settings">The task settings.</param>
    /// <returns>The newly created task.</returns>
    public ProgressTask AddTask(string description, ProgressTaskSettings settings)
    {
        lock (_taskLock)
        {
            return AddTaskAtInternal(description, settings, _tasks.Count);
        }
    }

    /// <summary>
    /// Adds a task at the specified index.
    /// </summary>
    /// <param name="description">The task description.</param>
    /// <param name="settings">The task settings.</param>
    /// <param name="index">The index at which the task should be inserted.</param>
    /// <returns>The newly created task.</returns>
    public ProgressTask AddTaskAt(string description, ProgressTaskSettings settings, int index)
    {
        lock (_taskLock)
        {
            return AddTaskAtInternal(description, settings, index);
        }
    }

    /// <summary>
    /// Adds a task before the reference task.
    /// </summary>
    /// <param name="description">The task description.</param>
    /// <param name="settings">The task settings.</param>
    /// <param name="referenceProgressTask">The reference task to add before.</param>
    /// <returns>The newly created task.</returns>
    public ProgressTask AddTaskBefore(string description, ProgressTaskSettings settings, ProgressTask referenceProgressTask)
    {
        lock (_taskLock)
        {
            var indexOfReference = _tasks.IndexOf(referenceProgressTask);

            return AddTaskAtInternal(description, settings, indexOfReference);
        }
    }

    /// <summary>
    /// Adds a task after the reference task.
    /// </summary>
    /// <param name="description">The task description.</param>
    /// <param name="settings">The task settings.</param>
    /// <param name="referenceProgressTask">The reference task to add after.</param>
    /// <returns>The newly created task.</returns>
    public ProgressTask AddTaskAfter(string description, ProgressTaskSettings settings, ProgressTask referenceProgressTask)
    {
        lock (_taskLock)
        {
            var indexOfReference = _tasks.IndexOf(referenceProgressTask);

            return AddTaskAtInternal(description, settings, indexOfReference + 1);
        }
    }

    /// <summary>
    /// Refreshes the current progress.
    /// </summary>
    public void Refresh()
    {
        _renderer.Update(this);
        _console.Write(ControlCode.Empty);
    }

    private ProgressTask AddTaskAtInternal(string description, ProgressTaskSettings settings, int position)
    {
        ArgumentNullException.ThrowIfNull(settings);

        var task = new ProgressTask(_taskId++, description, settings.MaxValue, settings.AutoStart);

        _tasks.Insert(position, task);

        return task;
    }

    internal IReadOnlyList<ProgressTask> GetTasks()
    {
        lock (_taskLock)
        {
            return new List<ProgressTask>(_tasks);
        }
    }
}