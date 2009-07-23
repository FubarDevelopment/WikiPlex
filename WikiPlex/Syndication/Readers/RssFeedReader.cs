using System.Xml;

namespace WikiPlex.Syndication
{
    public class RssFeedReader : FeedReader
    {
        public RssFeedReader(XmlDocument xmlFeed)
            : base(xmlFeed)
        {
        }

        protected override string NamespacePrefix
        {
            get { return null; }
        }

        protected override XmlNode GetRoot()
        {
            return XmlFeed.DocumentElement.SelectSingleNode("./channel");
        }

        protected override XmlNodeList GetItems(XmlNode root)
        {
            return root.SelectNodes("//item");
        }

        protected override SyndicationFeed CreateFeed(XmlNode root)
        {
            return new SyndicationFeed
                       {
                           Title = GetValue(root, "./title"),
                           Link = GetValue(root, "./link")
                       };
        }

        protected override SyndicationItem CreateFeedItem(XmlNode item)
        {
            return new SyndicationItem
                       {
                           Title = GetValue(item, "./title"),
                           Description = GetValue(item, "./description"),
                           Link = GetValue(item, "./link"),
                           Date = GetValue(item, "./pubDate")
                       };
        }
    }
}