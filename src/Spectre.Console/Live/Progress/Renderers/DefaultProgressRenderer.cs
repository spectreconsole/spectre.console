namespace Spectre.Console;

internal sealed class DefaultProgressRenderer : ProgressRenderer
{
    private readonly IAnsiConsole _console;
    private readonly List<ProgressColumn> _columns;
    private readonly LiveRenderable _live;
    private readonly object _lock;
    private readonly Stopwatch _stopwatch;
    private readonly bool _hideCompleted;
    private TimeSpan _lastUpdate;

    public override TimeSpan RefreshRate { get; }

    public DefaultProgressRenderer(IAnsiConsole console, List<ProgressColumn> columns, TimeSpan refreshRate, bool hideCompleted)
    {
        _console = console ?? throw new ArgumentNullException(nameof(console));
        _columns = columns ?? throw new ArgumentNullException(nameof(columns));
        _live = new LiveRenderable(console);
        _lock = new object();
        _stopwatch = new Stopwatch();
        _lastUpdate = TimeSpan.Zero;
        _hideCompleted = hideCompleted;

        RefreshRate = refreshRate;
    }

    public override void Started()
    {
        _console.Cursor.Hide();
    }

    public override void Completed(bool clear)
    {
        lock (_lock)
        {
            if (clear)
            {
                _console.Write(_live.RestoreCursor());
            }
            else
            {
                if (_live.HasRenderable && _live.DidOverflow)
                {
                    // Redraw the whole live renderable
                    _console.Write(_live.RestoreCursor());
                    _live.Overflow = VerticalOverflow.Visible;
                    _console.Write(_live.Target);
                }

                _console.WriteLine();
            }

            _console.Cursor.Show();
        }
    }

    public override void Update(ProgressContext context)
    {
        lock (_lock)
        {
            if (!_stopwatch.IsRunning)
            {
                _stopwatch.Start();
            }

            var renderContext = RenderOptions.Create(_console, _console.Profile.Capabilities);

            var delta = _stopwatch.Elapsed - _lastUpdate;
            _lastUpdate = _stopwatch.Elapsed;

            var grid = new Grid();
            for (var columnIndex = 0; columnIndex < _columns.Count; columnIndex++)
            {
                var column = new GridColumn().PadRight(1);

                var columnWidth = _columns[columnIndex].GetColumnWidth(renderContext);
                if (columnWidth != null)
                {
                    column.Width = columnWidth;
                }

                if (_columns[columnIndex].NoWrap)
                {
                    column.NoWrap();
                }

                // Last column?
                if (columnIndex == _columns.Count - 1)
                {
                    column.PadRight(0);
                }

                grid.AddColumn(column);
            }

            // Add rows
            foreach (var task in context.GetTasks().Where(tsk => !(_hideCompleted && tsk.IsFinished)))
            {
                var columns = _columns.Select(column => column.Render(renderContext, task, delta));
                grid.AddRow(columns.ToArray());
            }

            _live.SetRenderable(new Padder(grid, new Padding(0, 1)));
        }
    }

    public override IEnumerable<IRenderable> Process(RenderOptions options, IEnumerable<IRenderable> renderables)
    {
        lock (_lock)
        {
            yield return _live.PositionCursor();

            foreach (var renderable in renderables)
            {
                yield return renderable;
            }

            yield return _live;
        }
    }
}