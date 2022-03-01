using System;

namespace SKIT.FlurlHttpClient.Tools.DocsTracker
{
    public abstract class DocsTracker
    {
        private readonly string _outputPath;
        private readonly string _docCatalogUrl;

        public DocsTracker(DocsTrackerOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            _outputPath = options.OutputPath;
            _docCatalogUrl = options.DocumentationCatalogUrl;
        }

        public void Start()
        {

        }

        protected abstract Uri GetDocumentationCatalogUri();
    }
}
