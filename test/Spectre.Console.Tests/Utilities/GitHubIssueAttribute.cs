using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
