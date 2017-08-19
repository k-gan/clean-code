using System;
using System.Text;
using CleanCode.Functions.HtmlUtil;

namespace Functions
{
    public class PageDataRenderer
    {
        private readonly PageData pageData;
        private readonly StringBuilder pageContent;

        public PageDataRenderer(PageData pageData)
        {
            if (pageData == null) throw new ArgumentNullException(nameof(pageData));

            this.pageData = pageData;
            this.pageContent = new StringBuilder();
        }

        public string Render(bool includeSuite)
        {
            if (!HasTestAttribute())
                AddTestToPage(includeSuite);

            return pageData.Html;
        }

        private void AddTestToPage(bool includeSuite)
        {
            var wikiPage = this.pageData.WikiPage;
            AddTestToPageContent(includeSuite, wikiPage);
            this.pageData.Content = this.pageContent.ToString();
        }

        private void AddTestToPageContent(bool includeSuite, WikiPage wikiPage)
        {
            IncludeSuiteSetup(includeSuite, wikiPage);
            this.pageContent.Append(this.pageData.Content);
            IncludeSuiteTearDown(includeSuite, wikiPage);
        }

        private bool HasTestAttribute() => pageData.HasAttribute("Test");

        private void IncludeSuiteSetup(bool includeSuite, WikiPage wikiPage)
        {
            if (includeSuite)
                Include(wikiPage, SuiteResponder.SuiteSetupName, "setup");

            Include(wikiPage, "SetUp", "setup");
        }

        private void IncludeSuiteTearDown(bool includeSuite, WikiPage wikiPage)
        {
            Include(wikiPage, "TearDown", "teardown");

            if (includeSuite)
                Include(wikiPage, SuiteResponder.SuiteTeardownName, "teardown");
        }

        private static string CreatePagePathName(WikiPage wikiPage, string pageName)
        {
            var inheritedWikiPage = PageCrawler.GetInheritedPage(pageName, wikiPage);
            if (inheritedWikiPage == null) return null;

            var pagePath = wikiPage.PageCrawler.GetFullPath(inheritedWikiPage);
            return PathParser.Render(pagePath);
        }

        private void Include(WikiPage wikiPage, string pageName, string includeType)
        {
            var pagePathName = CreatePagePathName(wikiPage, pageName);
            if (pagePathName == null) return;

            Append(includeType, pagePathName);
        }

        private void Append(string includeType, string pagePathName)
        {
            this.pageContent.AppendLine();
            this.pageContent.AppendLine($"!include -{includeType} .{pagePathName}");
        }
    }
}