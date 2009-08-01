using System.Xml;

namespace WikiPlex.Syndication
{
    public interface IXmlDocumentReader
    {
        XmlDocument Read(string path);
    }
}