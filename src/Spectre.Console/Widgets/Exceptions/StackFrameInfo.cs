using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace Spectre.Console
{
    internal sealed class StackFrameInfo
    {
        public string Method { get; }
        public List<(string Type, string Name)> Parameters { get; }
        public string? Path { get; }
        public int? LineNumber { get; }

        public StackFrameInfo(
            string method, List<(string Type, string Name)> parameters,
            string? path, int? lineNumber)
        {
            Method = method ?? throw new ArgumentNullException(nameof(method));
            Parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));
            Path = path;
            LineNumber = lineNumber;
        }

        public bool TryGetUri([NotNullWhen(true)] out Uri? result)
        {
            try
            {
                if (Path == null)
                {
                    result = null;
                    return false;
                }

                if (!Uri.TryCreate(Path, UriKind.Absolute, out var uri))
                {
                    result = null;
                    return false;
                }

                if (uri.Scheme == "file")
                {
                    // For local files, we need to append
                    // the host name. Otherwise the terminal
                    // will most probably not allow it.
                    var builder = new UriBuilder(uri)
                    {
                        Host = Dns.GetHostName(),
                    };

                    uri = builder.Uri;
                }

                result = uri;
                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }
    }
}
