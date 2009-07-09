using System;
using System.IO;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Web.UI;
using System.Xml;
using WikiPlex.Common;

namespace WikiPlex.Formatting
{
    public class RssFeedRenderer : IRenderer
    {
        private readonly IXmlDocumentReader rmlDocumentReader;
        private readonly ISyndicationFeedFactory syndicationFeedFactory;

        public RssFeedRenderer()
            : this(new XmlDocumentReaderWrapper(), new SyndicationFeedFactory())
        {
        }

        public RssFeedRenderer(IXmlDocumentReader XmlDocumentReader, ISyndicationFeedFactory syndicationFeedFactory)
        {
            this.rmlDocumentReader = XmlDocumentReader;
            this.syndicationFeedFactory = syndicationFeedFactory;
        }

        public string Id
        {
            get { return "Rss Feed Renderer"; }
        }

        public bool CanExpand(string scopeName)
        {
            return scopeName == ScopeName.RssFeed;
        }

        public string Expand(string scopeName, string input, Func<string, string> htmlEncode, Func<string, string> attributeEncode)
        {
            string[] parameters = input.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            string urlParameter = parameters.FirstOrDefault(s => s.StartsWith("url=", StringComparison.OrdinalIgnoreCase));
            string maxParameter = parameters.FirstOrDefault(s => s.StartsWith("max=", StringComparison.OrdinalIgnoreCase));
            string titlesOnlyParameter = parameters.FirstOrDefault(s => s.StartsWith("titlesonly=", StringComparison.OrdinalIgnoreCase));
            string url;
            int max = 20;
            bool titlesOnly = false;

            if (string.IsNullOrEmpty(urlParameter))
                return RenderUnresolvedMacro("url");

            if (!string.IsNullOrEmpty(maxParameter)
                && (!int.TryParse(maxParameter.Substring(4), out max) || max <= 0 || max > 20))
                return RenderUnresolvedMacro("max");

            if (!string.IsNullOrEmpty(titlesOnlyParameter)
                && !bool.TryParse(titlesOnlyParameter.Substring(11), out titlesOnly))
                return RenderUnresolvedMacro("titlesOnly");

            url = urlParameter.Substring(4);
            try
            {
                var parsedUrl = new Uri(url, UriKind.Absolute);
                url = parsedUrl.AbsoluteUri;
            }
            catch
            {
                return RenderUnresolvedMacro("url");
            }
            
            var content = new StringBuilder();
            using (var tw = new StringWriter(content))
            using (var writer = new HtmlTextWriter(tw, string.Empty))
            {
                writer.NewLine = string.Empty;
                RenderFeed(url, titlesOnly, max, writer);
            }

            return content.ToString();
        }

        protected virtual void RenderFeed(string url, bool titlesOnly, int max, HtmlTextWriter writer)
        {
            XmlDocument xdoc = rmlDocumentReader.Read(url);

            if (xdoc == null)
            {
                writer.Write(RenderUnresolvedMacro("url"));
                return;
            }

            try
            {
                var feed = CreateSyndicationFeed(xdoc);
                if (feed == null)
                {
                    writer.Write(RenderUnresolvedMacro("url"));
                    return;
                }

                writer.AddAttribute(HtmlTextWriterAttribute.Class, "rss");
                writer.RenderBeginTag(HtmlTextWriterTag.Div);

                RenderAccentBar(writer, feed.Title.Text);

                for (int i = 0; i < feed.Items.Count(); i++)
                {
                    if (i >= max)
                        break;

                    var item = feed.Items.ElementAt(i);

                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "entry");
                    writer.RenderBeginTag(HtmlTextWriterTag.Div);
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "title");
                    writer.RenderBeginTag(HtmlTextWriterTag.Div);
                    writer.AddAttribute(HtmlTextWriterAttribute.Href, item.Links[0].Uri.ToString(), false);
                    writer.RenderBeginTag(HtmlTextWriterTag.A);
                    writer.Write(item.Title.Text);
                    writer.RenderEndTag(); //a
                    writer.RenderEndTag(); // div

                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "moreinfo");
                    writer.RenderBeginTag(HtmlTextWriterTag.Div);
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "date");
                    writer.RenderBeginTag(HtmlTextWriterTag.Span);
                    writer.Write(GetPublishDate(item));
                    writer.RenderEndTag(); // span
                    writer.Write(" &nbsp;|&nbsp; ");
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "source");
                    writer.RenderBeginTag(HtmlTextWriterTag.Span);
                    writer.Write("From ");
                    writer.AddAttribute(HtmlTextWriterAttribute.Target, "_blank");
                    writer.AddAttribute(HtmlTextWriterAttribute.Href, url, false);
                    writer.RenderBeginTag(HtmlTextWriterTag.A);
                    writer.Write(feed.Title.Text);
                    writer.RenderEndTag(); // a
                    writer.RenderEndTag(); // span
                    writer.RenderEndTag(); // div

                    if (!titlesOnly)
                    {
                        writer.RenderBeginTag(HtmlTextWriterTag.P);
                        writer.Write(item.Summary.Text);
                        writer.RenderEndTag(); // p
                    }

                    writer.RenderEndTag(); // div
                }

                RenderAccentBar(writer, feed.Title.Text);
                writer.RenderEndTag(); // div
            }
            catch
            {
                writer.Write(RenderUnresolvedMacro("url"));
            }
        }

        private static string GetPublishDate(SyndicationItem item)
        {
            string publishedDate = String.Empty;

            bool hasPublishedDateCategory = false;
            if (item.Categories.Count > 0)
            {
                var categories = item.Categories.ToDictionary(k => k.Name);
                SyndicationCategory category;
                hasPublishedDateCategory = categories.TryGetValue(SyndicationFeedFactory.PublishedDateCategoryName, out category);
                if (hasPublishedDateCategory)
                    publishedDate = category.Label;
            }

            if (!hasPublishedDateCategory)
            {
                publishedDate = item.PublishDate.Date.ToLongDateString();
            }

            return publishedDate;
        }

        private SyndicationFeed CreateSyndicationFeed(XmlDocument xdoc)
        {
            SyndicationFeed feed;
            try
            {
                feed = SyndicationFeed.Load(new XmlNodeReader(xdoc));
            }
            catch
            {
                // Try doing manual syndication feed parsing
                feed = syndicationFeedFactory.Create(xdoc);
            }

            return feed;
        }

        protected static void RenderAccentBar(HtmlTextWriter writer, string title)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "accentbar");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "left");
            writer.RenderBeginTag(HtmlTextWriterTag.Span);
            writer.Write("&nbsp;");
            writer.RenderEndTag(); // span
            writer.Write(title + " News Feed");
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "right");
            writer.RenderBeginTag(HtmlTextWriterTag.Span);
            writer.Write("&nbsp;");
            writer.RenderEndTag(); // span
            writer.RenderEndTag(); // div
        }

        private static string RenderUnresolvedMacro(string parameterName)
        {
            return string.Format("<span class=\"unresolved\">Cannot resolve rss macro, invalid parameter '{0}'.</span>", parameterName);
        }
    }
}