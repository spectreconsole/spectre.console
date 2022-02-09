using System.Collections.Concurrent;
using Buildalyzer;
using Buildalyzer.Workspaces;
using Microsoft.CodeAnalysis;

namespace Docs.Utilities;

public class SolutionWorkspaceProvider
{
    private static readonly ConcurrentDictionary<string, AdhocWorkspace> _workspaceCache = new();

    public AdhocWorkspace Get(string solutionFile)
    {
        return _workspaceCache.GetOrAdd(solutionFile, s =>
        {
            var analyzerManager = new AnalyzerManager(s, new AnalyzerManagerOptions());

            var workspace = new AdhocWorkspace();
            if (!string.IsNullOrEmpty(analyzerManager.SolutionFilePath))
            {
                var solutionInfo = SolutionInfo.Create(
                    SolutionId.CreateNewId(),
                    VersionStamp.Default,
                    analyzerManager.SolutionFilePath);

                workspace.AddSolution(solutionInfo);
            }

            foreach (var analyzerResult in analyzerManager.Projects.Values)
            {
                analyzerResult.AddToWorkspace(workspace, false);
            }

            return workspace;
        });
    }
}