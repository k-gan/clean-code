using System;

namespace CleanCode.Functions.HtmlUtil
{
    public class PageCrawler
    {
        public static WikiPage GetInheritedPage(string name, WikiPage wikiPage)
        {
            return new WikiPage { Name = name };
        }

        public string GetFullPath(WikiPage suiteSetup)
        {
            return suiteSetup.FullPath;
        }
    }
}