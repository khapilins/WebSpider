using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSpider
{
    public class LocationSearcher : SearchTemplate
    {
        public LocationSearcher(String Query) : base(Query)
        {
        }

        public override List<SearchResults> SelectAndRankPages(List<PageWord> page_words)
        {
            using (PageDBContext pc = new PageDBContext())
            {
                var pages_results = (from pw in page_words
                                     group pw by pw into pwGroup
                                     select new SearchResults(pwGroup.Key.ConcretePage, pwGroup.Sum(pw => 1f / pw.Location))).Distinct();

                this.Results = pages_results.Distinct().ToList();
            }

            return this.Results;
        }
    }
}
