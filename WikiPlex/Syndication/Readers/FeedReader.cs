using System;
using System.Xml;
using WikiPlex.Common;

namespace WikiPlex.Syndication
{
    public abstract class FeedReader : IFeedReader
    {
        protected FeedReader(XmlDocument xmlFeed)
        {
            XmlFeed = xmlFeed;
        }

        protected XmlDocument XmlFeed { get; private set; }
        protected XmlNamespaceManager Namespaces { get; private set; }
        protected abstract string NamespacePrefix { get; }

        public SyndicationFeed Read()
        {
            Guard.NotNull(XmlFeed, "xmlFeed");

            XmlNode root = GetRoot();
            if (root == null)
                throw new ArgumentException("Feed Root Not Found.");

            SetupNamespaces(root);
            SyndicationFeed feed = CreateFeed(root);

            XmlNodeList items = GetItems(root);
            foreach (XmlNode item in items)
                feed.Items.Add(CreateFeedItem(item));

            return feed;
        }

        protected abstract XmlNode GetRoot();
        protected abstract XmlNodeList GetItems(XmlNode root);
        protected abstract SyndicationFeed CreateFeed(XmlNode root);
        protected abstract SyndicationItem CreateFeedItem(XmlNode item);

        protected string GetValue(XmlNode parent, string xpath)
        {
            XmlNode child = parent.SelectSingleNode(xpath, Namespaces);
            return child == null ? string.Empty : child.InnerText;
        }

        private void SetupNamespaces(XmlNode root)
        {
            if (!string.IsNullOrEmpty(root.NamespaceURI))
            {
                Namespaces = new XmlNamespaceManager(XmlFeed.NameTable);
                Namespaces.AddNamespace(NamespacePrefix, root.NamespaceURI);
            }
        }
    }
}