using System.Web.UI;

namespace WikiPlex.Formatting.VideoRendering
{
    internal class FlashVideoRenderer : VideoRendererBase
    {
        public static readonly string ClassIdAttributeString = "CLSID:D27CDB6E-AE6D-11cf-96B8-444553540000";
        public static readonly string CodebaseAttrubuteString = "http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=6,0,0,0";
        public static readonly string PluginsPageAttributeString = "http://macromedia.com/go/getflashplayer";
        public static readonly string TypeAttributeString = "application/x-shockwave-flash";

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
            AddParameterTag("movie", url, writer);
        }
    }
}