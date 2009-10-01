using System.Web.UI;

namespace WikiPlex.Formatting
{
    internal class Silverlight3Renderer : BaseSilverlightRenderer
    {
        public override string DataMimeType
        {
            get { return "data:application/x-silverlight-2,"; }
        }

        public override string ObjectType
        {
            get { return "application/x-silverlight-2"; }
        }

        public override string DownloadUrl
        {
            get { return "http://go.microsoft.com/fwlink/?LinkID=149156&v=3.0.40624.0"; }
        }

        public override void AddParameterTags(string url, string[] initParams, HtmlTextWriter writer)
        {
            base.AddParameterTags(url, initParams, writer);

            AddParameter("minRuntimeVersion", "3.0.40624.0", writer);
            AddParameter("autoUpgrade", "true", writer);
        }
    }
}