namespace Spectre.Console.Tests;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public sealed class GitHubIssueAttribute : Attribute
{
    public string Url { get; }

    public GitHubIssueAttribute(string url)
    {
        Url = url;
    }
}