using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebSpider
{
    public class Crawler : ICrawler
    {        
        public static String ConnectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"C:\\Users\\khapi\\Documents\\Visual Studio 2015\\Projects\\WebSpider\\WebSpider\\Indexes.mdf\";Integrated Security=True;Connect Timeout=120";

        private List<String> _links = new List<string>();

        public List<string> LINKS { get { return _links; } set { _links = value; } }

        public String HTMLText { get; set; }

        public String URL { get; set; }

        public string ExtractedParagraphs { get; set; }

        public string ExtractedHeaders { get; set; }

        public string ExtractedDivs { get; set; }

        public string ExtractedLinksText { get; set; }

        public void ParseLink(string url)
        {
            URL = url;
            WebClient web = new WebClient();
            web.Encoding = Encoding.UTF8;
            try
            {
                HTMLText = web.DownloadString(url);
                HTMLText = Regex.Replace(HTMLText, "(</br>)|(<br>)", " ");
                HTMLText = Regex.Replace(HTMLText, @"<span\s?(.*?)>|</span>", "");
                HTMLText = Regex.Replace(HTMLText, "&(.*?);", " ");
                ParseHTML();
            }
            catch (Exception ex)
            { Log(ex); }
        }

        public List<String> Crawl(string start_url, int depth)
        {
            List<String> quee = new List<string>();
            List<string> res = new List<string>();
            ParseLink(start_url);
            quee.AddRange(LINKS);
            res.AddRange(LINKS);
            DBQueryQuee db_Quee = new DBQueryQuee();
            db_Quee.Start();
            FlushToDB(db_Quee);

            // LINKS = new List<string>();
            for (int i = 1; i < depth; ++i)
            {
                int quee_length = quee.Count();
                for (int j = 0; j < quee_length; ++j)
                {
                    ParseLink(quee[0]);
                    res.Add(quee[0]);
                    quee.AddRange(LINKS);
                    FlushToDB(db_Quee);

                    // LINKS = new List<string>();
                    quee.RemoveAt(0);
                }
            }

            res.AddRange(quee);
            return res;
        }

        private void ParseHTML()
        {
            ParseLinksChain firstchain = new ParseLinksChain();
            ParseHeadersChain second = new ParseHeadersChain();
            ParseParagraphsChain third = new ParseParagraphsChain();
            ParseDivsChain fourth = new ParseDivsChain();
            ParseLinksTextChain fifth = new ParseLinksTextChain();
            firstchain.SetNext(second);
            second.SetNext(third);
            third.SetNext(fourth);
            fourth.SetNext(fifth);
            firstchain.SendNext(this);
        }

        private void FlushToDB(DBQueryQuee db_quee)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                Guid current_url_Guid = Guid.NewGuid();
                FlushURL(current_url_Guid, db_quee);
                FlushWords(current_url_Guid, db_quee);
            }

            LINKS = new List<string>();
            ExtractedDivs = "";
            ExtractedHeaders = "";
            ExtractedLinksText = "";
            ExtractedParagraphs = "";
        }

        private void Log(Exception ex)
        {
            StreamWriter sr = new StreamWriter("log.txt", true);
            StringBuilder to_write = new StringBuilder(DateTime.Now.ToString());
            to_write.Append("\n");
            to_write.Append(URL);
            to_write.Append("\n");
            to_write.Append(ex.Message);
            to_write.Append("\n");
            sr.Write(to_write);
            sr.Close();
        }

        private void FlushURL(Guid current_url_Guid, DBQueryQuee db_quee)
        {
            Guid[] next_url_ids = new Guid[LINKS.Count];
            StringBuilder batch = new StringBuilder();

            // batch.Append(Environment.NewLine);
               
            // using (SqlConnection con = new SqlConnection(_connectionstring))
            // {
            // SqlCommand sqlcmd = new SqlCommand();
            // sqlcmd.Connection = con;
            batch.Append("if not exists(select * from UrlList where url='");
            batch.Append(URL);
            batch.Append("') INSERT INTO URLLIST (Id, url) values ('");
            batch.Append(current_url_Guid);
            batch.Append("', '");
            batch.Append(URL);
            batch.Append("') ");

            // batch.Append(Environment.NewLine);            
            db_quee.Queries.Enqueue(batch.ToString());
            batch = new StringBuilder();
            for (int i = 0; i < next_url_ids.Length; ++i)
            {
                next_url_ids[i] = Guid.NewGuid();
                batch.Append("if not exists(select * from UrlList where url='");
                batch.Append(LINKS[i]);
                batch.Append("') INSERT INTO URLLIST (Id, url) values ('");
                batch.Append(next_url_ids[i]);
                batch.Append("', '");
                batch.Append(LINKS[i]);
                batch.Append("')");
                db_quee.Queries.Enqueue(batch.ToString());

                // batch.Append(Environment.NewLine);
                batch = new StringBuilder();

                batch.Append("if not exists(select * from Link where FromID='");
                batch.Append(current_url_Guid + "' and ToID='" + next_url_ids[i]);
                batch.Append("')  INSERT INTO LINK (Id, FromID,ToID) values ('");
                batch.Append(Guid.NewGuid() + "','" + current_url_Guid + "','" + next_url_ids[i] + "')");
                db_quee.Queries.Enqueue(batch.ToString());
                batch = new StringBuilder();

                // batch.Append(Environment.NewLine);                    
            }

            // batch.Append("END");
            // sqlcmd.CommandText=batch.ToString();
            // sqlcmd.Connection.Open();
            // sqlcmd.ExecuteNonQuery();
            // sqlcmd.Connection.Close();
            // }
        }

        private void FlushWords(Guid current_url_Guid, DBQueryQuee db_quee)
        {
            StringBuilder batch = new StringBuilder();

            // batch.Append(Environment.NewLine);            
            // batch.Append(Environment.NewLine);
            // batch.Append("(select TOP 1 * from UrlList )");
            // batch.Append(Environment.NewLine);
            StringBuilder s_builder = new StringBuilder(ExtractedHeaders);
            s_builder.Append(ExtractedParagraphs);
            s_builder.Append(ExtractedDivs);
            s_builder.Append(ExtractedLinksText);
            string[] words = s_builder.ToString().Split(new char[] { ' ', '\n', '.', ',', '\'', '(', ')', ':', '/', '\\' }, StringSplitOptions.RemoveEmptyEntries);
            Guid[] word_ids = new Guid[words.Length];
            for (int i = 0; i < word_ids.Length; ++i)
            {
                word_ids[i] = Guid.NewGuid();
                batch.Append("if not exists(select * from WordList where word=N'");
                batch.Append(words[i].ToLower());
                batch.Append("') INSERT INTO WordList (Id, Word) values ('");
                batch.Append(word_ids[i] + "', N'" + words[i].ToLower());
                batch.Append("')");
                db_quee.Queries.Enqueue(batch.ToString());
                batch = new StringBuilder();

                // batch.Append(Environment.NewLine);
            }

            for (int i = 0; i < word_ids.Length; ++i)
            {
                batch.Append("if not exists(select * from WordLocation where UrlID='");
                batch.Append(current_url_Guid);
                batch.Append("' and ");
                batch.Append("WordID='");
                batch.Append(word_ids[i] + "' and WordLocation='" + (i + 1).ToString());
                batch.Append("') INSERT INTO WordLocation (Id, WordID,UrlID,WordLocation) values ('");
                batch.Append(Guid.NewGuid() + "', '" + word_ids[i] + "', '" + current_url_Guid + "', '" + (i + 1).ToString() + "')");
                db_quee.Queries.Enqueue(batch.ToString());
                batch = new StringBuilder();

                // batch.Append(Environment.NewLine);
            }

            // using (SqlConnection con = new SqlConnection(_connectionstring))
            // {
            //     //batch.Append("END");
            //     //SqlCommand sqlcmd = new SqlCommand();///////////////////////////////////
            //     //sqlcmd.Connection = con;
            //     //sqlcmd.CommandTimeout += sqlcmd.CommandTimeout;
            //     //sqlcmd.CommandText = batch.ToString();////////////////////////////////////
            //     //sqlcmd.Connection.Open();
            //     //sqlcmd.ExecuteNonQuery();        /////////////////////////////////////
            //     //sqlcmd.Connection.Close();
            // }
        }
    }
}
