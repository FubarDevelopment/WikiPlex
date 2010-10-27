using System.Web.UI;

namespace WikiPlex.Formatting.Renderers
{
    internal class Silverlight3Renderer : BaseSilverlightRenderer
    {
        public virtual string Version
        {
            get { return "3.0.40624.0"; }
        }

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
            get { return "http://go.microsoft.com/fwlink/?LinkID=149156&v=" + Version; }
        }

        public override void AddParameterTags(string url, bool gpuAcceleration, string[] initParams, HtmlTextWriter writer)
        {
            base.AddParameterTags(url, gpuAcceleration, initParams, writer);

            AddParameter("minRuntimeVersion", Version, writer);
            AddParameter("autoUpgrade", "true", writer);
        }
    }
}