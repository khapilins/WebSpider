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

        public List<PageWord> SelectPageWords()
        {
            string[] words = this.Query.ToLowerInvariant().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            List<PageWord> res = new List<PageWord>();
            using (PageDBContext pc = new PageDBContext())
            {
                foreach (var word in words)
                {
                    var stem = Word.Stem(word);
                    var q = from pw in pc.PageWord
                            join w in pc.Words on pw.WordID equals w.WordID
                            join p in pc.Pages on pw.PageID equals p.PageID
                            where w.WordStem == stem
                            select pw;
                    res.AddRange(q);
                }
            }

            return res;
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

        public List<SearchResults> Search()
        {
            string[] words = this.Query.Split(new char[] { ' ', '\n', '.', ',', '\'', '(', ')', ':', '/', '\\', '[', ']', '\"' }, StringSplitOptions.RemoveEmptyEntries);
            var PageWords = SelectPageWords();
            Results = SelectAndRankPages(PageWords);
            Results = NormalizeAndSort(Results);
            return Results;
        }

        public abstract List<SearchResults> SelectAndRankPages(List<PageWord> page_words);
    }
}
