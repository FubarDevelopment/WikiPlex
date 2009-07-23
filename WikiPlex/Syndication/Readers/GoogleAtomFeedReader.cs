using System.Xml;

namespace WikiPlex.Syndication
{
    public class GoogleAtomFeedReader : AtomFeedReader
    {
        public GoogleAtomFeedReader(XmlDocument xmlFeed)
            : base(xmlFeed)
        {
        }

        protected override string GetItemDate(XmlNode item)
        {
            return GetValue(item, "./atom:modified");
        }
    }
}