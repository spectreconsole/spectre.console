namespace Spectre.Console;

internal abstract class ProgressRenderer : IRenderHook
{
    public abstract TimeSpan RefreshRate { get; }

    public virtual void Started()
    {
    }

    public virtual void Completed(bool clear)
    {
    }

    public abstract void Update(ProgressContext context);
    public abstract IEnumerable<IRenderable> Process(RenderOptions options, IEnumerable<IRenderable> renderables);
}