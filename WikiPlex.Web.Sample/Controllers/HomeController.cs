using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using WikiPlex.Formatting;
using WikiPlex.Web.Sample.Models;
using WikiPlex.Web.Sample.Repositories;
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

        [ActionName("View")]
        public ActionResult ViewWiki(string slug)
        {
            Content content = repository.Get(slug);

            if (content == null)
                return RedirectToAction("Edit", new {slug});

            content.RenderedSource = wikiEngine.Render(content.Source, GetFormatter());
            return View(content);
        }

        public ActionResult ViewWikiVersion(string slug, int version)
        {
            Content content = repository.GetByVersion(slug, version);

            if (content == null)
                return RedirectToAction("View", new {slug});

            content.RenderedSource = wikiEngine.Render(content.Source, GetFormatter());
            return View("View", content);
        }

        private MacroFormatter GetFormatter()
        {
            var siteRenderers = new IRenderer[] {new TitleLinkRenderer(Url)};
            IEnumerable<IRenderer> allRenderers = Renderers.All.Union(siteRenderers);
            return new MacroFormatter(allRenderers);
        }

        [ActionName("Edit")]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult EditWiki(string slug)
        {
            Content content = repository.Get(slug);

            if (content == null)
                content = new Content {Title = new Title {Slug = slug}};

            return View(content);
        }

        [ActionName("Edit")]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult EditWiki(string slug, string name, string source)
        {
            repository.Save(slug, name, source);
            return RedirectToAction("View", new {slug});
        }

        [ActionName("Source")]
        [AcceptVerbs(HttpVerbs.Post)]
        [OutputCache(Location = OutputCacheLocation.None)]
        public string GetWikiSource(string slug, int version)
        {
            Content content = repository.GetByVersion(slug, version);

            return content.Source;
        }

        [ActionName("Preview")]
        [AcceptVerbs(HttpVerbs.Post)]
        [OutputCache(Location = OutputCacheLocation.None)]
        [ValidateInput(false)]
        public string GetWikiPreview(string slug, string source)
        {
            return wikiEngine.Render(source, GetFormatter());
        }
    }
}