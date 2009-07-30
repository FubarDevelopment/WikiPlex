using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using WikiPlex.Formatting;
using WikiPlex.Web.Sample.Models;
using WikiPlex.Web.Sample.Repositories;
using WikiPlex.Web.Sample.Views.Home;
using WikiPlex.Web.Sample.Wiki;

namespace WikiPlex.Web.Sample.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        private readonly IWikiRepository repository;
        private readonly IWikiEngine wikiEngine;

        public HomeController()
            : this(new WikiRepository(), new WikiEngine())
        {
        }

        public HomeController(IWikiRepository repository, IWikiEngine wikiEngine)
        {
            this.repository = repository;
            this.wikiEngine = wikiEngine;
        }

        public ActionResult ViewWiki(string slug)
        {
            var viewData = new ViewContent();
            viewData.Content = repository.Get(slug);

            if (viewData.Content == null)
                return RedirectToAction("EditWiki", new {slug});

            viewData.Content.RenderedSource = wikiEngine.Render(viewData.Content.Source, GetFormatter());
            viewData.History = repository.GetHistory(slug);

            return View("View", viewData);
        }

        public ActionResult ViewWikiVersion(string slug, int version)
        {
            var viewData = new ViewContent();
            viewData.Content = repository.GetByVersion(slug, version);

            if (viewData.Content == null)
                return RedirectToAction("ViewWiki", new {slug});

            viewData.Content.RenderedSource = wikiEngine.Render(viewData.Content.Source, GetFormatter());
            viewData.History = repository.GetHistory(slug);

            return View("View", viewData);
        }

        private MacroFormatter GetFormatter()
        {
            var siteRenderers = new IRenderer[] {new TitleLinkRenderer(Url)};
            IEnumerable<IRenderer> allRenderers = Renderers.All.Union(siteRenderers);
            return new MacroFormatter(allRenderers);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult EditWiki(string slug)
        {
            Content content = repository.Get(slug);

            if (content == null)
                content = new Content {Title = new Title {Slug = slug}};

            return View("Edit", content);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult EditWiki(string slug, string name, string source)
        {
            repository.Save(slug, name, source);
            return RedirectToAction("ViewWiki", new {slug});
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [OutputCache(Location = OutputCacheLocation.None)]
        public string GetWikiSource(string slug, int version)
        {
            Content content = repository.GetByVersion(slug, version);

            return content.Source;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [OutputCache(Location = OutputCacheLocation.None)]
        [ValidateInput(false)]
        public string GetWikiPreview(string slug, string source)
        {
            return wikiEngine.Render(source, GetFormatter());
        }
    }
}