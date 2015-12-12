using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSpider
{
    public class SimpleSearcher : SearchTemplate
    {
        public SimpleSearcher(String Query) : base(Query)
        {
        }

        public override List<SearchResults> SelectAndRankPages(List<PageWord> page_words)
        {
            using (PageDBContext pc = new PageDBContext())
            {
                var pages = (from p in
                                 from pw in page_words
                                 select pw.ConcretePage
                             select p).Distinct();
                foreach (Page p in pages)
                {
                    this.Results.Add(new SearchResults(p, 1));
                }
            }

            return this.Results;
        }
    }
}
