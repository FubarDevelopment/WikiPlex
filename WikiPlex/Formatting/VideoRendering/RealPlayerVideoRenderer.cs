using System.Web.UI;

namespace WikiPlex.Formatting.VideoRendering
{
    internal class RealPlayerVideoRenderer : VideoRendererBase
    {
        public static readonly string ClassIdAttributeString = "CLSID:CFCDAA03-8BE4-11CF-B84B-0020AFBBCCFA";
        public static readonly string CodebaseAttributeString = string.Empty;
        public static readonly string PluginsPageAttributeString = string.Empty;
        public static readonly string TypeAttributeString = "audio/x-pn-realaudio-plugin";

        public override string ClassIdAttribute
        {
            get { return ClassIdAttributeString; }
        }

        public override string CodebaseAttribute
        {
            get { return CodebaseAttributeString; }
        }

        public override string PluginsPageAttribute
        {
            get { return PluginsPageAttributeString; }
        }

        public override string TypeAttribute
        {
            get { return TypeAttributeString; }
        }

        public override void AddParameterTags(string url, HtmlTextWriter writer)
        {
            AddParameterTag("src", url, writer);
        }
    }
}