namespace WikiPlex.Syndication
{
    public class SyndicationItem
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public SyndicationDate Date { get; set; }
    }
}