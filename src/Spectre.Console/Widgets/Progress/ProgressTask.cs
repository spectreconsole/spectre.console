using System;
using System.Collections.Generic;
using System.Linq;

namespace Spectre.Console
{
    /// <summary>
    /// Represents a progress task.
    /// </summary>
    public sealed class ProgressTask : IProgress<double>
    {
        private readonly List<ProgressSample> _samples;
        private readonly object _lock;

        private double _maxValue;
        private string _description;
        private double _value;

        /// <summary>
        /// Gets the task ID.
        /// </summary>
        public int Id { get; }

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
        public ProgressTask(int id, string description, double maxValue, bool autoStart = true)
        {
            _samples = new List<ProgressSample>();
            _lock = new object();
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
            StartTime = autoStart ? DateTime.Now : null;
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

                StartTime = DateTime.Now;
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
                var now = DateTime.Now;
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

                var timestamp = DateTime.Now;
                var threshold = timestamp - TimeSpan.FromSeconds(30);

                // Remove samples that's too old
                while (_samples.Count > 0 && _samples[0].Timestamp < threshold)
                {
                    _samples.RemoveAt(0);
                }

                // Keep maximum of 1000 samples
                while (_samples.Count > 1000)
                {
                    _samples.RemoveAt(0);
                }

                _samples.Add(new ProgressSample(timestamp, Value - startValue));
            }
        }

        private double GetPercentage()
        {
            var percentage = (Value / MaxValue) * 100;
            percentage = Math.Min(100, Math.Max(0, percentage));
            return percentage;
        }

        private double? GetSpeed()
        {
            lock (_lock)
            {
                if (StartTime == null)
                {
                    return null;
                }

                if (_samples.Count == 0)
                {
                    return null;
                }

                var totalTime = _samples.Last().Timestamp - _samples[0].Timestamp;
                if (totalTime == TimeSpan.Zero)
                {
                    return null;
                }

                var totalCompleted = _samples.Sum(x => x.Value);
                return totalCompleted / totalTime.TotalSeconds;
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

                return DateTime.Now - StartTime;
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
                if (speed == null)
                {
                    return null;
                }

                // If the speed is zero, the estimate below
                // will return infinity (since it's a double),
                // so let's set the speed to 1 in that case.
                if (speed == 0)
                {
                    speed = 1;
                }

                var estimate = (MaxValue - Value) / speed.Value;
                return TimeSpan.FromSeconds(estimate);
            }
        }

        /// <inheritdoc />
        void IProgress<double>.Report(double value)
        {
            Update(increment: value - Value);
        }
    }
}