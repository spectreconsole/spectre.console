namespace Spectre.Console;

/// <summary>
/// Represents a progress task.
/// </summary>
public sealed class ProgressTask : IProgress<double>
{
    private readonly Lazy<CircularBuffer<ProgressSample>> lazySamples;
    private readonly object _lock;
    private readonly TimeProvider _timeProvider;

    private double _maxValue;
    private string _description;
    private double _value;

    private volatile bool samplesChanged;

    private double? _cachedLastSpeed;
    private CircularBuffer<ProgressSample> Samples => lazySamples.Value;

    /// <summary>
    /// Gets the task ID.
    /// </summary>
    public int Id { get; }

    /// <summary>
    /// Gets or sets optional user tag data.
    /// </summary>
    public object? Tag { get; set; }

    /// <summary>
    /// Gets or sets if we should override the default hiding of this task when completed.
    /// </summary>
    public bool? HideWhenCompleted { get; set; }

    /// <summary>
    /// Gets or sets the task description.
    /// </summary>
    public string Description
    {
        get => _description;
        set => Update(description: value);
    }

    /// <summary>
    /// Gets or sets the max value of the task.
    /// </summary>
    public double MaxValue
    {
        get => _maxValue;
        set => Update(maxValue: value);
    }

    /// <summary>
    /// Gets or sets the value of the task.
    /// </summary>
    public double Value
    {
        get => _value;
        set => Update(value: value);
    }

    /// <summary>
    /// Gets the start time of the task.
    /// </summary>
    public DateTime? StartTime { get; private set; }

    /// <summary>
    /// Gets the stop time of the task.
    /// </summary>
    public DateTime? StopTime { get; private set; }

    /// <summary>
    /// Gets the task state.
    /// </summary>
    public ProgressTaskState State { get; }

    /// <summary>
    /// Gets a value indicating whether or not the task has started.
    /// </summary>
    public bool IsStarted => StartTime != null;

    /// <summary>
    /// Gets a value indicating whether or not the task has finished.
    /// </summary>
    public bool IsFinished => StopTime != null || Value >= MaxValue;

    /// <summary>
    /// Gets the percentage done of the task.
    /// </summary>
    public double Percentage => GetPercentage();

    /// <summary>
    /// Gets the speed measured in steps/second.
    /// </summary>
    public double? Speed => GetSpeed();

    /// <summary>
    /// Gets the elapsed time.
    /// </summary>
    public TimeSpan? ElapsedTime => GetElapsedTime();

    /// <summary>
    /// Gets the remaining time.
    /// </summary>
    public TimeSpan? RemainingTime => GetRemainingTime();

