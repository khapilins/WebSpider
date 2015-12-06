using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSpider
{
    public class Link
    {
        public Link() { }

        public Link(Page from, Page to)
        {
            using (PageDBContext pc = new PageDBContext())
            {
                Link query = null;
                try
                {
                    query = pc.Link.First(l => l.FromPage == from.PageID && l.ToPage == to.PageID);
                }
                catch (InvalidOperationException iex)
                { }

                if (query == null)
                {
                    this.FromPage = from.PageID;
                    this.ToPage = to.PageID;
                    pc.Link.Add(this);
                    pc.SaveChanges();
                }
                else
                {
                    this.FromPage = query.FromPage;
                    this.ToPage = query.ToPage;
                    this.LinkID = query.LinkID;
                }
            }
        }

        public int LinkID { get; set; }

        public int FromPage { get; set; }

        public int ToPage { get; set; }        
    }
}
