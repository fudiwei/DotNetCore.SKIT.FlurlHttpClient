using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

namespace SKIT.FlurlHttpClient.Tools.DocsTracker.Trackers
{
    public class WeixinWorkEnterpriseTracker : DocsTracker
    {
        public WeixinWorkEnterpriseTracker(DocsTrackerOptions options)
            : base(options)
        {
        }

        protected override Uri GetDocumentationEntrypointUri()
        {
            return new Uri("https://developer.work.weixin.qq.com/document/path/90664");
        }

        protected override Models.Catalog ParseDocumentationCatalog(HtmlDocument htmlDocument)
        {
            IList<Models.Catalog.Types.Section> lstSection = new List<Models.Catalog.Types.Section>();
            HtmlNode tSidebarNode = htmlDocument.DocumentNode.SelectSingleNode("//div[contains(@class, 'sidebar')]");
            HtmlNode[] tOuterLiNodes = tSidebarNode.SelectSingleNode("./ul").SelectNodes("./li").ToArray();
            foreach (HtmlNode tOuterLiNode in tOuterLiNodes)
            {
                HtmlNode tOuterANode = tOuterLiNode.SelectSingleNode("./li/a") ?? tOuterLiNode.SelectSingleNode("./a");
                HtmlNode[] tMiddleLiNodes = tOuterLiNode.SelectNodes("./ul").SelectMany(n => n.SelectNodes("./li")).ToArray();

                foreach (HtmlNode tMiddleLiNode in tOuterLiNodes)
                {
                    HtmlNode tMiddleANode = tMiddleLiNode.SelectSingleNode("./li/a") ?? tMiddleLiNode.SelectSingleNode("./a");

                    string sectionTitle = $"{tOuterANode.InnerText.Trim()}-{tMiddleANode.InnerText.Trim()}";
                    string sectionUri = tMiddleANode.GetAttributeValue("href", string.Empty);
                    if ("true".Equals(tMiddleANode.GetAttributeValue("haschild", string.Empty)))
                    {
                        HtmlNode[] tInnerLiNodes = tMiddleLiNode.SelectNodes("./ul").SelectMany(n => n.SelectNodes("./li")).ToArray();
                        foreach (HtmlNode tInnerNode in tInnerLiNodes)
                        {
                            HtmlNode tInnerANode = tInnerNode.SelectSingleNode("./li/a") ?? tInnerNode.SelectSingleNode("./a");
                            sectionTitle = $"{tOuterANode.InnerText.Trim()}-{tMiddleANode.InnerText.Trim()}-{tInnerANode.InnerText.Trim()}";
                            sectionUri = tInnerANode.GetAttributeValue("href", string.Empty);
                            lstSection.Add(new Models.Catalog.Types.Section(title: sectionTitle, uri: sectionUri));
                        }
                    }
                    else
                    {
                        lstSection.Add(new Models.Catalog.Types.Section(title: sectionTitle, uri: sectionUri));
                    }
                }
            }

            return new Models.Catalog(innerHtml: tSidebarNode.InnerHtml, sections: lstSection.ToArray());
        }

        protected override Models.Content ParseDocumentationContent(Models.Catalog.Types.Section section, HtmlDocument htmlDocument)
        {
            HtmlNode tFrameRightNode = htmlDocument.DocumentNode.SelectSingleNode("//div[contains(@class, 'frame_cntRight')]");
            return new Models.Content(title: section.Title, uri: section.Uri, innerHtml: tFrameRightNode.InnerHtml);
        }
    }
}
