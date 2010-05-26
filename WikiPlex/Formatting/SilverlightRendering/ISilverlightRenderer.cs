using System.Web.UI;

namespace WikiPlex.Formatting
{
    internal interface ISilverlightRenderer
    {
        void AddObjectTagAttributes(HtmlTextWriter writer);
        void AddParameterTags(string url, bool gpuAcceleration, string[] initParams, HtmlTextWriter writer);
        void AddDownloadLink(HtmlTextWriter writer);
    }
}