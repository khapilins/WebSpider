using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebSpider
{
    public class DBQueryQuee
    {
        public static String _connectionstring = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"C:\\Users\\khapi\\Documents\\Visual Studio 2015\\Projects\\WebSpider\\WebSpider\\Indexes.mdf\";Integrated Security=True;Connect Timeout=300;Min Pool Size=0";
        private Queue _queries = new Queue();
        public Queue Queries;        
        public bool start { get; set; }        


        public DBQueryQuee() { Queries = Queue.Synchronized(_queries); }
        public void Start()
        {
            start = true;
            Thread t = new Thread(Write);
            t.Start();
        }

        private void Write()
        {
            while(start&&Queries.Count>0)
            {                                                              
                    if (Queries.Count >= 1000)
                    {
                        StringBuilder batch = new StringBuilder();
                        batch.Append("BEGIN");
                        batch.Append(Environment.NewLine);
                        for (int i = 0; i < 1000; ++i)
                        {
                            batch.Append(Queries.Dequeue());
                            batch.Append(Environment.NewLine);
                        }
                        batch.Append("END");
                        using (SqlConnection con = new SqlConnection(_connectionstring))
                        {
                            SqlCommand sqlcmd = new SqlCommand();
                            sqlcmd.CommandText = batch.ToString();
                            sqlcmd.Connection = con;
                            sqlcmd.Connection.Open();
                            sqlcmd.ExecuteNonQuery();
                            sqlcmd.Connection.Close();
                        }
                    }
                    else
                    {
                        StringBuilder batch = new StringBuilder();
                        batch.Append("BEGIN");
                        batch.Append(Environment.NewLine);
                        for (int i = 0; i < Queries.Count; ++i)
                        {
                            batch.Append(Queries.Dequeue());
                            batch.Append(Environment.NewLine);
                        }
                        batch.Append("END");
                        using (SqlConnection con = new SqlConnection(_connectionstring))
                        {
                            SqlCommand sqlcmd = new SqlCommand();
                            sqlcmd.CommandText = batch.ToString();
                            sqlcmd.Connection = con;
                            sqlcmd.Connection.Open();
                            sqlcmd.ExecuteNonQuery();
                            sqlcmd.Connection.Close();
                        }
                    }                    
                
            }
        }
    }
}
