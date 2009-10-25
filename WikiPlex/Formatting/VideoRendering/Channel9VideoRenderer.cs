using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using WikiPlex.Common;

namespace WikiPlex.Formatting
{
    internal class Channel9VideoRenderer : IVideoRenderer
    {
        public Dimensions Dimensions { get; set; }

        public void Render(string url, HtmlTextWriter writer)
        {
            if (Dimensions.Height.Type != UnitType.Pixel)
                throw new InvalidDimensionException("height", " Value can only be pixel based.");
            if (Dimensions.Width.Type != UnitType.Pixel)
                throw new InvalidDimensionException("width", " Value can only be pixel based.");

            var actualUri = new Uri(url);
            url = actualUri.GetLeftPart(UriPartial.Path);

            if (url[url.Length - 1] != '/')
                url += "/";
            if (!url.EndsWith("/player/", StringComparison.OrdinalIgnoreCase))
                url += "player";

            writer.AddAttribute(HtmlTextWriterAttribute.Src, url + "?h=" + Dimensions.Height.Value + "&w=" + Dimensions.Width.Value, false);
            writer.AddAttribute(HtmlTextWriterAttribute.Width, Dimensions.Width.ToString());
            writer.AddAttribute(HtmlTextWriterAttribute.Height, Dimensions.Height.ToString());
            writer.AddAttribute("scrolling", "no");
            writer.AddAttribute("frameborder", "0");
            writer.RenderBeginTag(HtmlTextWriterTag.Iframe);
            writer.RenderEndTag();
        }
    }
}