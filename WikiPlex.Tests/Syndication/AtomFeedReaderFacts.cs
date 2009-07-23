using System;
using System.Xml;
using WikiPlex.Syndication;
using Xunit;

namespace WikiPlex.Tests.Syndication
{
    public class AtomFeedReaderFacts
    {
        private const string xml = @"<?xml version=""1.0"" encoding=""ISO-8859-1""?>
<feed xmlns=""http://www.w3.org/2005/Atom"">
	<title>AtomSample</title> 
	<link>http://AtomSample.com</link> 
	<entry>
		<title type=""text"">Item 1 Title</title> 
		<link rel=""alternate"">http://item1.com</link> 
		<summary type=""text"">Item 1 Description</summary>
        <updated>2003-12-13T18:30:02Z</updated> 
    </entry>
	<entry>
		<title type=""text"">Item 2 Title</title> 
		<link rel=""alternate"">http://item2.com</link> 
		<summary type=""html"">Item 2 Description</summary> 
        <updated>2003-12-14T18:30:02Z</updated>
	</entry>
</feed>";

        private const string multipleLinkXml = @"<?xml version=""1.0"" encoding=""ISO-8859-1""?>
<feed xmlns=""http://www.w3.org/2005/Atom"">
	<title>AtomSample</title> 
	<link rel=""self"">http://AtomSample.com</link> 
    <link rel=""alternate"">http://NotAtomSample.com</link> 
    <link>http://AlsoNotAtomSample.com</link> 
</feed>";

        private const string encodedXml = @"<?xml version=""1.0"" encoding=""ISO-8859-1""?>
<feed xmlns=""http://www.w3.org/2005/Atom"">
	<title>AtomSample</title> 
	<link>http://AtomSample.com</link> 
	<entry>
		<title type=""text"">Item 1 Title</title> 
		<link rel=""alternate"">http://item1.com</link> 
		<summary type=""text"">&lt;strong&gt;Hello&lt;/strong&gt;</summary>
        <updated>2003-12-13T18:30:02Z</updated> 
    </entry>
	<entry>
		<title type=""text"">Item 2 Title</title> 
		<link rel=""alternate"">http://item2.com</link> 
		<summary type=""html""><![CDATA[<strong>Hello</strong>]]></summary> 
        <updated>2003-12-14T18:30:02Z</updated>
	</entry>
</feed>";

        private const string contentBasedXml = @"<?xml version=""1.0"" encoding=""ISO-8859-1""?>
<feed xmlns=""http://www.w3.org/2005/Atom"">
	<title>AtomSample</title> 
	<link>http://AtomSample.com</link> 
	<entry>
		<title type=""text"">Item 1 Title</title> 
		<link rel=""alternate"">http://item1.com</link> 
        <content type=""html"">The html content</content>
		<summary type=""text"">Item 1 Description</summary>
        <updated>2003-12-13T18:30:02Z</updated> 
    </entry>
	<entry>
		<title type=""text"">Item 2 Title</title> 
		<link rel=""alternate"">http://item2.com</link> 
		<summary type=""html"">Item 2 Description</summary> 
        <updated>2003-12-14T18:30:02Z</updated>
	</entry>
</feed>";

        public class Read
        {
            [Fact]
            public void Will_throw_ArgumentNullException_when_xml_document_is_null()
            {
                var reader = new AtomFeedReader();

                var ex = Record.Exception(() => reader.Read(null));

                Assert.IsType<ArgumentNullException>(ex);
            }

            [Fact]
            public void Will_read_the_feed_info_correctly()
            {
                var reader = new AtomFeedReader();
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xml);

                SyndicationFeed feed = reader.Read(xmlDoc);

                Assert.NotNull(feed);
                Assert.Equal("AtomSample", feed.Title);
                Assert.Equal("http://AtomSample.com", feed.Link);
            }

            [Fact]
            public void Will_read_the_items_correctly()
            {
                var reader = new AtomFeedReader();
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xml);

                SyndicationFeed feed = reader.Read(xmlDoc);

                Assert.Equal(2, feed.Items.Count);
                Assert.Equal("Item 1 Title", feed.Items[0].Title);
                Assert.Equal("Item 1 Description", feed.Items[0].Description);
                Assert.Equal("http://item1.com", feed.Items[0].Link);
                Assert.Equal("2003-12-13T18:30:02Z", feed.Items[0].Date);
                Assert.Equal("Item 2 Title", feed.Items[1].Title);
                Assert.Equal("Item 2 Description", feed.Items[1].Description);
                Assert.Equal("http://item2.com", feed.Items[1].Link);
                Assert.Equal("2003-12-14T18:30:02Z", feed.Items[1].Date);
            }

            [Fact]
            public void Will_read_the_correct_link()
            {
                var reader = new AtomFeedReader();
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(multipleLinkXml);

                SyndicationFeed feed = reader.Read(xmlDoc);

                Assert.Equal("http://AtomSample.com", feed.Link);
            }

            [Fact]
            public void Will_read_the_encoded_content_correctly()
            {
                var reader = new AtomFeedReader();
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(encodedXml);

                SyndicationFeed feed = reader.Read(xmlDoc);

                Assert.Equal(2, feed.Items.Count);
                Assert.Equal("<strong>Hello</strong>", feed.Items[0].Description);
                Assert.Equal("<strong>Hello</strong>", feed.Items[1].Description);
            }

            [Fact]
            public void Will_read_the_content_element_over_summary()
            {
                var reader = new AtomFeedReader();
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(contentBasedXml);

                SyndicationFeed feed = reader.Read(xmlDoc);

                Assert.Equal(2, feed.Items.Count);
                Assert.Equal("The html content", feed.Items[0].Description);
            }
        }
    }
}