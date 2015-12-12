using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSpider
{
    public class FrequencySearcher : SearchTemplate
    {
        public FrequencySearcher(String Query) : base(Query)
        {
        }

        /// <summary>
        /// Searching by word frequency on page, more is better
        /// </summary>
        public override List<SearchResults> SelectAndRankPages(List<PageWord> page_words)
        {
            using (PageDBContext pc = new PageDBContext())
            {
                var pages_results = (from p in
                                         from pw in page_words
                                         select pw.ConcretePage
                                     group p by p into pagesGroup
                                     select new SearchResults(pagesGroup.Key, (float)pagesGroup.Count())).Distinct();
                this.Results = pages_results.ToList();
            }

            return this.Results;
        }
    }
}
