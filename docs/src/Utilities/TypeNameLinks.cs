using System.Collections.Concurrent;

namespace Docs.Utilities;

public class TypeNameLinks
{
    public ConcurrentDictionary<string, string> Links { get; } = new ConcurrentDictionary<string, string>();
}