using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iveonik.Stemmers;

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
                        this.Results.Add(new SearchResults(p, 1));
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
                foundPageWord.AddRange(PageWord.GetPageWordByStem(words[i].WordStem));
            }

            Dictionary<Word, float> Probabilities = new Dictionary<Word, float>();
            foreach (Word w in words)
            {
                Probabilities.Add(w, w.Probability);
            }

            foreach (PageWord pw in foundPageWord)
            {
                if (!foundPages.Contains(pw.ConcretePage))
                {
                    foundPages.Add(pw.ConcretePage);
                }
            }

            foreach (Page page in foundPages)
            {
                SearchResults s = new SearchResults(page, 0);
                foreach (Word word in words)
                {
                    foreach (PageWord pageWord in foundPageWord)
                    {
                        if (pageWord.WordID == word.WordID && pageWord.PageID == page.PageID)
                        {
                            s.Score += 1 - Probabilities[word];
                        }
                    }
                }

                this.Results.Add(s);
            }

            this.NormaliseAndSortResults();
        }

        public void SearchByLocation()
        {
            this.Results = new List<SearchResults>();
            List<String> urls = new List<string>();
            List<Word> words = this.SplitQuery();
            List<PageWord> foundPageWord = new List<PageWord>();
            List<Page> foundPages = new List<Page>();
            for (int i = 0; i < words.Count(); i++)
            {
                foundPageWord.AddRange(PageWord.GetPageWordByStem(words[i].WordStem));
            }

            Dictionary<Word, float> Probabilities = new Dictionary<Word, float>();
            foreach (Word w in words)
            {
                Probabilities.Add(w, w.Probability);
            }

            foreach (PageWord pw in foundPageWord)
            {
                if (!foundPages.Contains(pw.ConcretePage))
                {
                    foundPages.Add(pw.ConcretePage);
                }
            }

            foreach (Page page in foundPages)
            {
                SearchResults s = new SearchResults(page, 0);
                foreach (Word word in words)
                {
                    foreach (PageWord pageWord in foundPageWord)
                    {
                        if (pageWord.WordID == word.WordID && pageWord.PageID == page.PageID)
                        {
                            s.Score += (1 - Probabilities[word]) * (1.0f / pageWord.Location);
                        }
                    }
                }

                this.Results.Add(s);
            }

            this.NormaliseAndSortResults();
        }

        private void NormaliseAndSortResults()
        {
            if (Results.Count > 0)
            {
                float small = 0.0000001f;
                float max = Results.Max(r => r.Score);
                if (small > max) { max = small; }
                Results.ForEach(r => r.Score = 1 - (r.Score / max));
                Results = Results.OrderBy(r => r.Score).ToList();
            }
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

        private List<int> GetWordsIDs()
        {
            List<int> res = new List<int>();
            string[] words = Query.Split(new char[] { ' ', '\n', '.', ',', '\'', '(', ')', ':', '/', '\\', '[', ']', '\"' }, StringSplitOptions.RemoveEmptyEntries);
            using (PageDBContext pc = new PageDBContext())
            {
                for (int i = 0; i < words.Length; ++i)
                {
                    res.AddRange(from w in pc.Words
                                 where w.WordStem == Word.Stem(words[i])
                                 select w.WordID);
                }
            }

            return res;
        }
    }

    public class SearchResults
    {
        public SearchResults(Page res_page, float score)
        {
            this.ResultPage = res_page;
            this.Score = score;
        }

        public Page ResultPage { get; set; }

        public string ResultLink
        {
            get
            {
                return ResultPage.Url;
            }
        }

        public String PageTitle
        {
            get
            {
                return ResultPage.PageTitle;
            }
        }

        public float Score { get; set; }

        public override string ToString()
        {
            return this.ResultLink;
        }
    }
}