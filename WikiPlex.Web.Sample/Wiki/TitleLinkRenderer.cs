using System;
using System.Collections.Generic;
using System.Web.Mvc;
using WikiPlex.Formatting;

namespace WikiPlex.Web.Sample.Wiki
{
    public class TitleLinkRenderer : Renderer
    {
        private const string LinkFormat = "<a href=\"{0}\">{1}</a>";
        private readonly UrlHelper urlHelper;

        public TitleLinkRenderer()
        {
        }

        public TitleLinkRenderer(UrlHelper urlHelper)
        {
            this.urlHelper = urlHelper;
        }

        protected override ICollection<string> ScopeNames
        {
            get { return new[] {WikiScopeName.WikiLink}; }
        }

        protected override string ExpandImpl(string scopeName, string input, Func<string, string> htmlEncode, Func<string, string> attributeEncode)
        {
            string url;

            if (urlHelper != null)
                url = urlHelper.RouteUrl("Default", new { slug = SlugHelper.Generate(input) });
            else
                url = "/WebForms/?p=" + SlugHelper.Generate(input);

            return string.Format(LinkFormat, attributeEncode(url), htmlEncode(input));
        }
    }
}