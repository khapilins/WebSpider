using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebSpider
{
    public class Crawler : ICrawler
    {
        private List<String> _links = new List<string>();

        public List<string> LINKS { get { return _links; } set { _links = value; } }

        public String HTMLText { get; set; }

        public String URL { get; set; }

        public string ExtractedParagraphs { get; set; }

        public string ExtractedHeaders { get; set; }

        public string ExtractedDivs { get; set; }

        public string ExtractedLinksText { get; set; }

        public void ParseLinkText(string url)
        {
            URL = url;
            WebClient web = new WebClient();
            web.Encoding = Encoding.UTF8;
            try
            {
                String HTMLText = web.DownloadString(url);
                HTMLText = Regex.Replace(HTMLText, "(</br>)|(<br>)", " ");
                HTMLText = Regex.Replace(HTMLText, @"<span\s?(.*?)>|</span>", "");
                HTMLText = Regex.Replace(HTMLText, "&(.*?);", " ");

                ParseHTMLForText();
            }
            catch (Exception ex)
            { Log(ex); }
        }

        public void Crawl(string start_url, int depth)
        {
            List<String> quee = new List<string>();
            ParseLinkText(start_url);
            quee.AddRange(LINKS);
            SaveEntity();
            LINKS = new List<string>();
            for (int i = 1; i < depth; ++i)
            {
                int quee_length = quee.Count();
                for (int j = 0; j < quee_length; ++j)
                {
                    ParseLinkText(quee[0]);
                    quee.AddRange(LINKS);
                    SaveEntity();
                    LINKS = new List<string>();
                    quee.RemoveAt(0);
                }
            }
        }

        public void CrawlNext(int depth)
        {
            using (PageDBContext pc = new PageDBContext())
            {
                foreach (Link l in Link.SelectLastLinks())
                {
                    foreach (Page p in pc.Pages.Where(pp => pp.PageID == l.ToPage))
                    {
                        Crawl(p.Url, depth);
                    }
                }
            }
        }

        public void SaveEntity()
        {
            PageDBContext pc = new PageDBContext();
            Page temp_page;
            temp_page = new Page(URL);
            List<Page> temp_page_list = new List<Page>();
            for (int i = 0; i < LINKS.Count(); i++)
            {
                Page another_temp_page;
                another_temp_page = new Page(LINKS[i]);
                new Link(temp_page, another_temp_page);
            }

            StringBuilder s_builder = new StringBuilder(ExtractedHeaders);
            s_builder.Append(ExtractedParagraphs);
            s_builder.Append(ExtractedDivs);
            s_builder.Append(ExtractedLinksText);
            string[] words = s_builder.ToString().Split(new char[] { ' ', '\n', '.', ',', '\'', '(', ')', ':', '/', '\\', '[', ']', '\"' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < words.Length; ++i)
            {
                new PageWord(URL, words[i].ToLower(), i + 1);
            }
        }

        private void Log(Exception ex)
        {
            StreamWriter sr = new StreamWriter("log.txt", true);
            StringBuilder to_write = new StringBuilder(DateTime.Now.ToString());
            to_write.Append("\n");
            to_write.Append(URL);
            to_write.Append("\n");
            to_write.Append(ex.Message);
            to_write.Append("\n");
            sr.Write(to_write);
            sr.Close();
        }

        private void ParseHTMLForText()
        {
            StringBuilder res = new StringBuilder();
            ParseLinksChain firstchain = new ParseLinksChain();
            ParseHeadersChain second = new ParseHeadersChain();
            ParseParagraphsChain third = new ParseParagraphsChain();
            ParseDivsChain fourth = new ParseDivsChain();
            ParseLinksTextChain fifth = new ParseLinksTextChain();
            firstchain.SetNext(second);
            second.SetNext(third);
            third.SetNext(fourth);
            fourth.SetNext(fifth);
            firstchain.SendNext(this);
        }
    }
}
