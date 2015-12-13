using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Navigation;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebSpider.MyCommand;
using MahApps.Metro.Controls;

namespace WebSpider
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private Invoker invoker = new Invoker();

        public MainWindow()
        {
            this.InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.invoker.ExecuteCommand(new SimpleSearchCommand(new Reciever(), ""));
            SearchProgressRing.Opacity = 0f;
        }

        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            Stopwatch t = new Stopwatch();
            SearchResultslistBox.DataContext = new object();
            SearchProgressRing.BeginInit();
            SearchProgressRing.Opacity = 1f;
            SearchProgressRing.IsActive = true;
            SearchProgressRing.EndInit();
            List<SearchResults> res = null;
            t.Start();
            MyCommand.MyCommand search_command = new SimpleSearchCommand();
            if (FrequencySearchradioButton.IsChecked == true)
            {
                search_command = new FrequencySearchCommand(new Reciever(), SearchQuerytextBox.Text);
            }

            if (SimpleSearchradioButton.IsChecked == true)
            {
                search_command = new SimpleSearchCommand(new Reciever(), SearchQuerytextBox.Text);
            }

            if (LocationSearchradioButton.IsChecked == true)
            {
                search_command = new SearchByLocationCommand(new Reciever(), SearchQuerytextBox.Text);
            }

            await Task.Run(() =>
                        {
                            res = this.invoker.ExecuteCommand(search_command);
                            this.invoker.Current++;
                            Application.Current.Dispatcher.BeginInvoke(new System.Action(() =>
                            {
                                SearchResultslistBox.DataContext = res;
                                SearchProgressRing.Opacity = 0f;
                                t.Stop();
                                ExecutionTimetextBlock.Text = t.ElapsedMilliseconds + "ms";
                            }));
                        });
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

        private void openCrawlWindowButton_Click(object sender, RoutedEventArgs e)
        {
            var w = new CrawlWindow();
            w.Show();
        }

        private void Window_Closed(object sender, System.EventArgs e)
        {
            foreach (var t in CrawlWindow.Tasks)
            {
                t.Abort();
            }
        }

        private void SearchQuerytextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                this.SearchButton_Click(sender, e);
            }
        }
    }
}