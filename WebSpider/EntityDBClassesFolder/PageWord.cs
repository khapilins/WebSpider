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
    }
}
