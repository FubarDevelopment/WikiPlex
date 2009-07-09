using System.Web.UI;

namespace WikiPlex.Formatting.VideoRendering
{
    internal interface IVideoRenderer
    {
        void AddObjectTagAttributes(string url, HtmlTextWriter writer);
        void AddParameterTags(string url, HtmlTextWriter writer);
        void AddEmbedTagAttributes(string url, HtmlTextWriter writer);
    }
}