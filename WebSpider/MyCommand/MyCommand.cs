using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSpider.MyCommand
{
    public abstract class MyCommand
    {        
        public MyCommand()
        { }

        public MyCommand(Reciever rec, String query)
        {
            Reciever = rec;
            SearchQuery = query;
        }

        public Reciever Reciever { get; set; }

        public String SearchQuery { get; set; }

        public abstract List<SearchResults> Execute();                        
    }
}
