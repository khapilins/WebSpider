using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSpider.MyCommand
{
    public class Reciever
    {
        public List<SearchResults> Action(string query, MyCommand cmd)
        {
            Searcher s = new Searcher(query);
            if (cmd is SimpleSearchCommand)
            {
                s.SimpleSearch();
                return s.Results;
            }
            else
            {
                s.SearchByFrequency();
                return s.Results;
            }
        }
    }
}
