using System;

namespace Spectre.Console.Tests
{
    public sealed class GitHubIssueAttribute : Attribute
    {
        public int IssueId { get; }

        public GitHubIssueAttribute(int issueId)
        {
            IssueId = issueId;
        }
    }
}