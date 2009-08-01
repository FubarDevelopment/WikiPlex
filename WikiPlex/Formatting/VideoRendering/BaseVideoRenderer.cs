using System.Web.UI;

namespace WikiPlex.Formatting
{
    internal abstract class VideoRendererBase : IVideoRenderer
    {
        public static string DefaultVideoHeight = "285";
        public static string DefaultVideoWidth = "320";

        public abstract string ClassIdAttribute { get; }
        public abstract string CodebaseAttribute { get; }
        public abstract string PluginsPageAttribute { get; }
        public abstract string TypeAttribute { get; }


        public virtual void AddObjectTagAttributes(string url, HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Type, TypeAttribute);
            writer.AddAttribute(HtmlTextWriterAttribute.Height, DefaultVideoHeight);
            writer.AddAttribute(HtmlTextWriterAttribute.Width, DefaultVideoWidth);
            writer.AddAttribute("classid", ClassIdAttribute);
            writer.AddAttribute("codebase", CodebaseAttribute);
        }

        public abstract void AddParameterTags(string url, HtmlTextWriter writer);

        public virtual void AddEmbedTagAttributes(string url, HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Type, TypeAttribute);
            writer.AddAttribute(HtmlTextWriterAttribute.Height, DefaultVideoHeight);
            writer.AddAttribute(HtmlTextWriterAttribute.Width, DefaultVideoWidth);
            writer.AddAttribute(HtmlTextWriterAttribute.Src, url);
            writer.AddAttribute("pluginspage", PluginsPageAttribute);
            writer.AddAttribute("autoplay", "false");
            writer.AddAttribute("autostart", "false");
        }

        protected static void AddParameterTag(string name, string value, HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Name, name);
            writer.AddAttribute(HtmlTextWriterAttribute.Value, value);
            writer.RenderBeginTag(HtmlTextWriterTag.Param);
            writer.RenderEndTag();
        }
    }
}