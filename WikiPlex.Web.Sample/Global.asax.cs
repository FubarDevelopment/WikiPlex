using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WikiPlex.Web.Sample.Wiki;

namespace WikiPlex.Web.Sample
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("content/{*pathInfo}");

            routes.MapRoute(
                "History",
                "{slug}/v{version}",
                new {controller = "Home", action = "ViewWikiVersion"},
                new {version = @"\d+"}
                );

            routes.MapRoute(
                "Source",
                "{slug}/source/v{version}",
                new { controller = "Home", action = "GetWikiSource" },
                new {version = @"\d+"}
                );

            routes.MapRoute(
                "Act",
                "{slug}/{action}",
                new { controller = "Home", action = "ViewWiki" },
                new {action = @"\w+"}
                );

            routes.MapRoute(
                "Default",
                "{slug}",
                new { controller = "Home", action = "ViewWiki", slug = "home" }
                );
        }

        protected void Application_Start()
        {
            RegisterRoutes(RouteTable.Routes);

            Macros.Register<TitleLinkMacro>();
        }
    }
}