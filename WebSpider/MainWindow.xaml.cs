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
            ////Stopwatch s = new Stopwatch();
            ////Thread t = new Thread(d =>
            ////{
            ////Crawler c = new Crawler();
            ////s.Start();
            ////c.CrawlNext(2);
            ////s.Stop();
            ////MessageBox.Show(s.ElapsedMilliseconds.ToString());
            ////});
            Thread t = new Thread(s =>
            {
                using (PageDBContext pc = new PageDBContext())
                {
                    var query = from p in pc.Pages
                                select p;
                    foreach (var a in query)
                    {
                        var b = a;
                    }

                    var wquery = from w in pc.Words
                                 select w;
                    foreach (var a in wquery)
                    {
                        var b = a;
                    }

                    var pwquery = from pw in pc.Words
                                  select pw;
                    foreach (var a in pwquery)
                    {
                        var b = a;
                    }
                }
            });
            t.Start();
            this.invoker.ExecuteCommand(new SimpleSearchCommand(new Reciever(), ""));
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            Stopwatch t = new Stopwatch();
            t.Start();
            Searcher s = new Searcher(SearchQuerytextBox.Text);
            s.SearchByFrequency();
            SearchResultslistBox.DataContext = s.Results;
            if (FrequencySearchradioButton.IsChecked == true)
            {
                this.invoker.ExecuteCommand(new FrequencySearchCommand(new Reciever(), SearchQuerytextBox.Text));
                this.invoker.Current++;
            }

            if (SimpleSearchradioButton.IsChecked == true)
            {
                SearchResultslistBox.DataContext = this.invoker.ExecuteCommand(new SimpleSearchCommand(new Reciever(), SearchQuerytextBox.Text));
                this.invoker.Current++;
            }

            if (LocationSearchradioButton.IsChecked == true)
            {
                SearchResultslistBox.DataContext = this.invoker.ExecuteCommand(new SearchByLocationCommand(new Reciever(), SearchQuerytextBox.Text));
                this.invoker.Current++;
            }

            t.Stop();
            ExecutionTimetextBlock.Text = t.ElapsedMilliseconds.ToString() + "ms";
        }

        private void Hyperlink_RequestNavigate(object sender, RoutedEventArgs e)
        {
            Hyperlink h = (Hyperlink)e.Source;
            Process.Start(new ProcessStartInfo(h.DataContext.ToString()));
            e.Handled = true;
        }

        private void Undobutton_Click(object sender, RoutedEventArgs e)
        {
            Stopwatch t = new Stopwatch();
            t.Start();
            SearchResultslistBox.DataContext = this.invoker.Undo();
            t.Stop();
            ExecutionTimetextBlock.Text = t.ElapsedMilliseconds.ToString() + "ms";
        }

        private void Redobutton_Click(object sender, RoutedEventArgs e)
        {
            Stopwatch t = new Stopwatch();
            t.Start();
            SearchResultslistBox.DataContext = this.invoker.Redo();
            t.Stop();
            ExecutionTimetextBlock.Text = t.ElapsedMilliseconds.ToString() + "ms";
        }
    }
}