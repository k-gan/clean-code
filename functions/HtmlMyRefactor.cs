using System;
using System.Text;
using CleanCode.Functions.HtmlUtil;

namespace CleanCode.Functions
{
    public class HtmlMyRefactor
    {
        private static void Include(
            WikiPage wikiPage,
            StringBuilder buffer,
            string pageName,
            string includeType,
            bool includePreliminaryNewLine = false)
        {
            var pagePathName = CreatePagePathName(wikiPage, pageName);
            if (pagePathName == null) return;

            if (includePreliminaryNewLine)
                buffer.Append(Environment.NewLine);

            Append(buffer, includeType, pagePathName);
        }

        private static string CreatePagePathName(WikiPage wikiPage, string pageName)
        {
            var inheritedWikiPage = PageCrawler.GetInheritedPage(pageName, wikiPage);

            if (inheritedWikiPage == null) return null;

            var pagePath = wikiPage.PageCrawler.GetFullPath(inheritedWikiPage);
            return PathParser.Render(pagePath);
        }

        private static void Append(
            StringBuilder buffer, string includeType, string pagePathName)
        {
            buffer.Append($"!include -{includeType} .")
                    .Append(pagePathName)
                    .Append(Environment.NewLine);
        }

        private static void IncludeSuiteSetup(
            bool includeSuite, WikiPage wikiPage, StringBuilder buffer)
        {
            if (includeSuite)
                Include(wikiPage, buffer, SuiteResponder.SuiteSetupName, "setup");

            Include(wikiPage, buffer, "SetUp", "setup");
        }

        private static void IncludeSuiteTearDown(
                    bool includeSuite, WikiPage wikiPage, StringBuilder buffer)
        {
            Include(wikiPage, buffer, "TearDown", "teardown", true);

            if (includeSuite)
                Include(wikiPage, buffer, SuiteResponder.SuiteTeardownName, "teardown");
        }

        public static string TestableHtml(PageData pageData, bool includeSuite)
        {
            var hasTest = pageData.HasAttribute("Test");

            if (hasTest) IncludeTest(pageData, includeSuite);
            return pageData.Html;
        }

        private static void IncludeTest(PageData pageData, bool includeSuite)
        {
            var wikiPage = pageData.WikiPage;
            var buffer = new StringBuilder();

            IncludeSuiteSetup(includeSuite, wikiPage, buffer);
            buffer.Append(pageData.Content);
            IncludeSuiteTearDown(includeSuite, wikiPage, buffer);

            pageData.Content = buffer.ToString();
        }
    }
}