    /// <summary>
    /// Gets or sets a value indicating whether the ProgressBar shows
    /// actual values or generic, continuous progress feedback.
    /// </summary>
    public bool IsIndeterminate { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProgressTask"/> class.
    /// </summary>
    /// <param name="id">The task ID.</param>
    /// <param name="description">The task description.</param>
    /// <param name="maxValue">The task max value.</param>
    /// <param name="autoStart">Whether or not the task should start automatically.</param>
    /// <param name="timeProvider">The time provider to use. Defaults to <see cref="TimeProvider.System"/>.</param>
    public ProgressTask(int id, string description, double maxValue, bool autoStart = true, TimeProvider? timeProvider = null)
    {
        lazySamples = new(() => new CircularBuffer<ProgressSample>(MaxSamplesKept));
        _lock = new object();
        _timeProvider = timeProvider ?? TimeProvider.System;
        _maxValue = maxValue;
        _value = 0;
        _description = description?.RemoveNewLines()?.Trim() ??
                       throw new ArgumentNullException(nameof(description));

        if (string.IsNullOrWhiteSpace(_description))
        {
            throw new ArgumentException("Task name cannot be empty", nameof(description));
        }

        Id = id;
        State = new ProgressTaskState();
        StartTime = autoStart ? _timeProvider.GetLocalNow().LocalDateTime : null;
    }

    /// <summary>
    /// Starts the task.
    /// </summary>
    public void StartTask()
    {
        lock (_lock)
        {
            if (StopTime != null)
            {
                throw new InvalidOperationException("Stopped tasks cannot be restarted");
            }

            StartTime = _timeProvider.GetLocalNow().LocalDateTime;
            StopTime = null;
        }
    }

    /// <summary>
    /// Stops and marks the task as finished.
    /// </summary>
    public void StopTask()
    {
        lock (_lock)
        {
            var now = _timeProvider.GetLocalNow().LocalDateTime;
            StartTime ??= now;
            StopTime = now;
        }
    }

    /// <summary>
    /// Increments the task's value.
    /// </summary>
    /// <param name="value">The value to increment with.</param>
    public void Increment(double value)
    {
        Update(increment: value);
    }

    /// <summary>
    /// Gets or Sets the max age for samples we store to calculate speed/estimated time left. Samples older than this value are discarded to give you the 'current' speed.
    /// </summary>
    public TimeSpan MaxSamplingAge { get; set; } = TimeSpan.FromSeconds(30);

    /// <summary>
    /// Gets or Sets the maximum number of samples we use to calculate speed/time left. If set to 0, no samples are kept.
    /// </summary>
    public int MaxSamplesKept { get; set; } = 1000;

    private void Update(
        string? description = null,
        double? maxValue = null,
        double? increment = null,
        double? value = null)
    {
        lock (_lock)
        {
            var startValue = Value;

            if (description != null)
            {
                description = description?.RemoveNewLines()?.Trim();
                if (string.IsNullOrWhiteSpace(description))
                {
                    throw new InvalidOperationException("Task name cannot be empty.");
                }

                _description = description;
            }

            if (maxValue != null)
            {
                _maxValue = maxValue.Value;
            }

            if (increment != null)
            {
                _value += increment.Value;
            }

            if (value != null)
            {
                _value = value.Value;
            }

            // Need to cap the max value?
            if (_value > _maxValue)
            {
                _value = _maxValue;
            }

            if (MaxSamplesKept == 0)
            {
                return;
            }

            var timestamp = _timeProvider.GetLocalNow().LocalDateTime;
            samplesChanged = true;
            Samples.Add(new ProgressSample(timestamp, Value - startValue));
        }
    }
    private double GetPercentage()
    {
        if (MaxValue == 0)
        {
            return 100;
        }

        var percentage = (Value / MaxValue) * 100;
        percentage = Math.Min(100, Math.Max(0, percentage));
        return percentage;
    }

    private double? GetSpeed()
    {
        if (!samplesChanged || StartTime == null || !lazySamples.IsValueCreated || Samples.Count == 0 || StopTime != null)
        {
            return _cachedLastSpeed;
        }

        samplesChanged = false;

        lock (_lock)
        {
            var threshold = DateTime.Now - MaxSamplingAge;
            var validSamples = Samples.Where(a => a.Timestamp >= threshold);
            var first = validSamples.FirstOrDefault();
            if (first.Equals(default(ProgressSample)))
            {
                return _cachedLastSpeed = null;
            }

            var totalTime = Samples[Samples.Count - 1].Timestamp - first.Timestamp; // circular buffer automatically rotates index so length is newest and 0 is oldest, we
            if (totalTime == TimeSpan.Zero)
            {
                return _cachedLastSpeed = null;
            }

            var totalCompleted = validSamples.Sum(x => x.Value);
            return _cachedLastSpeed = totalCompleted / totalTime.TotalSeconds;
        }
    }

    private TimeSpan? GetElapsedTime()
    {
        lock (_lock)
        {
            if (StartTime == null)
            {
                return null;
            }

            if (StopTime != null)
            {
                return StopTime - StartTime;
            }

            return _timeProvider.GetLocalNow().LocalDateTime - StartTime;
        }
    }

    private TimeSpan? GetRemainingTime()
    {
        lock (_lock)
        {
            if (IsFinished)
            {
                return TimeSpan.Zero;
            }

            var speed = GetSpeed();
            if (speed == null || speed == 0)
            {
                return null;
            }

            // If the speed is near zero, the estimate below causes the
            // TimeSpan creation to throw an OverflowException. Just return
            // the maximum possible remaining time instead of overflowing.
            var estimate = (MaxValue - Value) / speed.Value;
            if (estimate > TimeSpan.MaxValue.TotalSeconds)
            {
                return TimeSpan.MaxValue;
            }

            return TimeSpan.FromSeconds(estimate);
        }
    }

    /// <inheritdoc />
    void IProgress<double>.Report(double value)
    {
        Update(increment: value - Value);
    }
}

/// <summary>
/// Contains extension methods for <see cref="ProgressTask"/>.
/// </summary>
public static class ProgressTaskExtensions
{
    /// <summary>
    /// Sets the task description.
    /// </summary>
    /// <param name="task">The task.</param>
    /// <param name="description">The description.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static ProgressTask Description(this ProgressTask task, string description)
    {
        ArgumentNullException.ThrowIfNull(task);

        task.Description = description;
        return task;
    }

    /// <summary>
    /// Sets the max value of the task.
    /// </summary>
    /// <param name="task">The task.</param>
    /// <param name="value">The max value.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static ProgressTask MaxValue(this ProgressTask task, double value)
    {
        ArgumentNullException.ThrowIfNull(task);

        task.MaxValue = value;
        return task;
    }

    /// <summary>
    /// Sets the value of the task.
    /// </summary>
    /// <param name="task">The task.</param>
    /// <param name="value">The value.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static ProgressTask Value(this ProgressTask task, double value)
    {
        ArgumentNullException.ThrowIfNull(task);

        task.Value = value;
        return task;
    }

    /// <summary>
    /// Sets whether the task is considered indeterminate or not.
    /// </summary>
    /// <param name="task">The task.</param>
    /// <param name="indeterminate">Whether the task is considered indeterminate or not.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static ProgressTask IsIndeterminate(this ProgressTask task, bool indeterminate = true)
    {
        ArgumentNullException.ThrowIfNull(task);

        task.IsIndeterminate = indeterminate;
        return task;
    }
}