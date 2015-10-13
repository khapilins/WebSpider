using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSpider
{
    public class ProxyCrawler:ICrawler
    {
        public Crawler CrawlerObj
        { get
            {
                if (_crawler == null) return _crawler = new Crawler();
                else return _crawler;
            }
            set { _crawler = value; }
        }
        private Crawler _crawler;
        public void ParseLink(String url)
        {
            if (_crawler == null) _crawler = new Crawler();
            _crawler.ParseLink(url);
        }
        public List<String> Crawl(String start_url,int depth)
        {
            if (_crawler == null) _crawler = new Crawler();
            return _crawler.Crawl(start_url, depth);
        }
    }
}
