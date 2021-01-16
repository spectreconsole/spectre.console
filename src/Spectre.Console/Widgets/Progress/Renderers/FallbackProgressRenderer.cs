using System;
using System.Collections.Generic;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    internal sealed class FallbackProgressRenderer : ProgressRenderer
    {
        private const double FirstMilestone = 25;
        private static readonly double?[] _milestones = new double?[] { FirstMilestone, 50, 75, 95, 96, 97, 98, 99, 100 };

        private readonly Dictionary<int, double> _taskMilestones;
        private readonly object _lock;
        private IRenderable? _renderable;
        private DateTime _lastUpdate;

        public override TimeSpan RefreshRate => TimeSpan.FromSeconds(1);

        public FallbackProgressRenderer()
        {
            _taskMilestones = new Dictionary<int, double>();
            _lock = new object();
        }

        public override void Update(ProgressContext context)
        {
            lock (_lock)
            {
                var hasStartedTasks = false;
                var updates = new List<(string, double)>();

                foreach (var task in context.GetTasks())
                {
                    if (!task.IsStarted || task.IsFinished)
                    {
                        return;
                    }

                    hasStartedTasks = true;

                    if (TryAdvance(task.Id, task.Percentage))
                    {
                        updates.Add((task.Description, task.Percentage));
                    }
                }

                // Got started tasks but no updates for 30 seconds?
                if (hasStartedTasks && updates.Count == 0 && (DateTime.Now - _lastUpdate) > TimeSpan.FromSeconds(30))
                {
                    foreach (var task in context.GetTasks())
                    {
                        updates.Add((task.Description, task.Percentage));
                    }
                }

                if (updates.Count > 0)
                {
                    _lastUpdate = DateTime.Now;
                }

                _renderable = BuildTaskGrid(updates);
            }
        }

        public override IEnumerable<IRenderable> Process(RenderContext context, IEnumerable<IRenderable> renderables)
        {
            lock (_lock)
            {
                var result = new List<IRenderable>();
                result.AddRange(renderables);

                if (_renderable != null)
                {
                    result.Add(_renderable);
                }

                _renderable = null;

                return result;
            }
        }

        private bool TryAdvance(int task, double percentage)
        {
            if (!_taskMilestones.TryGetValue(task, out var milestone))
            {
                _taskMilestones.Add(task, FirstMilestone);
                return true;
            }

            if (percentage > milestone)
            {
                var nextMilestone = GetNextMilestone(percentage);
                if (nextMilestone != null && _taskMilestones[task] != nextMilestone)
                {
                    _taskMilestones[task] = nextMilestone.Value;
                    return true;
                }
            }

            return false;
        }

        private static double? GetNextMilestone(double percentage)
        {
            return Array.Find(_milestones, p => p > percentage);
        }

        private static IRenderable? BuildTaskGrid(List<(string Name, double Percentage)> updates)
        {
            if (updates.Count > 0)
            {
                var renderables = new List<IRenderable>();
                foreach (var (name, percentage) in updates)
                {
                    renderables.Add(new Markup($"[blue]{name}[/]: {(int)percentage}%"));
                }

                return new Rows(renderables);
            }

            return null;
        }
    }
}
