using System.Web.UI;

namespace WikiPlex.Formatting
{
    internal class WindowsMediaPlayerVideoRenderer : VideoRendererBase
    {
        public static readonly string ClassIdAttributeString = "CLSID:22D6F312-B0F6-11D0-94AB-0080C74C7E95";
        public static readonly string CodebaseAttrubuteString = "http://activex.microsoft.com/activex/controls/mplayer/en/nsmp2inf.cab#Version=5,1,52,701";
        public static readonly string PluginsPageAttributeString = "http://www.microsoft.com/windows/windowsmedia/download/default.asp";
        public static readonly string TypeAttributeString = "application/x-mplayer2";

        public override string ClassIdAttribute
        {
            get { return ClassIdAttributeString; }
        }

        public override string CodebaseAttribute
        {
            get { return CodebaseAttrubuteString; }
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
            AddParameterTag("fileName", url, writer);
        }
    }
}