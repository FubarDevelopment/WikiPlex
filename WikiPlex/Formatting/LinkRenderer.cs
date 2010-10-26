using System;
using WikiPlex.Common;

namespace WikiPlex.Formatting
{
    /// <summary>
    /// Will render all link based scopes.
    /// </summary>
    public class LinkRenderer : RendererBase
    {
        private const string ExternalLinkFormat = "<a href=\"{0}\" class=\"externalLink\">{1}<span class=\"externalLinkIcon\"></span></a>";
        private const string LinkFormat = "<a href=\"{0}\">{1}</a>";

        /// <summary>
        /// Creates a new instance of the <see cref="LinkRenderer"/> class.
        /// </summary>
        public LinkRenderer()
            : base(ScopeName.LinkNoText, ScopeName.LinkWithText, ScopeName.LinkAsMailto, 
                   ScopeName.Anchor, ScopeName.LinkToAnchor)
        {}

        /// <summary>
        /// Gets the invalid macro error text.
        /// </summary>
        public override string InvalidMacroError
        {
            get { return "Cannot resolve link macro, invalid number of parameters."; }
        }

        /// <summary>
        /// Will expand the input into the appropriate content based on scope.
        /// </summary>
        /// <param name="scopeName">The scope name.</param>
        /// <param name="input">The input to be expanded.</param>
        /// <param name="htmlEncode">Function that will html encode the output.</param>
        /// <param name="attributeEncode">Function that will html attribute encode the output.</param>
        /// <returns>The expanded content.</returns>
        protected override string ExpandImpl(string scopeName, string input, Func<string, string> htmlEncode, Func<string, string> attributeEncode)
        {
            input = input.Trim();

            try
            {
                if (scopeName == ScopeName.LinkNoText)
                    return ExpandLinkNoText(input, attributeEncode, htmlEncode);
                if (scopeName == ScopeName.LinkWithText)
                    return ExpandLinkWithText(input, attributeEncode, htmlEncode);
                if (scopeName == ScopeName.LinkAsMailto)
                    return string.Format(ExternalLinkFormat, attributeEncode("mailto:" + input), htmlEncode(input));
                if (scopeName == ScopeName.Anchor)
                    return string.Format("<a name=\"{0}\"></a>", attributeEncode(input));
                if (scopeName == ScopeName.LinkToAnchor)
                    return string.Format(LinkFormat, attributeEncode("#" + input), htmlEncode(input));
            }
            catch (ArgumentException)
            {
                throw new RenderException();
            }

            return null;
        }

        private static string ExpandLinkWithText(string input, Func<string, string> attributeEncode, Func<string, string> htmlEncode)
        {
            TextPart part = Utility.ExtractTextParts(input);
            string url = part.Text;
            if (!url.StartsWith("http", StringComparison.OrdinalIgnoreCase) && !url.StartsWith("mailto", StringComparison.OrdinalIgnoreCase))
                url = "http://" + url;

            return string.Format(ExternalLinkFormat, attributeEncode(url), htmlEncode(part.FriendlyText));
        }

        private static string ExpandLinkNoText(string input, Func<string, string> attributeEncode, Func<string, string> htmlEncode)
        {
            string url = input;
            if (!url.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                url = "http://" + url;

            return string.Format(ExternalLinkFormat, attributeEncode(url), htmlEncode(input));
        }
    }
}