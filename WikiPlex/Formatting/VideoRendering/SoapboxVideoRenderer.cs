using System.Text.RegularExpressions;
using System.Web.UI;

namespace WikiPlex.Formatting.VideoRendering
{
    internal class SoapboxVideoRenderer : IVideoRenderer
    {
        public static readonly string FlashVarsAttributeFormatString = "c=v&v={0}";
        public static readonly string QualityAttributeString = "high";
        public static readonly string SrcAttributeString = "http://images.soapbox.msn.com/flash/soapbox1_1.swf";
        private const string VideoIdRegexPattern = @"^http://\b(soapbox|video)\b\.msn\.com/video\.aspx\?vid=(.+)$";
        public static readonly string WModeAttributeString = "transparent";

        public void AddObjectTagAttributes(string url, HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Height, VideoRendererBase.DefaultVideoHeight);
            writer.AddAttribute(HtmlTextWriterAttribute.Width, VideoRendererBase.DefaultVideoWidth);
        }

        public void AddParameterTags(string url, HtmlTextWriter writer)
        {
            // render nothing
        }

        public void AddEmbedTagAttributes(string url, HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Height, VideoRendererBase.DefaultVideoHeight);
            writer.AddAttribute(HtmlTextWriterAttribute.Width, VideoRendererBase.DefaultVideoWidth);
            writer.AddAttribute(HtmlTextWriterAttribute.Type, FlashVideoRenderer.TypeAttributeString);
            writer.AddAttribute("pluginspage", FlashVideoRenderer.PluginsPageAttributeString);
            writer.AddAttribute("quality", QualityAttributeString);
            writer.AddAttribute("wmode", WModeAttributeString);

            writer.AddAttribute(HtmlTextWriterAttribute.Src, SrcAttributeString);
            writer.AddAttribute("flashvars", string.Format(FlashVarsAttributeFormatString, ExtractVideoIdFromUrl(url)), false);
        }

        private static string ExtractVideoIdFromUrl(string url)
        {
            var videoRegex = new Regex(VideoIdRegexPattern);

            Match match = videoRegex.Match(url);
            if (match.Success && match.Groups.Count > 1)
                return match.Groups[2].Value;

            return string.Empty;
        }
    }
}