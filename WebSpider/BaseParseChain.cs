using System;
using System.Text;
using System.Text.RegularExpressions;

namespace WebSpider
{
    public class BaseParseChain
    {
        protected BaseParseChain Next;

        public BaseParseChain(BaseParseChain next) { Next = next; }

        public BaseParseChain() { }

        public void SetNext(BaseParseChain next) { Next = next; }

        public void Add(BaseParseChain next)
        {
            if (Next != null)
            { Next.Add(next); }
            else { Next = next; }
        }

        public virtual void SendNext(Crawler c) { if (this.Next != null) { this.Next.SendNext(c); } }        
    }

    public class ParseHeadersChain : BaseParseChain
    {
        public override void SendNext(Crawler c)
        {
            this.ExtractHeaders(c);
            base.SendNext(c);
        }

        private void ExtractHeaders(Crawler c)
        {
            MatchCollection res = Regex.Matches(c.HTMLText, @"h[0-3]\s?(.*?)>(.*?)</h", RegexOptions.Multiline);
            StringBuilder s = new StringBuilder();
            string subres = "";
            foreach (Match m in res)
            {
                subres = Regex.Replace(m.Groups[2].Value, "(<.*?>)", " ");
                if (!String.IsNullOrWhiteSpace(subres))
                {
                    s.Append(subres);
                    s.Append("\n");
                }
            }

            c.ExtractedHeaders = s.ToString();
        }
    }

    public class ParseParagraphsChain : BaseParseChain
    {
        public override void SendNext(Crawler c)
        {
            this.ExtractParagraphs(c);
            base.SendNext(c);
        }

        private void ExtractParagraphs(Crawler c)
        {
            MatchCollection res = Regex.Matches(c.HTMLText, @"<p\s?(.*?)>(.*?)</p", RegexOptions.Multiline);
            StringBuilder s = new StringBuilder();
            string subres = "";
            foreach (Match m in res)
            {
                subres = Regex.Replace(m.Groups[2].Value, "(<.*?>)", " ");
                if (!String.IsNullOrWhiteSpace(subres))
                {
                    s.Append(subres);
                    s.Append("\n");
                }
            }

            c.ExtractedParagraphs = s.ToString();
        }
    }

    public class ParseDivsChain : BaseParseChain
    {
        public override void SendNext(Crawler c)
        {
            this.ExtractDivs(c);
            base.SendNext(c);
        }

        private void ExtractDivs(Crawler c)
        {
            MatchCollection res = Regex.Matches(c.HTMLText, @"<div\s?(.*?)>(.*?)</div", RegexOptions.Multiline);

            StringBuilder s = new StringBuilder();
            string subres = "";
            foreach (Match m in res)
            {
                subres = Regex.Replace(m.Groups[2].Value, "(<.*?>)", " ");
                if (!String.IsNullOrWhiteSpace(subres))
                {
                    s.Append(subres);
                    s.Append("\n");
                }
            }

            c.ExtractedDivs = s.ToString();
        }
    }

    public class ParseLinksTextChain : BaseParseChain
    {
        public override void SendNext(Crawler c)
        {
            this.ExtractLinksText(c);
            base.SendNext(c);
        }

        private void ExtractLinksText(Crawler c)
        {
            MatchCollection res = Regex.Matches(c.HTMLText, @"href(.*?)>(.*?)</a", RegexOptions.Multiline);

            StringBuilder s = new StringBuilder();
            string subres = "";
            foreach (Match m in res)
            {
                subres = Regex.Replace(m.Groups[2].Value, "(<.*?>)", " ");
                if (!String.IsNullOrWhiteSpace(subres))
                {
                    s.Append(subres);
                    s.Append("\n");
                }
            }

            c.ExtractedLinksText = s.ToString();
        }
    }

    public class ParseLinksChain : BaseParseChain
    {
        public override void SendNext(Crawler c)
        {
            this.ExtractLinks(c);
            base.SendNext(c);
        }

        private void ExtractLinks(Crawler c)
        {
            MatchCollection res = Regex.Matches(c.HTMLText, @"href=(.*)");
            string link = "";
            foreach (Match m in res)
            {
                link = this.ExtractSingleLink(m.ToString(), c.URL);
                if (!String.IsNullOrWhiteSpace(link))
                { c.LINKS.Add(link); }
            }
        }

        private string ExtractSingleLink(string href_link, string start_url)
        {
            StringBuilder result_link = new StringBuilder();
            href_link = href_link.Split(new char[] { '\"', '\'' })[1];
            if (!String.IsNullOrEmpty(href_link))
            {
                if (href_link.StartsWith("//"))
                {
                    result_link.Append("http:");
                    result_link.Append(href_link);
                }
                else if (href_link.StartsWith("http")) { return href_link; }
                else if (href_link[0] == '/')
                {
                    var link_parts = start_url.Split(new char[] { '/' });
                    result_link.Append(link_parts[0]);
                    result_link.Append("//");
                    result_link.Append(link_parts[2]);
                    result_link.Append(href_link);
                }
            }

            return result_link.ToString();
        }
    }
}
