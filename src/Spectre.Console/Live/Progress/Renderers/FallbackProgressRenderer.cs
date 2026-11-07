namespace Spectre.Console;

internal sealed class FallbackProgressRenderer : ProgressRenderer
{
    private const double FirstMilestone = 25;
    private static readonly double?[] _milestones = [FirstMilestone, 50, 75, 95, 96, 97, 98, 99, 100];

    private readonly Dictionary<int, double> _taskMilestones;
    private readonly HashSet<int> _indeterminateTasks;
    private readonly object _lock;
    private readonly TimeProvider _timeProvider;
    private IRenderable? _renderable;
    private DateTime _lastUpdate;

    public override TimeSpan RefreshRate => TimeSpan.FromSeconds(1);

    public FallbackProgressRenderer(TimeProvider timeProvider)
    {
        _taskMilestones = new Dictionary<int, double>();
        _indeterminateTasks = new HashSet<int>();
        _lock = new object();
        _timeProvider = timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));
    }

    public override void Update(ProgressContext context)
    {
        lock (_lock)
        {
            var hasStartedTasks = false;
            var updates = new List<(string Name, string Value)>();

            foreach (var task in context.GetTasks())
            {
                if (!task.IsStarted || task.IsFinished)
                {
                    continue;
                }

                hasStartedTasks = true;

                if (task.IsIndeterminate)
                {
                    if (_indeterminateTasks.Add(task.Id))
                    {
                        _taskMilestones.TryAdd(task.Id, FirstMilestone);
                        updates.Add(CreateUpdate(task));
                    }

                    continue;
                }

                var wasIndeterminate = _indeterminateTasks.Remove(task.Id);
                if (TryAdvance(task.Id, task.Percentage) || wasIndeterminate)
                {
                    updates.Add(CreateUpdate(task));
                }
            }

            // Got started tasks but no updates for 30 seconds?
            if (hasStartedTasks && updates.Count == 0 && (_timeProvider.GetLocalNow().LocalDateTime - _lastUpdate) > TimeSpan.FromSeconds(30))
            {
                foreach (var task in context.GetTasks())
                {
                    updates.Add(CreateUpdate(task));
                }
            }

            if (updates.Count > 0)
            {
                _lastUpdate = _timeProvider.GetLocalNow().LocalDateTime;
            }

            _renderable = BuildTaskGrid(updates);
        }
    }

    public override IEnumerable<IRenderable> Process(RenderOptions options, IEnumerable<IRenderable> renderables)
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

    private static (string Name, string Value) CreateUpdate(ProgressTask task)
    {
        var value = task.IsIndeterminate && !task.IsFinished
            ? "in progress"
            : $"{(int)task.Percentage}%";

        return (task.Description, value);
    }

    private static IRenderable? BuildTaskGrid(List<(string Name, string Value)> updates)
    {
        if (updates.Count > 0)
        {
            var renderables = new List<IRenderable>();
            foreach (var (name, value) in updates)
            {
                renderables.Add(new Markup($"[blue]{name}[/]: {value}"));
            }

            return new Rows(renderables);
        }

        return null;
    }
}