using System;

namespace SKIT.FlurlHttpClient.Tools.DocsTracker.Models
{
    public class Catalog
    {
        public static class Types
        {
            public class Section
            {
                public string Title { get; }

                public string Uri { get; }

                public Section(string title, string uri)
                {
                    Title = title;
                    Uri = uri;
                }
            }
        }

        public string InnerHtml { get; }

        public Types.Section[] Sections { get; }

        public Catalog(string innerHtml)
            : this(innerHtml, Array.Empty<Types.Section>())
        {
        }

        public Catalog(string innerHtml, Types.Section[] sections)
        {
            InnerHtml = innerHtml;
            Sections = sections;
        }
    }
}
