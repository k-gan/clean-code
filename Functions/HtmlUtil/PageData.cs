using System.Collections.Generic;

namespace CleanCode.Functions.HtmlUtil
{
    public class PageData
    {
        private readonly List<string> _attributes = new List<string>();

        public WikiPage WikiPage { get; set; }
        public string Content { get; internal set; }
        public string Html { get; internal set; }

        public bool HasAttribute(string attribute)
        {
            return _attributes.Contains(attribute);
        }
    }
}