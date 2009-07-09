using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using WikiPlex.Common;
using WikiPlex.Formatting.VideoRendering;

namespace WikiPlex.Formatting
{
    public class VideoRenderer : IRenderer
    {
        public string Id
        {
            get { return "Video"; }
        }

        public bool CanExpand(string scopeName)
        {
            return (scopeName == ScopeName.FlashVideo
                    || scopeName == ScopeName.QuickTimeVideo
                    || scopeName == ScopeName.RealPlayerVideo
                    || scopeName == ScopeName.SoapboxVideo
                    || scopeName == ScopeName.WindowsMediaVideo
                    || scopeName == ScopeName.YouTubeVideo
                    || scopeName == ScopeName.InvalidVideo);
        }

        public string Expand(string scopeName, string input, Func<string, string> htmlEncode, Func<string, string> attributeEncode)
        {
            if (scopeName == ScopeName.InvalidVideo)
                return RenderUnresolvedMacro("type");

            string[] parameters = input.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);
            string urlParameter = parameters.FirstOrDefault(s => s.StartsWith("url=", StringComparison.OrdinalIgnoreCase));
            string alignParameter = parameters.FirstOrDefault(s => s.StartsWith("align=", StringComparison.OrdinalIgnoreCase));
            HorizontalAlign align = HorizontalAlign.Center;

            if (string.IsNullOrEmpty(urlParameter))
                return RenderUnresolvedMacro("url");
            if (!string.IsNullOrEmpty(alignParameter)
                && !Utility.IsDefinedOnEnum<HorizontalAlign>(alignParameter.Substring(6)))
                return RenderUnresolvedMacro("align");
            if (!string.IsNullOrEmpty(alignParameter))
            {
                align = (HorizontalAlign) Enum.Parse(typeof (HorizontalAlign), alignParameter.Substring(6), true);
                if (align != HorizontalAlign.Center && align != HorizontalAlign.Left && align != HorizontalAlign.Right)
                    return RenderUnresolvedMacro("align");
            }

            IVideoRenderer videoRenderer = GetVideoRenderer(scopeName);

            string url = urlParameter.Substring(4);
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
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "video");
                writer.AddAttribute(HtmlTextWriterAttribute.Style, string.Format("text-align:{0}", align));
                writer.RenderBeginTag(HtmlTextWriterTag.Div);

                writer.AddAttribute(HtmlTextWriterAttribute.Class, "player");
                writer.RenderBeginTag(HtmlTextWriterTag.Span);

                videoRenderer.AddObjectTagAttributes(url, writer);
                writer.RenderBeginTag(HtmlTextWriterTag.Object);

                videoRenderer.AddParameterTags(url, writer);

                videoRenderer.AddEmbedTagAttributes(url, writer);
                writer.RenderBeginTag(HtmlTextWriterTag.Embed);
                writer.RenderEndTag();

                writer.RenderEndTag(); // </object>
                writer.RenderEndTag(); // </span>

                writer.Write("<br />");

                writer.AddAttribute(HtmlTextWriterAttribute.Class, "external");
                writer.RenderBeginTag(HtmlTextWriterTag.Span);
                writer.AddAttribute(HtmlTextWriterAttribute.Href, url);
                writer.AddAttribute(HtmlTextWriterAttribute.Target, "_blank");
                writer.RenderBeginTag(HtmlTextWriterTag.A);
                writer.Write("Launch in another window");
                writer.RenderEndTag();
                writer.RenderEndTag();

                writer.RenderEndTag(); // </div>
            }

            return content.ToString();
        }

        private static IVideoRenderer GetVideoRenderer(string scopeName)
        {
            IVideoRenderer videoRenderer = null;

            switch (scopeName)
            {
                case ScopeName.FlashVideo:
                    videoRenderer = new FlashVideoRenderer();
                    break;
                case ScopeName.QuickTimeVideo:
                    videoRenderer = new QuickTimeVideoRenderer();
                    break;
                case ScopeName.RealPlayerVideo:
                    videoRenderer = new RealPlayerVideoRenderer();
                    break;
                case ScopeName.SoapboxVideo:
                    videoRenderer = new SoapboxVideoRenderer();
                    break;
                case ScopeName.WindowsMediaVideo:
                    videoRenderer = new WindowsMediaPlayerVideoRenderer();
                    break;
                case ScopeName.YouTubeVideo:
                    videoRenderer = new YouTubeVideoRenderer();
                    break;
            }
            return videoRenderer;
        }

        private static string RenderUnresolvedMacro(string parameterName)
        {
            return string.Format("<span class=\"unresolved\">Cannot resolve video macro, invalid parameter '{0}'.</span>", parameterName);
        }
    }
}