using System.Collections.Generic;

namespace WikiPlex.Syndication
{
    public class SyndicationFeed
    {
        public SyndicationFeed()
        {
            Items = new List<SyndicationItem>();
        }

        public string Title { get; set; }
        public string Link { get; set; }
        public IList<SyndicationItem> Items { get; private set; }
    }
}