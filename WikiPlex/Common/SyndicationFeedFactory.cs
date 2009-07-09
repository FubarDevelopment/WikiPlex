using System;
using System.Collections.Generic;
using System.ServiceModel.Syndication;
using System.Xml;
using WikiPlex.Common;

namespace WikiPlex.Formatting
{
    public class SyndicationFeedFactory : ISyndicationFeedFactory
    {
        public const string PublishedDateCategoryName = "PublishedDate";

        public SyndicationFeed Create(XmlDocument xdoc)
        {
            Guard.NotNull(xdoc, "xdoc");

            if (string.Compare(xdoc.DocumentElement.Name, "rss", StringComparison.OrdinalIgnoreCase) != 0)
                return null;

            var syndicationFeed = new SyndicationFeed();
            var channelElement = xdoc.SelectSingleNode("/rss/channel");
            syndicationFeed.Title = new TextSyndicationContent(ParseOptionalValue(channelElement.SelectSingleNode("./title")));

            var items = channelElement.SelectNodes(".//item");
            var syndItems = new List<SyndicationItem>();
            foreach (XmlNode item in items)
                syndItems.Add(CreateSyndicationItem(item));

            syndicationFeed.Items = syndItems;
            return syndicationFeed;
        }

        private static SyndicationItem CreateSyndicationItem(XmlNode itemElement)
        {
            var syndicationItem = new SyndicationItem
                                      {
                                          Title = new TextSyndicationContent(ParseOptionalValue(itemElement.SelectSingleNode("./title"))),
                                          Summary = new TextSyndicationContent(ParseOptionalValue(itemElement.SelectSingleNode("./description"))),
                                      };
            syndicationItem.Links.Add(new SyndicationLink { Uri = new Uri(itemElement.SelectSingleNode("./link").InnerText) });

            // HACK: We store publish date in a category since parsing dates is problematic.
            syndicationItem.Categories.Add(new SyndicationCategory(PublishedDateCategoryName, "http://tempuri.org/", itemElement.SelectSingleNode("./pubDate").InnerText));
            return syndicationItem;
        }

        private static string ParseOptionalValue(XmlNode xElement)
        {
            return xElement == null ? String.Empty : xElement.InnerText;
        }
    }
}