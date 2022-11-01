namespace Spectre.Console.Rendering;

/// <summary>
/// Represents a render hook.
/// </summary>
public interface IRenderHook
{
    /// <summary>
    /// Processes the specified renderables.
    /// </summary>
    /// <param name="options">The render options.</param>
    /// <param name="renderables">The renderables to process.</param>
    /// <returns>The processed renderables.</returns>
    IEnumerable<IRenderable> Process(RenderOptions options, IEnumerable<IRenderable> renderables);
}