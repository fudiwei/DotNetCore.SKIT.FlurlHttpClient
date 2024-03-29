﻿using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

namespace SKIT.FlurlHttpClient.Tools.DocsTracker.Trackers
{
    public class WeixinMediaPlatformTracker : DocsTracker
    {
        public WeixinMediaPlatformTracker(DocsTrackerOptions options)
            : base(options)
        {
        }

        protected override Uri GetDocumentationEntrypointUri()
        {
            return new Uri("https://developers.weixin.qq.com/doc/offiaccount/Getting_Started/Overview.html");
        }

        protected override Models.Catalog ParseDocumentationCatalog(HtmlDocument htmlDocument)
        {
            IList<Models.Catalog.Types.Section> lstSection = new List<Models.Catalog.Types.Section>();
            HtmlNode tAsideNode = htmlDocument.DocumentNode.SelectSingleNode("//aside");
            HtmlNode[] tParentNodes = tAsideNode.SelectNodes(".//div[contains(@class, 'NavigationLevel--level-1')]")?.ToArray() ?? Array.Empty<HtmlNode>();
            foreach (HtmlNode tParentNode in tParentNodes)
            {
                HtmlNode tParentItemNode = tParentNode
                    .SelectSingleNode(".//div[contains(@class, 'NavigationLevel__parent')]")
                    .SelectSingleNode(".//span[contains(@class, 'NavigationItem')]");
                HtmlNode[] tChildItemNodes = tParentNode
                    .SelectSingleNode(".//ul[contains(@class, 'NavigationLevel__children')]")
                    .SelectNodes(".//span[contains(@class, 'NavigationItem')]")
                    .ToArray();
                foreach (HtmlNode tChildItemNode in tChildItemNodes)
                {
                    string sectionTitle = $"{tParentItemNode.InnerText.Trim()}-{tChildItemNode.InnerText.Trim()}";
                    string sectionUri = tChildItemNode.SelectSingleNode(".//a").GetAttributeValue("href", string.Empty);
                    lstSection.Add(new Models.Catalog.Types.Section(title: sectionTitle, uri: sectionUri));
                }
            }

            return new Models.Catalog(innerHtml: tAsideNode.InnerHtml, sections: lstSection.ToArray());
        }

        protected override Models.Content ParseDocumentationContent(Models.Catalog.Types.Section section, HtmlDocument htmlDocument)
        {
            HtmlNode tContentNode = htmlDocument.DocumentNode.SelectSingleNode("//div[@id='docContent']");
            return new Models.Content(title: section.Title, uri: section.Uri, innerHtml: tContentNode.InnerHtml);
        }
    }
}
