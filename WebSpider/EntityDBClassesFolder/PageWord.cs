using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSpider
{
    public class PageWord
    {
        public PageWord() { }

        public PageWord(String page, string word, int location)
        {
            using (PageDBContext pc = new PageDBContext())
            {
                Page temp_page = null;
                Word temp_word = null;
                PageWord page_word_query = null;

                temp_page = new Page(page);
                temp_word = new Word(word);

                try
                {
                    page_word_query = pc.PageWord.First(pw => pw.WordID == temp_word.WordID && pw.PageID == temp_page.PageID && Location == location);
                }
                catch (InvalidOperationException iex)
                { }

                if (page_word_query == null)
                {
                    this.Location = location;
                    this.PageID = temp_page.PageID;
                    this.WordID = temp_word.WordID;
                    pc.PageWord.Add(this);
                    pc.SaveChanges();
                }
                else
                {
                    this.WordID = page_word_query.WordID;
                    this.PageWordID = page_word_query.PageWordID;
                    this.PageID = page_word_query.PageID;
                    this.Location = page_word_query.Location;
                }
            }
        }

        public int PageWordID { get; set; }

        public int PageID { get; set; }

        public int WordID { get; set; }

        public int Location { get; set; }

        public Page ConcretePage
        {
            get
            {
                Page res = null;
                using (PageDBContext pc = new PageDBContext())
                {
                    try
                    {
                        res = pc.Pages.Find(this.PageID);
                    }
                    catch (InvalidOperationException iex)
                    { }
                }

                return res;
            }
        }

        public Word ConcreteWord
        {
            get
            {
                Word res = null;
                using (PageDBContext pc = new PageDBContext())
                {
                    try
                    {
                        res = pc.Words.Find(this.WordID);
                    }
                    catch (InvalidOperationException iex)
                    { }
                }

                return res;
            }
        }

        public static List<PageWord> GetPageWordByWord(string word)
        {
            List<PageWord> res = new List<PageWord>();
            using (PageDBContext pc = new PageDBContext())
            {
                int wordId = new Word(word).WordID;
                IEnumerable<PageWord> query = from pw in pc.PageWord
                                              where pw.WordID == wordId
                                              select pw;
                res.AddRange(query);
            }

            return res;
        }

        public static List<PageWord> GetPageWordByStem(string stem)
        {
            List<PageWord> res = new List<PageWord>();
            using (PageDBContext pc = new PageDBContext())
            {
                IEnumerable<Word> words = from w in pc.Words
                                          where w.WordStem == stem
                                          select w;
                foreach (Word w in words)
                {
                    IEnumerable<PageWord> query = from pw in pc.PageWord
                                                  where pw.WordID == w.WordID
                                                  select pw;
                    res.AddRange(query);
                }
            }

            return res;
        }
    }
}
