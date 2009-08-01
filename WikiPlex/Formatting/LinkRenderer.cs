using System;
using WikiPlex.Common;

namespace WikiPlex.Formatting
{
    public class LinkRenderer : IRenderer
    {
        private const string ExternalLinkFormat = "<a href=\"{0}\" class=\"externalLink\">{1}<span class=\"externalLinkIcon\"></span></a>";
        private const string LinkFormat = "<a href=\"{0}\">{1}</a>";

        public string Id
        {
            get { return "LinkFormatting"; }
        }

        public bool CanExpand(string scopeName)
        {
            return (scopeName == ScopeName.LinkNoText
                    || scopeName == ScopeName.LinkWithText
                    || scopeName == ScopeName.LinkAsMailto
                    || scopeName == ScopeName.Anchor
                    || scopeName == ScopeName.LinkToAnchor);
        }

        public string Expand(string scopeName, string input, Func<string, string> htmlEncode, Func<string, string> attributeEncode)
        {
            input = input.Trim();

            if (scopeName == ScopeName.LinkNoText)
            {
                string url = input;
                if (!url.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                    url = "http://" + url;

                return string.Format(ExternalLinkFormat, attributeEncode(url), htmlEncode(input));
            }

            if (scopeName == ScopeName.LinkWithText)
            {
                try
                {
                    TextPart part = Utility.ExtractTextParts(input);
                    string url = part.Text;
                    if (!url.StartsWith("http", StringComparison.OrdinalIgnoreCase) && !url.StartsWith("mailto", StringComparison.OrdinalIgnoreCase))
                        url = "http://" + url;

                    return string.Format(ExternalLinkFormat, attributeEncode(url), htmlEncode(part.FriendlyText));
                }
                catch
                {
                    return RenderUnresolvedMacro();
                }
            }

            if (scopeName == ScopeName.LinkAsMailto)
                return string.Format(ExternalLinkFormat, attributeEncode("mailto:" + input), htmlEncode(input));

            if (scopeName == ScopeName.Anchor)
                return string.Format("<a name=\"{0}\"></a>", attributeEncode(input));

            if (scopeName == ScopeName.LinkToAnchor)
                return string.Format(LinkFormat, attributeEncode("#" + input), htmlEncode(input));

            return input;
        }

        private static string RenderUnresolvedMacro()
        {
            return "<span class=\"unresolved\">Cannot resolve link macro, invalid number of parameters.</span>";
        }
    }
}