using System;
using System.Linq;
using System.Xml;
using WikiPlex.Formatting;
using Xunit;

namespace WikiPlex.Tests.Formatting
{
    public class SyndicationFeedFactoryFacts
    {
        public class The_CreateFromRss_Method
        {
            [Fact]
            public void Should_throw_argument_null_exception_if_document_is_null()
            {
                var factory = new SyndicationFeedFactory();

                var ex = Record.Exception(() => factory.Create(null));

                Assert.NotNull(ex);
                Assert.IsType<ArgumentNullException>(ex);
                Assert.Equal("xdoc", ((ArgumentNullException)ex).ParamName);
            }

            [Fact]
            public void Will_not_parse_the_feed_if_it_is_not_rss()
            {
                var factory = new SyndicationFeedFactory();
                string atom =
                    @"<?xml version='1.0' encoding='utf-8'?>
                        <feed xmlns='http://www.w3.org/2005/Atom' xml:lang='en-us'>
                          <title type='text'>cygweb</title>
                          <subtitle type='text'>blather listen repeat</subtitle>
                          <id>http://alexharden.org/blog/</id>
                          <link rel='alternate' type='application/xhtml+xml' href='http://alexharden.org/blog/' />
                          <link rel='self' type='application/atom+xml' href='http://alexharden.org/blog/atom.xml'/>
                          <author>
                            <name>aharden</name>
                            <uri>http://alexharden.org/blog/</uri>
                            <email>aharden@comcast.net</email>
                          </author>
                          <rights>Creative Commons Attribution 2.5</rights>
                          <generator uri='http://www.sixapart.com/movabletype/' version='3.33'>Movable Type</generator>
                          <icon>http://http://alexharden.org/blog//favicon.ico</icon>
                          <logo>http://http://alexharden.org/blog//cygweb-logo-50-wht.png</logo>
                          <updated>2006-12-16T21:20:35Z</updated>
                        </feed>";

                var xdoc = new XmlDocument();
                xdoc.LoadXml(atom);

                var feed = factory.Create(xdoc);

                Assert.Null(feed);
            }

            [Fact]
            public void Will_parse_rss_feed_successfully()
            {
                var factory = new SyndicationFeedFactory();
                string rss =
                    @"<rss version='2.0'>
                            <channel>
                                <title>Channel Title</title>
                                <item>
                                    <title>Item Title 1</title>
                                    <description>Item Description 1</description>
                                    <link>http://tempuri.org/1</link>
                                    <pubDate>Tue, 24 Mar 2009 01:33:70 GMT</pubDate>
                                </item>
                                <item>
                                    <title>Item Title 2</title>
                                    <link>http://tempuri.org/2</link>
                                    <pubDate>Tue, 24 Mar 2009 01:33:70 GMT</pubDate>
                                </item>
                            </channel>
                        </rss>";

                var xdoc = new XmlDocument();
                xdoc.LoadXml(rss);

                var feed = factory.Create(xdoc);

                Assert.Equal(feed.Title.Text, "Channel Title");
                Assert.Equal(2, feed.Items.Count());
                var items = feed.Items.ToList();
                Assert.Equal(items[0].Title.Text, "Item Title 1");
                Assert.Equal(items[0].Summary.Text, "Item Description 1");
                Assert.Equal(items[0].Links[0].Uri.ToString(), "http://tempuri.org/1");
                Assert.Equal(items[0].Categories.ToDictionary(k => k.Name)[SyndicationFeedFactory.PublishedDateCategoryName].Label, "Tue, 24 Mar 2009 01:33:70 GMT");
                Assert.Equal(items[1].Title.Text, "Item Title 2");
                Assert.Equal(items[1].Summary.Text, String.Empty);
            }
        }
    }
}