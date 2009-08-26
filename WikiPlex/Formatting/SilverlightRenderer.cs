using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace WikiPlex.Formatting
{
    public class SilverlightRenderer : IRenderer
    {
        public string Id
        {
            get { return "Silverlight"; }
        }

        public bool CanExpand(string scopeName)
        {
            return scopeName == ScopeName.Silverlight;
        }

        public string Expand(string scopeName, string input, Func<string, string> htmlEncode, Func<string, string> attributeEncode)
        {
            string[] parameters = input.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            string urlParameter = parameters.FirstOrDefault(s => s.StartsWith("url=", StringComparison.OrdinalIgnoreCase));
            string heightParameter = parameters.FirstOrDefault(s => s.StartsWith("height=", StringComparison.OrdinalIgnoreCase));
            string widthParameter = parameters.FirstOrDefault(s => s.StartsWith("width=", StringComparison.OrdinalIgnoreCase));
            string versionParameter = parameters.FirstOrDefault(s => s.StartsWith("version=", StringComparison.OrdinalIgnoreCase));
            string url;
            int height = 200;
            int width = 200;
            bool percentHeight = false, percentWidth = false;
            int version = 3;

            if(!string.IsNullOrEmpty(heightParameter) && heightParameter.EndsWith("%"))
            {
                percentHeight = true;
                heightParameter = heightParameter.TrimEnd('%');
            }
            if(!string.IsNullOrEmpty(widthParameter) && widthParameter.EndsWith("%"))
            {
                percentWidth = true;
                widthParameter = widthParameter.TrimEnd('%');
            }

            if (string.IsNullOrEmpty(urlParameter))
                return RenderUnresolvedMacro("url");
            if (!string.IsNullOrEmpty(heightParameter) 
                && (!int.TryParse(heightParameter.Substring(7), out height) || height <= 0))
                return RenderUnresolvedMacro("height");
            if (!string.IsNullOrEmpty(widthParameter) 
                && (!int.TryParse(widthParameter.Substring(6), out width) || width <= 0))
                return RenderUnresolvedMacro("width");
            if (!string.IsNullOrEmpty(versionParameter)
                && (!int.TryParse(versionParameter.Substring(8), out version) || version < 2 || version > 3))
                version = 3;

            url = urlParameter.Substring(4);
            if (url.ToLowerInvariant().Contains("codeplex.com"))
                return RenderUnresolvedMacro("url");

            try
            {
                var parsedUrl = new Uri(url, UriKind.Absolute);
                url = parsedUrl.AbsoluteUri;
            }
            catch
            {
                return RenderUnresolvedMacro("url");
            }

            ISilverlightRenderer renderer = GetRenderer(version);

            var content = new StringBuilder();
            using (var tw = new StringWriter(content))
            using (var writer = new HtmlTextWriter(tw, string.Empty))
            {
                writer.NewLine = string.Empty;

                renderer.AddObjectTagAttributes(writer);
                writer.AddStyleAttribute(HtmlTextWriterStyle.Height, height + (percentHeight ? "%" : "px"));
                writer.AddStyleAttribute(HtmlTextWriterStyle.Width, width + (percentWidth ? "%" : "px"));
                writer.RenderBeginTag(HtmlTextWriterTag.Object);

                renderer.AddParameterTags(url, writer);
                renderer.AddDownloadLink(writer);

                writer.RenderEndTag(); // object
                
                writer.AddStyleAttribute(HtmlTextWriterStyle.Visibility, "hidden");
                writer.AddStyleAttribute(HtmlTextWriterStyle.Height, "0");
                writer.AddStyleAttribute(HtmlTextWriterStyle.Width, "0");
                writer.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, "0");
                writer.RenderBeginTag(HtmlTextWriterTag.Iframe);
                writer.RenderEndTag();
            }

            return content.ToString();
        }

        private static string RenderUnresolvedMacro(string parameterName)
        {
            return string.Format("<span class=\"unresolved\">Cannot resolve silverlight macro, invalid parameter '{0}'.</span>", parameterName);
        }

        private static ISilverlightRenderer GetRenderer(int version)
        {
            if (version == 3)
                return new Silverlight3Renderer();

            return new Silverlight2Renderer();
        }
    }
}