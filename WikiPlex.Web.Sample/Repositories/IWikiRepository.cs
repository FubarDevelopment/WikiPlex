using System.Collections.Generic;
using WikiPlex.Web.Sample.Models;

namespace WikiPlex.Web.Sample.Repositories
{
    public interface IWikiRepository
    {
        Content Get(string slug, string title);
        Content Get(int id);
        Content GetByVersion(string slug, int version);
        Content GetByVersion(int id, int version);
        ICollection<Content> GetHistory(string slug);
        void Save(string slug, string title, string source);
        int Save(int id, string slug, string title, string source);
    }
}