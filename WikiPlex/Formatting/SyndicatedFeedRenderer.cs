using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Xml;
using WikiPlex.Syndication;

namespace WikiPlex.Formatting
{
    /// <summary>
    /// Will render the syndicated feed scopes.
    /// </summary>
    public class SyndicatedFeedRenderer : IRenderer
    {
        private readonly IXmlDocumentReader xmlDocumentReader;
        private readonly ISyndicationReader syndicationReader;

        /// <summary>
        /// Initializes a new instance of the <see cref="SyndicatedFeedRenderer"/>.
        /// </summary>
        public SyndicatedFeedRenderer()
            : this(new XmlDocumentReaderWrapper(), new SyndicationReader())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SyndicatedFeedRenderer"/>.
        /// </summary>
        /// <param name="xmlDocumentReader">The xml document reader.</param>
        /// <param name="syndicationReader">The syndication reader.</param>
        public SyndicatedFeedRenderer(IXmlDocumentReader xmlDocumentReader, ISyndicationReader syndicationReader)
        {
            this.xmlDocumentReader = xmlDocumentReader;
            this.syndicationReader = syndicationReader;
        }

        /// <summary>
        /// Gets the id of a renderer.
        /// </summary>
        public string Id
        {
            get { return "Syndicated Feed Renderer"; }
        }

        /// <summary>
        /// Determines if this renderer can expand the given scope name.
        /// </summary>
        /// <param name="scopeName">The scope name to check.</param>
        /// <returns>A boolean value indicating if the renderer can or cannot expand the macro.</returns>
        public bool CanExpand(string scopeName)
        {
            return scopeName == ScopeName.SyndicatedFeed;
        }

        /// <summary>
        /// Will expand the input into the appropriate content based on scope.
        /// </summary>
        /// <param name="scopeName">The scope name.</param>
        /// <param name="input">The input to be expanded.</param>
        /// <param name="htmlEncode">Function that will html encode the output.</param>
        /// <param name="attributeEncode">Function that will html attribute encode the output.</param>
        /// <returns>The expanded content.</returns>
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

        /// <summary>
        /// Handles rendering a feed.
        /// </summary>
        /// <param name="url">The url to read the feed from.</param>
        /// <param name="titlesOnly">Indicates if only titles should be displayed.</param>
        /// <param name="max">The maximum number of entries to display.</param>
        /// <param name="writer">The text writer to write to.</param>
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

        /// <summary>
        /// Handles rendering the accent bar.
        /// </summary>
        /// <param name="writer">The text writer to write to.</param>
        /// <param name="title">The title to write.</param>
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
            return string.Format("<span class=\"unresolved\">Cannot resolve syndicated feed macro, invalid parameter '{0}'.</span>", parameterName);
        }
    }
}