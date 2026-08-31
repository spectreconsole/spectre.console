namespace Spectre.Console.Ansi;

internal static class OscHyperLinkParser
{
    public static OscCommand? Parse(ReadOnlySpan<char> buffer)
    {
        string? id = null;

        // The URI is everything after the first ';'. Per the OSC 8 spec the params
        // section precedes it, and the URI itself may legally contain ';'
        var separator = buffer.IndexOf(';');
        if (separator < 0)
        {
            return new OscCommand.HyperLinkStart(null, buffer.ToString());
        }

        var parameters = buffer[..separator];
        var uri = buffer[(separator + 1)..];

        // Params are a ':'-separated list of key=value pairs.
        while (!parameters.IsEmpty)
        {
            var colonIndex = parameters.IndexOf(':');
            var pair = colonIndex < 0 ? parameters : parameters[..colonIndex];

            var equalsIndex = pair.IndexOf('=');
            if (equalsIndex >= 0 && pair[..equalsIndex].Trim() is "id")
            {
                var value = pair[(equalsIndex + 1)..].Trim();
                id = value.IsEmpty ? null : value.ToString();
            }

            if (colonIndex < 0)
            {
                break;
            }

            parameters = parameters[(colonIndex + 1)..];
        }

        if (id == null && uri.Length == 0)
        {
            return new OscCommand.HyperLinkEnd();
        }

        return uri.Length != 0
            ? new OscCommand.HyperLinkStart(Id: id, Uri: uri.ToString())
            : null;
    }
}