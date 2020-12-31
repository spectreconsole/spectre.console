namespace Spectre.Console.Rendering
{
    public interface ITreeRendering
    {
        string GetPart(TreePart part);
        int PartSize { get; }
    }
}