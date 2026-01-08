namespace Spectre.Console.Rendering;

/// <summary>
/// Represents something renderable that's reconstructed
/// when its state change in any way.
/// </summary>
public abstract class JustInTimeRenderable : Renderable
{
    private bool _dirty;
    private IRenderable? _rendered;

    /// <inheritdoc/>
    protected sealed override Measurement Measure(RenderOptions context, int maxWidth)
    {
        return GetInner().Measure(context, maxWidth);
    }

    /// <inheritdoc/>
    protected sealed override IEnumerable<Segment> Render(RenderOptions context, int width)
    {
        return GetInner().Render(context, width);
    }

    /// <summary>
    /// Builds the inner renderable.
    /// </summary>
    /// <returns>A new inner renderable.</returns>
    protected abstract IRenderable Build();

    /// <summary>
    /// Checks if there are any children that has changed.
    /// If so, the underlying renderable needs rebuilding.
    /// </summary>
    /// <returns><c>true</c> if the object needs rebuilding, otherwise <c>false</c>.</returns>
    protected virtual bool HasDirtyChildren()
    {
        return false;
    }

    /// <summary>
    /// Marks this instance as dirty.
    /// </summary>
    protected void MarkAsDirty()
    {
        _dirty = true;
    }

    /// <summary>
    /// Marks this instance as dirty.
    /// </summary>
    /// <param name="action">
    /// The action to execute before marking the instance as dirty.
    /// </param>
    protected void MarkAsDirty(Action action)
    {
        if (action is null)
        {
            throw new ArgumentNullException(nameof(action));
        }

        action();
        _dirty = true;
    }

    private IRenderable GetInner()
    {
        if (_dirty || HasDirtyChildren() || _rendered == null)
        {
            _rendered = Build();
            _dirty = false;
        }

        return _rendered;
    }
}