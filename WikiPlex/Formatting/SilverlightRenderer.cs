using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI;
using WikiPlex.Common;

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
            try
            {
                string[] parameters = input.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);
                string url = Parameters.ExtractUrl(parameters);
                Dimensions dimensions = Parameters.ExtractDimensions(parameters, 200, 200);

                string versionValue;
                int version = 3;
                if (Parameters.TryGetValue(parameters, "version", out versionValue) && int.TryParse(versionValue, out version))
                {
                    if (version < 2 || version > 3)
                        version = 3;
                }

                string[] initParams = GetInitParams(parameters);

                ISilverlightRenderer renderer = GetRenderer(version);

                var content = new StringBuilder();
                using (var tw = new StringWriter(content))
                using (var writer = new HtmlTextWriter(tw, string.Empty))
                {
                    writer.NewLine = string.Empty;

                    renderer.AddObjectTagAttributes(writer);
                    writer.AddStyleAttribute(HtmlTextWriterStyle.Height, dimensions.Height.ToString());
                    writer.AddStyleAttribute(HtmlTextWriterStyle.Width, dimensions.Width.ToString());
                    writer.RenderBeginTag(HtmlTextWriterTag.Object);

                    renderer.AddParameterTags(url, initParams, writer);
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
            catch (ArgumentException ex)
            {
                return string.Format("<span class=\"unresolved\">Cannot resolve silverlight macro, invalid parameter '{0}'.</span>", ex.ParamName);
            }
        }

        private static string[] GetInitParams(IEnumerable<string> parameters)
        {
            return (from p in parameters
                    where !p.StartsWith("url=", StringComparison.OrdinalIgnoreCase)
                          && !p.StartsWith("height=", StringComparison.OrdinalIgnoreCase)
                          && !p.StartsWith("width=", StringComparison.OrdinalIgnoreCase)
                          && !p.StartsWith("version=", StringComparison.OrdinalIgnoreCase)
                    select p).ToArray();
        }

        private static ISilverlightRenderer GetRenderer(int version)
        {
            if (version == 3)
                return new Silverlight3Renderer();

            return new Silverlight2Renderer();
        }
    }
}