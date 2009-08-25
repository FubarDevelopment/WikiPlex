using System.Web.UI;

namespace WikiPlex.Formatting
{
    internal class QuickTimeVideoRenderer : VideoRendererBase
    {
        public static readonly string ClassIdAttributeString = "CLSID:02BF25D5-8C17-4B23-BC80-D3488ABDDC6B";
        public static readonly string CodebaseAttributeString = "http://www.apple.com/qtactivex/qtplugin.cab";
        public static readonly string PluginsPageAttributeString = "http://www.apple.com/quicktime/download/";
        public static readonly string TypeAttributeString = "video/quicktime";

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
            AddParameterTag("autoplay", "false", writer);
        }
    }
}