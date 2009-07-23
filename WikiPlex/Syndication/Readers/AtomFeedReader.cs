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
                       };
        }

        protected override SyndicationItem CreateFeedItem(XmlNode item)
        {
            return new SyndicationItem
                       {
                           Title = GetValue(item, "./atom:title"),
                           Description = GetDescriptionValue(item),
                           Link = GetItemLink(item),
                           Date = new SyndicationDate(GetItemDate(item))
                       };
        }

        private string GetItemLink(XmlNode item)
        {
            XmlNode link = item.SelectSingleNode("./atom:link[@rel='alternate']", Namespaces);
            if (link == null)
                return string.Empty;

            return link.Attributes.GetNamedItem("href").InnerText;
        }

        protected virtual string GetItemDate(XmlNode item)
        {
            return GetValue(item, "./atom:updated");
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