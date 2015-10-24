using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSpider
{
    public class Page
    {       
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
    } 
}
