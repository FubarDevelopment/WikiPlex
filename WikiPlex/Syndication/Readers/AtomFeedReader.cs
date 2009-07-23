using System.Xml;

namespace WikiPlex.Syndication
{
    public class AtomFeedReader : FeedReader
    {
        public AtomFeedReader(XmlDocument xmlFeed)
            : base(xmlFeed)
        {
        }

        protected override string NamespacePrefix
        {
            get { return "atom"; }
        }

        protected override XmlNode GetRoot()
        {
            return XmlFeed.DocumentElement;
        }

        protected override XmlNodeList GetItems(XmlNode root)
        {
            return root.SelectNodes("//atom:entry", Namespaces);
        }

        protected override SyndicationFeed CreateFeed(XmlNode root)
        {
            return new SyndicationFeed
                       {
                           Title = GetValue(root, "./atom:title"),
                           Link = GetRootLink(root)
                       };
        }

        protected override SyndicationItem CreateFeedItem(XmlNode item)
        {
            return new SyndicationItem
                       {
                           Title = GetValue(item, "./atom:title"),
                           Description = GetDescriptionValue(item),
                           Link = GetValue(item, "./atom:link[@rel='alternate']"),
                           Date = GetValue(item, "./atom:updated")
                       };
        }

        private string GetRootLink(XmlNode root)
        {
            XmlNode selfLink = root.SelectSingleNode("./atom:link[@rel='self']", Namespaces);

            if (selfLink != null)
                return selfLink.InnerText;

            XmlNodeList linkNodes = root.SelectNodes("//atom:link", Namespaces);
            foreach (XmlElement node in linkNodes)
            {
                if (string.IsNullOrEmpty(node.GetAttribute("rel")))
                    return node.InnerText;
            }

            return string.Empty;
        }

        private string GetDescriptionValue(XmlNode parent)
        {
            string content = GetValue(parent, "./atom:content");
            if (!string.IsNullOrEmpty(content))
                return content;

            return GetValue(parent, "./atom:summary");
        }
    }
}