using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Iveonik.Stemmers;

namespace WebSpider
{
    public class Word
    {
        public Word() { }

        public Word(String word)
        {
            using (PageDBContext pc = new PageDBContext())
            {
                Word query = null;
                try
                {
                    query = pc.Words.First(w => w.WordValue == word);
                }
                catch (InvalidOperationException iex)
                { }

                if (query == null)
                {
                    WordValue = word;
                    WordStem = Word.Stem(word);
                    pc.Words.Add(this);
                    pc.SaveChanges();
                }
                else
                {
                    this.WordID = query.WordID;
                    this.WordValue = query.WordValue;
                    this.WordStem = query.WordStem;
                }
            }
        }

        public int WordID { get; set; }

        public String WordValue { get; set; }

        public String WordStem { get; set; }

        public float Probability
        {
            get
            {
                using (PageDBContext pc = new PageDBContext())
                {
                    try
                    {
                        var query = from pw in pc.PageWord
                                    where pw.WordID == this.WordID
                                    select pw;
                        return (float)query.Count() / (float)pc.PageWord.Count();
                    }
                    catch (InvalidOperationException iex)
                    { return 0; }
                }
            }
        }

        public static String Stem(String word)
        {
            if (Regex.IsMatch(word, "^[А-Яа-я]+$"))
            {
                return new RussianStemmer().Stem(word);
            }
            else
            {
                return new EnglishStemmer().Stem(word);
            }
        }
    }
}
