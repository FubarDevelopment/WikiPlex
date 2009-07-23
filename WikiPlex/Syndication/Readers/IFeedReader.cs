using System.Xml;

namespace WikiPlex.Syndication
{
    public interface IFeedReader
    {
        SyndicationFeed Read(XmlDocument xmlDocument);
    }
}