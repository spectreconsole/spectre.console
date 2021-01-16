using System;
using System.IO;
using System.Text;

namespace Spectre.Console.Cli
{
    internal sealed class StringWriterWithEncoding : StringWriter
    {
        public override Encoding Encoding { get; }

        public StringWriterWithEncoding(Encoding encoding)
        {
            Encoding = encoding ?? throw new ArgumentNullException(nameof(encoding));
        }
    }
}
