using System.Xml;
using WikiPlex.Common;

namespace WikiPlex.Syndication
{
    public class AtomFeedReader : IFeedReader
    {
        private readonly XmlDocument xmlDocument;

        public AtomFeedReader(XmlDocument xmlDocument)
        {
            this.xmlDocument = xmlDocument;
        }

        public SyndicationFeed Read()
        {
            return ReadImpl(xmlDocument);
        }

        protected SyndicationFeed ReadImpl(XmlDocument xmlDocument)
        {
            Guard.NotNull(xmlDocument, "xmlDocument");
            var namespaces = new XmlNamespaceManager(xmlDocument.NameTable);
            namespaces.AddNamespace("x", xmlDocument.DocumentElement.NamespaceURI);

            var feed = new SyndicationFeed
                           {
                               Title = GetValue(xmlDocument.DocumentElement, namespaces, "./x:title"),
                               Link = GetLinkValue(xmlDocument.DocumentElement, namespaces)
                           };

            XmlNodeList items = xmlDocument.DocumentElement.SelectNodes("//x:entry", namespaces);
            foreach (XmlNode item in items)
            {
                feed.Items.Add(new SyndicationItem
                                   {
                                       Title = GetValue(item, namespaces, "./x:title"),
                                       Description = GetDescriptionValue(item, namespaces),
                                       Link = GetValue(item, namespaces, "./x:link[@rel='alternate']"),
                                       Date = GetValue(item, namespaces, "./x:updated")
                                   });
            }

            return feed;
        }

        private static string GetValue(XmlNode parent, XmlNamespaceManager namespaces, string xpath)
        {
            XmlNode child = parent.SelectSingleNode(xpath, namespaces);
            return child == null ? string.Empty : child.InnerText;
        }

        private static string GetLinkValue(XmlNode parent, XmlNamespaceManager namespaces)
        {
            XmlNode selfLink = parent.SelectSingleNode("./x:link[@rel='self']", namespaces);

            if (selfLink != null)
                return selfLink.InnerText;

            XmlNodeList linkNodes = parent.SelectNodes("//x:link", namespaces);
            foreach (XmlElement node in linkNodes)
            {
                if (string.IsNullOrEmpty(node.GetAttribute("rel")))
                    return node.InnerText;
            }

            return string.Empty;
        }

        private static string GetDescriptionValue(XmlNode parent, XmlNamespaceManager namespaces)
        {
            string content = GetValue(parent, namespaces, "./x:content");
            if (!string.IsNullOrEmpty(content))
                return content;

            return GetValue(parent, namespaces, "./x:summary");
        }
    }
}