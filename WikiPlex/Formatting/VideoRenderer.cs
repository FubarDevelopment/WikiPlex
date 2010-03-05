using System;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using WikiPlex.Common;

namespace WikiPlex.Formatting
{
    /// <summary>
    /// Will render all the video scopes.
    /// </summary>
    public class VideoRenderer : IRenderer
    {
        /// <summary>
        /// Gets the id of a renderer.
        /// </summary>
        public string Id
        {
            get { return "Video"; }
        }

        /// <summary>
        /// Determines if this renderer can expand the given scope name.
        /// </summary>
        /// <param name="scopeName">The scope name to check.</param>
        /// <returns>A boolean value indicating if the renderer can or cannot expand the macro.</returns>
        public bool CanExpand(string scopeName)
        {
            return (scopeName == ScopeName.Channel9Video
                    || scopeName == ScopeName.FlashVideo
                    || scopeName == ScopeName.QuickTimeVideo
                    || scopeName == ScopeName.RealPlayerVideo
                    || scopeName == ScopeName.WindowsMediaVideo
                    || scopeName == ScopeName.YouTubeVideo
                    || scopeName == ScopeName.InvalidVideo);
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
            if (scopeName == ScopeName.InvalidVideo)
                return RenderUnresolvedMacro("type", string.Empty);

            try
            {
                string[] parameters = input.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);
                string url = Parameters.ExtractUrl(parameters);
                HorizontalAlign align = Parameters.ExtractAlign(parameters, HorizontalAlign.Center);

                IVideoRenderer videoRenderer = GetVideoRenderer(scopeName);
                videoRenderer.Dimensions = Parameters.ExtractDimensions(parameters, 285, 320);

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

                    videoRenderer.Render(url, writer);
                    
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
            catch (InvalidDimensionException ex)
            {
                return RenderUnresolvedMacro(ex.ParamName, ex.Reason);
            }
            catch (ArgumentException ex)
            {
                return RenderUnresolvedMacro(ex.ParamName, string.Empty);
            }
        }

        private static IVideoRenderer GetVideoRenderer(string scopeName)
        {
            IVideoRenderer videoRenderer = null;

            switch (scopeName)
            {
                case ScopeName.Channel9Video:
                    videoRenderer = new Channel9VideoRenderer();
                    break;
                case ScopeName.FlashVideo:
                    videoRenderer = new FlashVideoRenderer();
                    break;
                case ScopeName.QuickTimeVideo:
                    videoRenderer = new QuickTimeVideoRenderer();
                    break;
                case ScopeName.RealPlayerVideo:
                    videoRenderer = new RealPlayerVideoRenderer();
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

        private static string RenderUnresolvedMacro(string parameterName, string message)
        {
            return string.Format("<span class=\"unresolved\">Cannot resolve video macro, invalid parameter '{0}'.{1}</span>", parameterName, message);
        }
    }
}