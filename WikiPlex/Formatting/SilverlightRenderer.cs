using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI;
using WikiPlex.Common;

namespace WikiPlex.Formatting
{
    /// <summary>
    /// Will render the silverlight scopes.
    /// </summary>
    public class SilverlightRenderer : IRenderer
    {
        /// <summary>
        /// Gets the id of a renderer.
        /// </summary>
        public string Id
        {
            get { return "Silverlight"; }
        }

        /// <summary>
        /// Determines if this renderer can expand the given scope name.
        /// </summary>
        /// <param name="scopeName">The scope name to check.</param>
        /// <returns>A boolean value indicating if the renderer can or cannot expand the macro.</returns>
        public bool CanExpand(string scopeName)
        {
            return scopeName == ScopeName.Silverlight;
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
            try
            {
                string[] parameters = input.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);
                string url = Parameters.ExtractUrl(parameters);
                Dimensions dimensions = Parameters.ExtractDimensions(parameters, 200, 200);
                bool gpuAcceleration = Parameters.ExtractBool(parameters, "gpuAcceleration", false);

                string versionValue;
                int version = 4;
                if (Parameters.TryGetValue(parameters, "version", out versionValue) && int.TryParse(versionValue, out version))
                {
                    if (version < 2 || version > 4)
                        version = 4;
                }

                if (version == 2 && gpuAcceleration)
                    return "<span class\"unresolved\">Cannot resolve silverlight macro, 'gpuAcceleration' cannot be enabled with version 2 of Silverlight.</span>";

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

                    renderer.AddParameterTags(url, gpuAcceleration, initParams, writer);
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
                          && !p.StartsWith("gpuAcceleration=", StringComparison.OrdinalIgnoreCase)
                    select p).ToArray();
        }

        private static ISilverlightRenderer GetRenderer(int version)
        {
            switch (version)
            {
                case 4:
                    return new Silverlight4Renderer();
                case 3:
                    return new Silverlight3Renderer();
                default:
                    return new Silverlight2Renderer();
            }
        }
    }
}