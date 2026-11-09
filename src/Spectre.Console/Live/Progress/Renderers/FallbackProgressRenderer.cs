namespace Spectre.Console;

internal sealed class FallbackProgressRenderer : ProgressRenderer
{
    private readonly double[] _milestones;
    private readonly Func<ProgressTask, string> _formatter;
    private readonly Dictionary<int, double> _taskMilestones;
    private readonly object _lock;
    private readonly TimeProvider _timeProvider;
    private IRenderable? _renderable;
    private DateTime _lastUpdate;

    public override TimeSpan RefreshRate => TimeSpan.FromSeconds(1);

    public FallbackProgressRenderer(TimeProvider timeProvider, IEnumerable<double> milestones, Func<ProgressTask, string> formatter)
    {
        ArgumentNullException.ThrowIfNull(milestones);

        _milestones = milestones.Distinct().OrderBy(m => m).ToArray();
        _formatter = formatter ?? throw new ArgumentNullException(nameof(formatter));
        _taskMilestones = new Dictionary<int, double>();
        _lock = new object();
        _timeProvider = timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));
    }

    public override void Update(ProgressContext context)
    {
        lock (_lock)
        {
            var hasStartedTasks = false;
            var updates = new List<ProgressTask>();

            foreach (var task in context.GetTasks())
            {
                if (!task.IsStarted || task.IsFinished)
                {
                    continue;
                }

                hasStartedTasks = true;

                if (TryAdvance(task.Id, task.Percentage))
                {
                    updates.Add(task);
                }
            }

            // Got started tasks but no updates for 30 seconds?
            if (hasStartedTasks && updates.Count == 0 && (_timeProvider.GetLocalNow().LocalDateTime - _lastUpdate) > TimeSpan.FromSeconds(30))
            {
                updates.AddRange(context.GetTasks());
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
        var milestone = GetReachedMilestone(percentage);

        if (_taskMilestones.TryGetValue(task, out var previous) && milestone <= previous)
        {
            return false;
        }

        _taskMilestones[task] = milestone;
        return true;
    }

    private double GetReachedMilestone(double percentage)
    {
        var reached = double.NegativeInfinity;

        foreach (var milestone in _milestones)
        {
            if (milestone > percentage)
            {
                break;
            }

            reached = milestone;
        }

        return reached;
    }

    private Rows? BuildTaskGrid(List<ProgressTask> updates)
    {
        if (updates.Count > 0)
        {
            var renderables = new List<IRenderable>();
            foreach (var task in updates)
            {
                renderables.Add(new Markup(_formatter(task)));
            }

            return new Rows(renderables);
        }

        return null;
    }
}