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
            if (cmd is SimpleSearchCommand)
            {
                var s = new SimpleSearcher(query);
                s.Search();
                return s.Results;
            }

            if (cmd is FrequencySearchCommand)
            {
                var s = new FrequencySearcher(query);
                s.Search();
                return s.Results;
            }

            if (cmd is SearchByLocationCommand)
            {
                ////var s = new Searcher(query);
                ////s.SearchByLocation();
                ////return s.Results;
                var s = new LocationSearcher(query);
                s.Search();
                return s.Results;
            }
            else { return null; }
        }
    }
}
