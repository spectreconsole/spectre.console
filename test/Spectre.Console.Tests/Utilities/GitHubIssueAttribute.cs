namespace Spectre.Console.Tests
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class GitHubIssueAttribute : Attribute
    {
        public int IssueId { get; }

        public GitHubIssueAttribute(int issueId)
        {
            IssueId = issueId;
        }
    }
}