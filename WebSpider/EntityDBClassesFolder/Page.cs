using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebSpider
{
    public class Page
    {
        private string _pageTitle;

        public Page() { }

        public Page(string URL)
        {
            using (PageDBContext pc = new PageDBContext())
            {
                Page query = null;
                try
                {
                    query = pc.Pages.First(p => p.Url == URL);
                }
                catch (InvalidOperationException iex)
                { }

                if (query == null)
                {
                    this.Url = URL;
                    pc.Pages.Add(this);
                    pc.SaveChanges();
                }
                else
                {
                    this.PageID = query.PageID;
                    this.Url = query.Url;
                }
            }
        }
       
        public int PageID { get; set; }

        public string Url { get; set; }

        public String PageTitle
        {
            get
            {
                if (String.IsNullOrEmpty(_pageTitle))
                {                 
                    WebClient web = new WebClient();
                    web.Encoding = Encoding.UTF8;
                    try
                    {
                        String HTMLText = web.DownloadString(this.Url);
                        HTMLText = Regex.Replace(HTMLText, "(</br>)|(<br>)", " ");
                        HTMLText = Regex.Replace(HTMLText, @"<span\s?(.*?)>|</span>", "");
                        HTMLText = Regex.Replace(HTMLText, "&(.*?);", " ");

                        MatchCollection regex_reults = Regex.Matches(HTMLText, @"<title\s?(.*?)>(.*?)</", RegexOptions.Multiline);
                        StringBuilder s = new StringBuilder();
                        string subres = regex_reults[0].Groups[2].Value;
                        _pageTitle = subres;
                    }
                    catch
                    { }                
                }

                return _pageTitle;                
            }

            set
            {
                _pageTitle = value;
            }
        }

        public override bool Equals(object obj)
        {
            try
            {
                if (this.PageID == ((Page)obj).PageID)
                {
                    return true;
                }
                else { return false; }
            }
            catch
            {
                return false;
            }
        }
    }
}
