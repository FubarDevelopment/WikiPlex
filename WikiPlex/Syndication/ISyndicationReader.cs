using System.Xml;

namespace WikiPlex.Syndication
{
    public interface ISyndicationReader
    {
        SyndicationFeed Read(XmlDocument xmlDocument);
    }
}