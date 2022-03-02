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

        protected override Uri GetDocumentationUri()
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
                string sectionUri = tDdANode.GetAttributeValue("href", string.Empty);
                if (string.IsNullOrEmpty(sectionUri) || sectionUri.StartsWith("javascript:"))
                {
                    HtmlNode[] tLiANodes = tDlNode.SelectNodes(".//li").SelectMany(n => n.SelectNodes(".//a")).ToArray();
                    foreach (HtmlNode tLiANode in tLiANodes)
                    {
                        sectionTitle = sectionTitle + $"-{tLiANode.InnerText.Trim()}";
                        sectionUri = tLiANode.GetAttributeValue("href", string.Empty);
                        if (sectionUri.Contains("/index.shtml"))
                            continue;

                        lstSection.Add(new Models.Catalog.Types.Section(title: sectionTitle, uri: sectionUri));
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
            HtmlNode tDocMainNode = htmlDocument.DocumentNode.SelectSingleNode("//div[contains(@class, 'doc-main')]");
            return new Models.Content(title: section.Title, uri: section.Uri, innerHtml: tDocMainNode.InnerHtml);
        }
    }
}
