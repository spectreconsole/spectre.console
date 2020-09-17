using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Statiq.Common;

namespace Docs.Modules
{
    public sealed class ReadEmbedded : Module
    {
        private readonly System.Reflection.Assembly _assembly;
        private readonly string _resource;

        public ReadEmbedded(System.Reflection.Assembly assembly, string resource)
        {
            _assembly = assembly ?? throw new ArgumentNullException(nameof(assembly));
            _resource = resource ?? throw new ArgumentNullException(nameof(resource));
        }

        protected override Task<IEnumerable<IDocument>> ExecuteContextAsync(IExecutionContext context)
        {
            return Task.FromResult((IEnumerable<IDocument>)new[]
            {
                context.CreateDocument(ReadResource()),
            });
        }

        private Stream ReadResource()
        {
            var resourceName = _resource.Replace("/", ".");

            var stream = _assembly.GetManifestResourceStream(resourceName);
            if (stream == null)
            {
                throw new InvalidOperationException("Could not load manifest resource stream.");
            }

            return stream;
        }
    }
}