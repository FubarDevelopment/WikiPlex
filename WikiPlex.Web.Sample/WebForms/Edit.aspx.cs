using System;
using System.Web;
using System.Web.UI;
using WikiPlex.Web.Sample.Models;
using WikiPlex.Web.Sample.Repositories;

namespace WikiPlex.Web.Sample.WebForms
{
    public partial class Edit : Page
    {
        private readonly IWikiRepository repository = new WikiRepository();

        protected void Page_Load(object sender, EventArgs e)
        {
            string slug = GetSlug();
            Content content = repository.Get(slug);

            if (content != null)
            {
                Name.Text = content.Title.Name;
                Source.Text = content.Source;
                CancelPlaceHolder.Visible = true;
                Cancel.OnClientClick = "window.location.href='" + ResolveClientUrl("~/WebForms/?p=" + HttpUtility.UrlEncode(slug) + "'");
            }
        }

        private string GetSlug()
        {
            string slug = Request.QueryString["p"];
            if (string.IsNullOrEmpty(slug))
                slug = "home";
            return slug;
        }

        protected void SaveSource(object sender, EventArgs e)
        {
            string slug = GetSlug();
            repository.Save(slug, Name.Text, Source.Text);
            Response.Redirect("~/WebForms/?p=" + HttpUtility.UrlEncode(slug));
        }
    }
}