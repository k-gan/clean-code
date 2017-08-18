using System;
using System.Text;
using CleanCode.Functions.HtmlUtil;
using Xunit;

namespace CleanCode.Functions
{
    public class HtmlUntouched
    {
        public static string TestableHtml(
            PageData pageData,
            bool includeSuiteSetup)
        {
            var wikiPage = pageData.WikiPage;
            var buffer = new StringBuilder();

            if (pageData.HasAttribute("Test"))
            {
                if(includeSuiteSetup)
                {
                    var suiteSetup = PageCrawler.GetInheritedPage(
                        SuiteResponder.SuiteSetupName, wikiPage);
                    
                    if(suiteSetup != null)
                    {
                        var pagePath = suiteSetup.PageCrawler.GetFullPath(suiteSetup);
                        var pagePathName = PathParser.Render(pagePath);
                        buffer.Append("!include -setup .")
                            .Append(pagePathName)
                            .Append(Environment.NewLine);
                    }
                }

                var setup = PageCrawler.GetInheritedPage("SetUp", wikiPage);

                if(setup != null)
                {
                    var setupPath = wikiPage.PageCrawler.GetFullPath(setup);
                    var setupPathName = PathParser.Render(setupPath);
                    buffer.Append("!include -setup .")
                            .Append(setupPathName)
                            .Append(Environment.NewLine);
                }
            }

            buffer.Append(pageData.Content);

            if(pageData.HasAttribute("Test"))
            {
                var teardown = PageCrawler.GetInheritedPage("TearDown", wikiPage);

                if(teardown != null)
                {
                    var tearDownPath = wikiPage.PageCrawler.GetFullPath(teardown);
                    var tearDownPathName = PathParser.Render(tearDownPath);
                    buffer.Append(Environment.NewLine)
                        .Append("!include - teardown .")
                        .Append(tearDownPathName)
                        .Append(Environment.NewLine);
                }

                if(includeSuiteSetup)
                {
                    var suiteTeardown = PageCrawler.GetInheritedPage(
                        SuiteResponder.SuiteTeardownName, wikiPage);
                    
                    if(suiteTeardown != null)
                    {
                        var pagePath = suiteTeardown.PageCrawler.GetFullPath(suiteTeardown);
                        var pagePathName = PathParser.Render(pagePath);

                        buffer.Append("!include -teardown .")
                            .Append(pagePathName)
                            .Append(Environment.NewLine);
                    }
                }
            }

            pageData.Content = buffer.ToString();
            return pageData.Html;
        }
    }
}
