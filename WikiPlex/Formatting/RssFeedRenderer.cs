using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Xml;
using WikiPlex.Common;
using WikiPlex.Syndication;

namespace WikiPlex.Formatting
{
    public class RssFeedRenderer : IRenderer
    {
        private readonly IXmlDocumentReader xmlDocumentReader;
        private readonly ISyndicationReader syndicationReader;

        public RssFeedRenderer()
            : this(new XmlDocumentReaderWrapper(), new SyndicationReader())
        {
        }

        public RssFeedRenderer(IXmlDocumentReader xmlDocumentReader, ISyndicationReader syndicationReader)
        {
            this.xmlDocumentReader = xmlDocumentReader;
            this.syndicationReader = syndicationReader;
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
            XmlDocument xdoc = xmlDocumentReader.Read(url);

            if (xdoc == null)
            {
                writer.Write(RenderUnresolvedMacro("url"));
                return;
            }

            try
            {
                SyndicationFeed feed = syndicationReader.Read(xdoc);

                writer.AddAttribute(HtmlTextWriterAttribute.Class, "rss");
                writer.RenderBeginTag(HtmlTextWriterTag.Div);

                RenderAccentBar(writer, feed.Title);

                for (int i = 0; i < feed.Items.Count(); i++)
                {
                    if (i >= max)
                        break;

                    var item = feed.Items.ElementAt(i);

                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "entry");
                    writer.RenderBeginTag(HtmlTextWriterTag.Div);
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "title");
                    writer.RenderBeginTag(HtmlTextWriterTag.Div);
                    writer.AddAttribute(HtmlTextWriterAttribute.Href, item.Link, false);
                    writer.RenderBeginTag(HtmlTextWriterTag.A);
                    writer.Write(item.Title);
                    writer.RenderEndTag(); //a
                    writer.RenderEndTag(); // div

                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "moreinfo");
                    writer.RenderBeginTag(HtmlTextWriterTag.Div);
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "date");
                    writer.RenderBeginTag(HtmlTextWriterTag.Span);
                    writer.Write(item.Date);
                    writer.RenderEndTag(); // span
                    writer.Write(" &nbsp;|&nbsp; ");
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "source");
                    writer.RenderBeginTag(HtmlTextWriterTag.Span);
                    writer.Write("From ");
                    writer.AddAttribute(HtmlTextWriterAttribute.Target, "_blank");
                    writer.AddAttribute(HtmlTextWriterAttribute.Href, url, false);
                    writer.RenderBeginTag(HtmlTextWriterTag.A);
                    writer.Write(feed.Title);
                    writer.RenderEndTag(); // a
                    writer.RenderEndTag(); // span
                    writer.RenderEndTag(); // div

                    if (!titlesOnly)
                    {
                        writer.RenderBeginTag(HtmlTextWriterTag.P);
                        writer.Write(item.Description);
                        writer.RenderEndTag(); // p
                    }

                    writer.RenderEndTag(); // div
                }

                RenderAccentBar(writer, feed.Title);
                writer.RenderEndTag(); // div
            }
            catch
            {
                writer.Write(RenderUnresolvedMacro("url"));
            }
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