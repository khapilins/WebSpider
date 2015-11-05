using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Navigation;
using System.Linq;
using System.Collections.Generic;
using WebSpider.MyCommand;

namespace WebSpider
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Invoker invoker = new Invoker();

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
            ////Crawler c = new Crawler();
            //// c.CrawlNext(1);
            this.invoker.ExecuteCommand(new SimpleSearchCommand(new Reciever(), ""));            
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            Searcher s = new Searcher(SearchQuerytextBox.Text);
            s.SearchByFrequency();
            SearchResultslistBox.DataContext = s.Results;
            if (FrequencySearchradioButton.IsChecked == true)
            {
                this.invoker.ExecuteCommand(new FrequencySearchCommand(new Reciever(), SearchQuerytextBox.Text));
            }
            else
            {
                SearchResultslistBox.DataContext = this.invoker.ExecuteCommand(new SimpleSearchCommand(new Reciever(), SearchQuerytextBox.Text));
            }
        }

        private void Hyperlink_RequestNavigate(object sender, RoutedEventArgs e)
        {
            Hyperlink h = (Hyperlink)e.Source;
            Process.Start(new ProcessStartInfo(h.DataContext.ToString()));
            e.Handled = true;
        }

        private void Undobutton_Click(object sender, RoutedEventArgs e)
        {
            SearchResultslistBox.DataContext = this.invoker.Undo();
        }

        private void Redobutton_Click(object sender, RoutedEventArgs e)
        {
            SearchResultslistBox.DataContext = this.invoker.Redo();
        }
    }
}