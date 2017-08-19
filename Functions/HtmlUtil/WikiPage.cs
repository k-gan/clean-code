namespace CleanCode.Functions.HtmlUtil
{
    public class WikiPage
    {
        public string Name { get; set; }
        public PageCrawler PageCrawler { get; set; } = new PageCrawler();
        public string FullPath { get; set; }
    }
}