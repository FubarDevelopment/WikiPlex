using System;
using System.Text.RegularExpressions;
using System.Web.UI;
using WikiPlex.Common;

namespace WikiPlex.Formatting
{
    public class YouTubeVideoRenderer : IVideoRenderer
    {
        private const string VideoIdRegexPattern = @"^http://www\.youtube\.com/watch\?v=(.+)$";

        public static readonly string WModeAttributeString = "transparent";
        public static string SrcSttributeFormatString = "http://www.youtube.com/v/{0}";

        public Dimensions Dimensions { get; set; }

        public void AddObjectTagAttributes(string url, HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Height, Dimensions.Height.ToString());
            writer.AddAttribute(HtmlTextWriterAttribute.Width, Dimensions.Width.ToString());
        }

        public void AddParameterTags(string url, HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Name, "movie");
            writer.AddAttribute(HtmlTextWriterAttribute.Value,
                                string.Format(SrcSttributeFormatString, ExtractVideoIdFromUrl(url)));
            writer.RenderBeginTag(HtmlTextWriterTag.Param);
            writer.RenderEndTag();

            writer.AddAttribute(HtmlTextWriterAttribute.Name, "wmode");
            writer.AddAttribute(HtmlTextWriterAttribute.Value, WModeAttributeString);
            writer.RenderBeginTag(HtmlTextWriterTag.Param);
            writer.RenderEndTag();
        }

        public void AddEmbedTagAttributes(string url, HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Height, Dimensions.Height.ToString());
            writer.AddAttribute(HtmlTextWriterAttribute.Width, Dimensions.Width.ToString());
            writer.AddAttribute(HtmlTextWriterAttribute.Type, FlashVideoRenderer.TypeAttributeString);
            writer.AddAttribute("wmode", WModeAttributeString);

            writer.AddAttribute(HtmlTextWriterAttribute.Src,
                                string.Format(SrcSttributeFormatString, ExtractVideoIdFromUrl(url)));
        }

        private static string ExtractVideoIdFromUrl(string url)
        {
            var videoRegex = new Regex(VideoIdRegexPattern);

            Match match = videoRegex.Match(url);
            if (match.Success && match.Groups.Count > 1)
                return match.Groups[1].Value;

            return string.Empty;
        }
    }
}