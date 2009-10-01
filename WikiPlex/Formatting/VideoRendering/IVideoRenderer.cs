using System.Web.UI;
using WikiPlex.Common;

namespace WikiPlex.Formatting
{
    internal interface IVideoRenderer
    {
        Dimensions Dimensions { get; set; }

        void AddObjectTagAttributes(string url, HtmlTextWriter writer);
        void AddParameterTags(string url, HtmlTextWriter writer);
        void AddEmbedTagAttributes(string url, HtmlTextWriter writer);
    }
}