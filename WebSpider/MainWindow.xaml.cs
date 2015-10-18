using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WebSpider
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // using (SqlConnection con = new SqlConnection(Crawler._connectionstring))
            // {
            // SqlCommand sqlcmd = new SqlCommand();
            // sqlcmd.Connection = con;
            // sqlcmd.CommandText = "Truncate table UrlList";
            // sqlcmd.Connection.Open();
            // sqlcmd.ExecuteNonQuery();
            // sqlcmd.Connection.Close();
            // sqlcmd.CommandText = "Truncate table Link";
            // sqlcmd.Connection.Open();
            // sqlcmd.ExecuteNonQuery();
            // sqlcmd.Connection.Close();
            // sqlcmd.CommandText = "Truncate table WordList";
            // sqlcmd.Connection.Open();
            // sqlcmd.ExecuteNonQuery();
            // sqlcmd.Connection.Close();
            // sqlcmd.CommandText = "Truncate table Wordlocation";
            // sqlcmd.Connection.Open();
            // sqlcmd.ExecuteNonQuery();
            // sqlcmd.Connection.Close();
            // }
            Stopwatch s = new Stopwatch();
            ProxyCrawler c = new ProxyCrawler();
            Thread t = new Thread(() => 
            {
                s.Start();
                c.Crawl(@"http://ru.wikipedia.org", 1);
                s.Stop();
                MessageBox.Show(s.ElapsedMilliseconds.ToString());
            });
            t.Start();
            Thread.Sleep(10000);
        }
    }
}
