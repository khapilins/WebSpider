using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSpider
{
    public class Searcher
    {
        public List<SearchResults> Results = new List<SearchResults>();

        public Searcher(string query)
        {
            this.Query = query;
        }

        public string Query { get; set; }

        /// <summary>
        /// Simplest search, return page if all words were found there
        /// </summary>
        public void SimpleSearch()
        {
            this.Results = new List<SearchResults>();
            List<String> urls = new List<string>();
            List<Word> words = this.SplitQuery();
            List<PageWord> foundPageWord = new List<PageWord>();
            List<Page> foundPages = new List<Page>();
            for (int i = 0; i < words.Count(); i++)
            {
                foundPageWord.AddRange(PageWord.GetPageWordByWord(words[i].WordValue));
            }

            foreach (PageWord pw in foundPageWord)
            {
                foundPages.Add(pw.ConcretePage);
            }

            foreach (Page p in foundPages)
            {
                if (words.All(w => foundPageWord.Any(paw => paw.WordID == w.WordID && paw.PageID == p.PageID)))
                {
                    if (!urls.Contains(p.Url))
                    {
                        urls.Add(p.Url);
                        this.Results.Add(new SearchResults(p.Url, 1));
                    }
                }
                ////foundPages.Add(pw.ConcretePage);                
            }
        }

        /// <summary>
        /// Searching by word frequency on page, more is better
        /// </summary>
        public void SearchByFrequency()
        {
            this.Results = new List<SearchResults>();
            List<String> urls = new List<string>();
            List<Word> words = this.SplitQuery();
            List<PageWord> foundPageWord = new List<PageWord>();
            List<Page> foundPages = new List<Page>();
            for (int i = 0; i < words.Count(); i++)
            {
                foundPageWord.AddRange(PageWord.GetPageWordByWord(words[i].WordValue));
            }

            foreach (PageWord pw in foundPageWord)
            {
                foundPages.Add(pw.ConcretePage);
            }

            foreach (Page page in foundPages)
            {
                SearchResults s = new SearchResults(page.Url, 0);
                foreach (Word word in words)
                {
                    foreach (PageWord pageWord in foundPageWord)
                    {
                        if (pageWord.WordID == word.WordID && pageWord.PageID == page.PageID)
                        {
                            s.Score++;
                        }
                    }
                }

                this.Results.Add(s);
            }

            this.NormaliseAndSortResults();
        }

        private void NormaliseAndSortResults()
        {
            float small = 0.0000001f;
            float max = Results.Max(r => r.Score);
            if (small > max) { max = small; }

            Results.ForEach(r => r.Score = 1 - (r.Score / max));
            Results = Results.OrderBy(r => r.Score).ToList();
        }

        private List<Word> SplitQuery()
        {
            string[] words = Query.Split(new char[] { ' ', '\n', '.', ',', '\'', '(', ')', ':', '/', '\\', '[', ']', '\"' }, StringSplitOptions.RemoveEmptyEntries);
            List<Word> res = new List<Word>();
            for (int i = 0; i < words.Length; ++i)
            {
                res.Add(new Word(words[i].ToLower()));
            }

            return res;
        }
    }

    public class SearchResults
    {
        public SearchResults(string link, float score)
        {
            this.ResultLink = link;
            this.Score = score;
        }

        public string ResultLink { get; set; }

        public float Score { get; set; }

        public override string ToString()
        {
            return this.ResultLink;
        }
    }
}