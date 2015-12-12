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
        public override List<SearchResults> SelectAndRankPages(List<Word> words, List<PageWord> page_words)
        {
            List<Page> foundPages = new List<Page>();
            foreach (PageWord pw in page_words)
            {
                if (!foundPages.Contains(pw.ConcretePage))
                {
                    foundPages.Add(pw.ConcretePage);
                }
            }

            ////Dictionary<Word, float> Probs = new Dictionary<Word, float>();
            ////foreach (Word w in words)
            ////{
            ////    Probs.Add(w, w.Probability);
            ////}

            foreach (Page page in foundPages)
            {
                SearchResults s = new SearchResults(page, 0);
                foreach (Word word in words)
                {
                    foreach (PageWord pageWord in page_words)
                    {
                        if (pageWord.WordID == word.WordID && pageWord.PageID == page.PageID)
                        {
                            s.Score += 1;
                        }
                    }
                }

                this.Results.Add(s);
            }

            return this.Results;
        }
    }
}
