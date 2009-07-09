using System.ServiceModel.Syndication;
using System.Xml;

namespace WikiPlex.Formatting
{
    public interface ISyndicationFeedFactory
    {
        SyndicationFeed Create(XmlDocument xdoc);
    }
}