using System.Xml;
using WikiPlex.Common;

namespace WikiPlex.IntegrationTests
{
    public class LocalXmlReader : IXmlDocumentReader
    {
        public XmlDocument Read(string path)
        {
            if (path.StartsWith("http://local/"))
                path = path.Substring("http://local/".Length);

            var xdoc = new XmlDocument();
            xdoc.Load(path);
            return xdoc;
        }
    }
}