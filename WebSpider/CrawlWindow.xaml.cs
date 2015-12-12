using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace WebSpider
{
    /// <summary>
    /// Interaction logic for CrawlWindow.xaml
    /// </summary>
    public partial class CrawlWindow : Window
    {
        public static List<Thread> Tasks = new List<Thread>();
        private static List<String> TaskNames = new List<string>();

        public CrawlWindow()
        {
            this.InitializeComponent();
        }

        private void StartCrawlbutton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string tmp_link = StartLinkTextBox.Text;
                int tmp_depth = Int32.Parse(DepthTextBox.Text);
                Thread t = new Thread(() =>
                {
                    Crawler c = new Crawler();
                    c.Crawl(tmp_link, tmp_depth);
                });
                Tasks.Add(t);
                t.Name = tmp_link;
                t.Start();
                ThreadslistBox.BeginInit();
                TaskNames.Add(t.Name);
                ThreadslistBox.DataContext = TaskNames;
                ThreadslistBox.EndInit();
            }
            catch
            {
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ThreadslistBox.BeginInit();            
            ThreadslistBox.DataContext = TaskNames;
            ThreadslistBox.EndInit();
        }

        private void StartCrawlbutton_Copy_Click(object sender, RoutedEventArgs e)
        {
            try
            {                
                int tmp_depth = Int32.Parse(DepthTextBox.Text);
                Thread t = new Thread(() =>
                {
                    Crawler c = new Crawler();
                    c.CrawlNext(tmp_depth);
                });
                Tasks.Add(t);                
                t.Start();
                ThreadslistBox.BeginInit();
                TaskNames.Add("Crawling from last visited links");
                ThreadslistBox.DataContext = TaskNames;
                ThreadslistBox.EndInit();
            }
            catch
            {
            }
        }
    }
}
