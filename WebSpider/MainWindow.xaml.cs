using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Navigation;
using System.Linq;
using System.Collections.Generic;

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
            // Stopwatch s = new Stopwatch();
            // ProxyCrawler c = new ProxyCrawler();
            // Thread t = new Thread(() =>
            // {
            //    s.Start();
            //    c.Crawl(@"http://ru.wikipedia.org", 1);
            //    s.Stop();
            //    MessageBox.Show(s.ElapsedMilliseconds.ToString());
            // });
            // t.Start();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            Searcher s = new Searcher(SearchQuerytextBox.Text);
            s.SearchByFrequency();                        
            SearchResultslistBox.DataContext = s.Results;
        }

        private void Hyperlink_RequestNavigate(object sender, RoutedEventArgs e)
        {
            Hyperlink h = (Hyperlink)e.Source;                                    
            Process.Start(new ProcessStartInfo(h.DataContext.ToString()));
            e.Handled = true;
        }
    }
}