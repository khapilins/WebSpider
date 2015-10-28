using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    pc.Words.Add(this);
                    pc.SaveChanges();
                }
                else
                {
                    this.WordID = query.WordID;
                    this.WordValue = query.WordValue;
                }
            }
        }

        public int WordID { get; set; }

        public String WordValue { get; set; }

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
    }
}
