using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSpider
{
    public class PageDBContext : DbContext
    {
        public DbSet<Page> Pages { get; set; }

        public DbSet<Word> Words { get; set; }

        public DbSet<PageWord> PageWord { get; set; }

        public DbSet<Link> Link { get; set; }
    }
}
