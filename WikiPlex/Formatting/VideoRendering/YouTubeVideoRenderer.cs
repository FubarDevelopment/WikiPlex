using System.Text.RegularExpressions;
using System.Web.UI;

namespace WikiPlex.Formatting
{
    internal class YouTubeVideoRenderer : EmbeddedVideoRender
    {
        private static readonly Regex VideoIdRegex = new Regex(@"^http://www\.youtube\.com/watch\?v=(.+)$", RegexOptions.Compiled);
        const string WModeAttributeString = "transparent";
        const string SrcSttributeFormatString = "http://www.youtube.com/v/{0}";

        protected override void AddObjectTagAttributes(string url)
        {
            AddAttribute(HtmlTextWriterAttribute.Height, Dimensions.Height.ToString());
            AddAttribute(HtmlTextWriterAttribute.Width, Dimensions.Width.ToString());
        }

        protected override void AddParameterTags(string url)
        {
            AddParameterTag("movie", string.Format(SrcSttributeFormatString, ExtractVideoIdFromUrl(url)));
            AddParameterTag("wmode", WModeAttributeString);
        }

        protected override void AddEmbedTagAttributes(string url)
        {
            AddAttribute(HtmlTextWriterAttribute.Height, Dimensions.Height.ToString());
            AddAttribute(HtmlTextWriterAttribute.Width, Dimensions.Width.ToString());
            AddAttribute(HtmlTextWriterAttribute.Type, "application/x-shockwave-flash");
            AddAttribute("wmode", WModeAttributeString);

            AddAttribute(HtmlTextWriterAttribute.Src, string.Format(SrcSttributeFormatString, ExtractVideoIdFromUrl(url)));
        }

        private static string ExtractVideoIdFromUrl(string url)
        {
            Match match = VideoIdRegex.Match(url);
            if (match.Success && match.Groups.Count > 1)
                return match.Groups[1].Value;

            return string.Empty;
        }
    }
}