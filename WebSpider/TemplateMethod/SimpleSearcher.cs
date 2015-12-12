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

        public override List<SearchResults> SelectAndRankPages(List<Word> words, List<PageWord> page_words)
        {
            foreach (Page p in from pw in page_words
                               select pw.ConcretePage)
            {
                if (words.Any(w => page_words.Any(paw => paw.WordID == w.WordID && paw.PageID == p.PageID)))
                {
                    this.Results.Add(new SearchResults(p, 1));
                }                          
            }

            return this.Results;
        }
    }
}
