using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSpider
{
    public abstract class SearchTemplate
    {
        public SearchTemplate(String Query)
        {
            this.Query = Query;
        }

        public List<SearchResults> Results { get; set; } = new List<SearchResults>();

        public String Query { get; set; }

        public List<Word> SelectWords(string[] words)
        {
            List<Word> res = new List<Word>();
            using (PageDBContext pc = new PageDBContext())
            {
                foreach (var word in words)
                {
                    var stem = Word.Stem(word);
                    res.AddRange((from w in pc.Words
                                  where stem == w.WordStem
                                  select w).ToList());
                }
            }

            return res;
        }

        public List<PageWord> SelectPageWords(List<Word> words)
        {
            List<PageWord> res = new List<PageWord>();
            using (PageDBContext pc = new PageDBContext())
            {
                foreach (Word w in words)
                {
                    res.AddRange((from pw in pc.PageWord
                                  where pw.WordID == w.WordID
                                  select pw).ToList());
                }

                return res;
            }
        }

        public List<SearchResults> NormalizeAndSort(List<SearchResults> results)
        {
            List<SearchResults> res = new List<SearchResults>();
            if (results.Count > 0)
            {
                float small = 0.0000001f;
                float max = results.Max(r => r.Score);
                if (small > max) { max = small; }
                results.ForEach(r => r.Score = 1 - (r.Score / max));
                res = results.OrderBy(r => r.Score).ToList();
            }

            return res;
        }

        public List<SearchResults> Search(string query)
        {
            string[] words = Query.Split(new char[] { ' ', '\n', '.', ',', '\'', '(', ')', ':', '/', '\\', '[', ']', '\"' }, StringSplitOptions.RemoveEmptyEntries);
            var Words = SelectWords(words);
            var PageWords = SelectPageWords(Words);
            Results = SelectAndRankPages(Words, PageWords);
            Results = NormalizeAndSort(Results);
            return Results;
        }

        public abstract List<SearchResults> SelectAndRankPages(List<Word> words, List<PageWord> page_words);
    }
}
