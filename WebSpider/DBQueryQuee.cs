using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace WebSpider
{
    public class DBQueryQuee
    {
        public Queue Queries;

        private static String _connectionstring = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"C:\\Users\\khapi\\Documents\\Visual Studio 2015\\Projects\\WebSpider\\WebSpider\\Indexes.mdf\";Integrated Security=True;Connect Timeout=300;Min Pool Size=20";

        private SqlConnection _connection = new SqlConnection(_connectionstring);
                              
        private Queue _queries = new Queue();        

        public DBQueryQuee() { Queries = Queue.Synchronized(_queries); }

        public bool start { get; set; }

        public void Start()
        {
            start = true;
            Thread t = new Thread(Write);
            t.Start();
        }

        private void Write()
        {
            Stopwatch s = new Stopwatch();
            s.Start();
            while (start && Queries.Count > 0)
            {
                if (Queries.Count >= 1500)
                {
                    StringBuilder batch = new StringBuilder();
                    batch.Append("BEGIN");
                    batch.Append(Environment.NewLine);
                    for (int i = 0; i < 1500; ++i)
                    {
                        batch.Append(Queries.Dequeue());
                        batch.Append(Environment.NewLine);
                    }

                    batch.Append("END");
                    try
                    {
                        SqlCommand sqlcmd = new SqlCommand();
                        sqlcmd.CommandText = batch.ToString();
                        sqlcmd.Connection = _connection;
                        sqlcmd.Connection.Open();
                        sqlcmd.ExecuteNonQuery();
                        sqlcmd.Connection.Close();
                    }
                    catch (Exception ex)
                    {
                        _connection.Close();
                        MessageBox.Show(ex.Message);
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
                    try
                    {
                        SqlCommand sqlcmd = new SqlCommand();
                        sqlcmd.CommandText = batch.ToString();
                        sqlcmd.Connection = _connection;
                        sqlcmd.Connection.Open();
                        sqlcmd.ExecuteNonQuery();
                        sqlcmd.Connection.Close();
                    }
                    catch (Exception ex)
                    {
                        _connection.Close();
                        MessageBox.Show(ex.Message);
                    }
                }
            }

            s.Stop();
            MessageBox.Show(s.ElapsedMilliseconds.ToString());
        }
    }
}