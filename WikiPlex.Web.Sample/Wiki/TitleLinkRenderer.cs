using System;
using System.Web.Mvc;
using WikiPlex.Formatting;

namespace WikiPlex.Web.Sample.Wiki
{
    public class TitleLinkRenderer : IRenderer
    {
        private const string LinkFormat = "<a href=\"{0}\">{1}</a>";
        private readonly UrlHelper urlHelper;

        public TitleLinkRenderer(UrlHelper urlHelper)
        {
            this.urlHelper = urlHelper;
        }

        public string Id
        {
            get { return "Wiki Title Link Renderer"; }
        }

        public bool CanExpand(string scopeName)
        {
            return scopeName == WikiScopeName.WikiLink;
        }

        public string Expand(string scopeName, string input, Func<string, string> htmlEncode, Func<string, string> attributeEncode)
        {
            string url = urlHelper.RouteUrl("Default", new { slug = SlugHelper.Generate(input) });
            return string.Format(LinkFormat, attributeEncode(url), htmlEncode(input));
        }
    }
}