using WikiPlex.Web.Sample.Models;

namespace WikiPlex.Web.Sample.Repositories
{
    public interface IWikiRepository
    {
        Content Get(string slug);
        Content GetByVersion(string slug, int version);
        void Save(string slug, string title, string source);
    }
}