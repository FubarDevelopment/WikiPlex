using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using Xunit;
using Xunit.Extensions;
using WikiPlex.Common;

namespace WikiPlex.IntegrationTests
{
    public class XmlLoaderFacts
    {
        public class Load
        {
            [Theory]
            [InlineData("Atom")]
            [InlineData("GoogleAtom")]
            [InlineData("Rss")]
            public void Will_return_the_xml_document_with_the_xml_from_the_path_specified(string xmlFeed)
            {
                string baseDirectory = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Data\\RssFeedFormatting");

                string[] files = Directory.GetFiles(baseDirectory, xmlFeed + ".xml");
                if (files.Count() != 1)
                    Assert.False(true);
                var loader = new XmlDocumentReaderWrapper();
                var expected = new XmlDocument();
                expected.Load(files[0]);

                XmlDocument xdoc = loader.Read(files[0]);

                Assert.NotNull(xdoc);
                Assert.Equal(expected.OuterXml, xdoc.OuterXml);
            }

            [Theory]
            [InlineData("does not exist")]
            [InlineData("http://doesnotexist")]
            public void Will_return_null_if_feed_does_not_exist_at_path(string path)
            {
                var loader = new XmlDocumentReaderWrapper();

                XmlDocument xdoc = loader.Read(path);

                Assert.Null(xdoc);
            }
        }
    }
}