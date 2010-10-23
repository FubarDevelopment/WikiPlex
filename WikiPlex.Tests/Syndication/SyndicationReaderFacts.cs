using System;
using System.Xml;
using WikiPlex.Syndication;
using Xunit;

namespace WikiPlex.Tests.Syndication
{
    public class SyndicationReaderFacts
    {
        public class Read
        {
            [Fact]
            public void Should_throw_ArgumentNullException_when_xml_document_is_null()
            {
                var factory = new SyndicationReader();

                Exception ex = Record.Exception(() => factory.Read(null));

                Assert.IsType<ArgumentNullException>(ex);
            }

            [Fact]
            public void Should_return_feed_when_document_element_is_rss()
            {
                var factory = new SyndicationReader();
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(@"<?xml version=""1.0""?><rss version=""0.91""><channel><title>RSS</title></channel></rss>");

                SyndicationFeed feed = factory.Read(xmlDoc);

                Assert.Equal("RSS", feed.Title);
            }

            [Fact]
            public void Should_return_feed_when_document_element_is_feed_and_correct_xmlns()
            {
                var factory = new SyndicationReader();
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(@"<?xml version=""1.0""?><feed xmlns=""http://www.w3.org/2005/Atom""><title>Atom</title></feed>");

                SyndicationFeed feed = factory.Read(xmlDoc);

                Assert.Equal("Atom", feed.Title);
            }

            [Fact]
            public void Should_return_feed_when_document_element_is_feed_and_correct_xmlns_for_google_atom()
            {
                var factory = new SyndicationReader();
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(@"<?xml version=""1.0""?><feed xmlns=""http://purl.org/atom/ns#""><title>Google Atom</title></feed>");

                SyndicationFeed feed = factory.Read(xmlDoc);

                Assert.Equal("Google Atom", feed.Title);
            }

            [Fact]
            public void Should_throw_ArgumentException_when_document_element_is_unsupported()
            {
                var factory = new SyndicationReader();
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(@"<?xml version=""1.0""?><something />");

                Exception ex = Record.Exception(() => factory.Read(xmlDoc));

                Assert.IsType<ArgumentException>(ex);
            }
        }
    }
}