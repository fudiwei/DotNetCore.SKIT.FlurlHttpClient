using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

namespace SKIT.FlurlHttpClient.Tools.DocsTracker.Trackers
{
    public class WeixinPayCommonTracker : DocsTracker
    {
        public WeixinPayCommonTracker(DocsTrackerOptions options)
            : base(options)
        {
        }

        protected override Uri GetDocumentationEntrypointUri()
        {
            return new Uri("https://pay.weixin.qq.com/wiki/doc/apiv3/apis/index.shtml");
        }

        protected override Models.Catalog ParseDocumentationCatalog(HtmlDocument htmlDocument)
        {
            IList<Models.Catalog.Types.Section> lstSection = new List<Models.Catalog.Types.Section>();
            HtmlNode tSideNode = htmlDocument.DocumentNode.SelectSingleNode("//div[contains(@class, 'doc-side')]");
            HtmlNode[] tDlNodes = tSideNode.SelectNodes(".//dl[contains(@class, 'doc-dl')]")?.ToArray() ?? Array.Empty<HtmlNode>();
            foreach (HtmlNode tDlNode in tDlNodes)
            {
                HtmlNode tDtNode = tDlNode.SelectSingleNode(".//dt");
                HtmlNode tDdANode = tDlNode.SelectSingleNode(".//dd").SelectSingleNode(".//a");

                string sectionTitle = $"{tDtNode.InnerText.Trim()}-{tDdANode.InnerText.Trim()}";
                string sectionUri = tDlNode.GetAttributeValue("href", string.Empty);
                if (string.IsNullOrEmpty(sectionUri) || sectionUri.StartsWith("javascript:"))
                {
                    HtmlNode[] tLiANodes = tDlNode.SelectNodes(".//li").SelectMany(n => n.SelectNodes(".//a")).ToArray();
                    foreach (HtmlNode tLiANode in tLiANodes)
                    {
                        sectionTitle = $"{tDtNode.InnerText.Trim()}-{tDdANode.InnerText.Trim()}-{tLiANode.InnerText.Trim()}";
                        sectionUri = tLiANode.GetAttributeValue("href", string.Empty);
                        if (!sectionUri.Contains("/index.shtml"))
                        {
                            lstSection.Add(new Models.Catalog.Types.Section(title: sectionTitle, uri: sectionUri));
                        }
                    }
                }
                else
                {
                    lstSection.Add(new Models.Catalog.Types.Section(title: sectionTitle, uri: sectionUri));
                }
            }

            return new Models.Catalog(innerHtml: tSideNode.InnerHtml, sections: lstSection.ToArray());
        }

        protected override Models.Content ParseDocumentationContent(Models.Catalog.Types.Section section, HtmlDocument htmlDocument)
        {
            HtmlNode tDocBoxNode = htmlDocument.DocumentNode.SelectSingleNode("//div[contains(@class, 'doc-box')]");
            tDocBoxNode.SelectSingleNode(".//div[contains(@class, 'doc-menu')]")?.Remove();
            tDocBoxNode.SelectSingleNode(".//div[contains(@class, 'doc-side')]")?.Remove();
            return new Models.Content(title: section.Title, uri: section.Uri, innerHtml: tDocBoxNode.InnerHtml);
        }
    }
}
