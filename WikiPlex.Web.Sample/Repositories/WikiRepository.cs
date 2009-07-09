using System;
using System.Linq;
using WikiPlex.Web.Sample.Models;

namespace WikiPlex.Web.Sample.Repositories
{
    public class WikiRepository : IWikiRepository
    {
        private readonly WikiPlexDataContext dataContext = new WikiPlexDataContext();

        public Content Get(string slug)
        {
            return (from c in dataContext.Contents
                   where c.Title.Slug == slug
                   orderby c.Version descending
                   select c).FirstOrDefault();
        }

        public Content GetByVersion(string slug, int version)
        {
            return (from c in dataContext.Contents
                    where c.Title.Slug == slug
                    where c.Version == version
                    select c).FirstOrDefault();
        }

        public void Save(string slug, string title, string source)
        {
            var currentTitle = (from t in dataContext.Titles
                                where t.Slug == slug
                                select t).SingleOrDefault();

            if (currentTitle == null)
            {
                currentTitle = new Title {Name = title, Slug = slug};
                dataContext.Titles.InsertOnSubmit(currentTitle);
            }
            else
            {
                currentTitle.Name = title;
            }

            currentTitle.Contents.Add(new Content {Source = source, Version = currentTitle.Contents.Count + 1, VersionDate = DateTime.Now});

            dataContext.SubmitChanges();
        }
    }
}