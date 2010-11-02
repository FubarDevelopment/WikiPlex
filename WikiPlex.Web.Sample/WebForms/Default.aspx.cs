﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WikiPlex.Formatting;
using WikiPlex.Web.Sample.Repositories;
using WikiPlex.Web.Sample.Wiki;
using Content=WikiPlex.Web.Sample.Models.Content;

namespace WikiPlex.Web.Sample.WebForms
{
    public partial class Default : Page
    {
        private readonly IWikiRepository repository = new WikiRepository();
        private readonly IWikiEngine wikiEngine = new WikiEngine();
        private Content wikiContent;

        protected void Page_Load(object sender, EventArgs e)
        {
            int id = GetId();
            string slug = GetSlug();

            int version;
            if (!string.IsNullOrEmpty(Request.QueryString["v"]) && int.TryParse(Request.QueryString["v"], out version))
            {
                wikiContent = repository.GetByVersion(slug, version);
                if (wikiContent == null)
                    Response.Redirect(ResolveClientUrl("~/WebForms/?p=" + HttpUtility.UrlEncode(slug)));
            }
            else
                wikiContent = repository.Get(id);

            if (wikiContent == null)
                Response.Redirect(ResolveClientUrl("~/WebForms/Edit.aspx?i=" + id + "&p=" + HttpUtility.UrlEncode(slug)));

            title.Text = "WikiPlex Sample - " + HttpUtility.HtmlEncode(wikiContent.Title.Name);
            sourceSlug.Text = previewSlug.Text = wikiContent.Title.Slug;
            sourceVersion.Text = wikiContent.Version.ToString();
            renderedSource.Text = wikiEngine.Render(wikiContent.Source, GetRenderers());
            Name.Value = wikiContent.Title.Name;
            NotLatestPlaceHolder.Visible = wikiContent.Version != wikiContent.Title.MaxVersion;

            pageHistory.DataSource = repository.GetHistory(slug);
            pageHistory.DataBind();
        }

        private int GetId()
        {
            string idParam = Request.QueryString["i"];
            if (string.IsNullOrEmpty(idParam))
                return 1;
            return int.Parse(idParam);
        }

        private string GetSlug()
        {
            string slug = Request.QueryString["p"];
            if (string.IsNullOrEmpty(slug))
                slug = "home";
            return slug;
        }

        protected void BindPageHistoryItem(object sender, RepeaterItemEventArgs e)
        {
            var date = e.Item.FindControl("date") as Literal;
            var versionLink = e.Item.FindControl("versionLink") as HyperLink;
            var historyItem = e.Item.DataItem as Content;

            date.Visible = versionLink.Visible = false;

            if (wikiContent.Version == historyItem.Version)
            {
                date.Visible = true;
                date.Text = wikiContent.VersionDate.ToString();
            }
            else
            {
                versionLink.Visible = true;
                versionLink.NavigateUrl = ResolveClientUrl("~/WebForms/?p=" + HttpUtility.UrlEncode(wikiContent.Title.Slug) + "&v=" + historyItem.Version);
                versionLink.Text = historyItem.VersionDate.ToString();
            }
        }

        private static IEnumerable<IRenderer> GetRenderers()
        {
            var siteRenderers = new IRenderer[] { new TitleLinkRenderer() };
            return Renderers.All.Union(siteRenderers);
        }

        protected void SaveWikiContent(object sender, EventArgs e)
        {
            string slug = GetSlug();
            repository.Save(slug, Name.Value, Source.Text);
            Response.Redirect("~/WebForms/?p=" + HttpUtility.UrlEncode(slug));
        }
    }
}