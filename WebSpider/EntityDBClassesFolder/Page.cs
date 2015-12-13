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
        public Page() { }

        public Page(string URL, string Title)
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
                    this.PageTitle = Title;
                    pc.Pages.Add(this);
                    pc.SaveChanges();
                }
                else
                {
                    this.PageID = query.PageID;
                    this.Url = query.Url;
                    if (String.IsNullOrEmpty(query.PageTitle))
                    {
                        this.PageTitle = Title;
                        query.PageTitle = Title;
                        pc.SaveChanges();
                    }
                    else
                    {
                        this.PageTitle = query.PageTitle;
                    }
                }
            }
        }

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

        public String PageTitle { get; set; }

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

        public override int GetHashCode()
        {
            return this.Url.GetHashCode();
        }
    }
}
