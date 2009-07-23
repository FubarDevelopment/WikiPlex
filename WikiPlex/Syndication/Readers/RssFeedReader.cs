using System;
using System.Xml;
using WikiPlex.Common;

namespace WikiPlex.Syndication
{
    public class RssFeedReader : IFeedReader
    {
        private readonly XmlDocument xmlDocument;

        public RssFeedReader(XmlDocument xmlDocument)
        {
            this.xmlDocument = xmlDocument;
        }

        public SyndicationFeed Read()
        {
            Guard.NotNull(xmlDocument, "xmlDocument");
            XmlNodeList channels = xmlDocument.SelectNodes("//rss/channel");

            if (channels.Count == 0)
                throw new ArgumentException("No Channels Found.");

            XmlNode channel = channels[0];
            var feed = new SyndicationFeed
                           {
                               Title = GetValue(channel, "./title"),
                               Link = GetValue(channel, "./link")
                           };

            XmlNodeList items = channel.SelectNodes("//item");
            foreach (XmlNode item in items)
            {
                feed.Items.Add(new SyndicationItem
                                   {
                                       Title = GetValue(item, "./title"),
                                       Description = GetValue(item, "./description"),
                                       Link = GetValue(item, "./link"),
                                       Date = GetValue(item, "./pubDate")
                                   });
            }

            return feed;
        }

        private static string GetValue(XmlNode parent, string xpath)
        {
            XmlNode child = parent.SelectSingleNode(xpath);
            return child == null ? string.Empty : child.InnerText;
        }
    }
}