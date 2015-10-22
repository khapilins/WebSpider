using System;
using System.Collections.Generic;

namespace WebSpider
{
    public class ProxyCrawler : ICrawler
    {        
        private Crawler _crawler;

        public Crawler CrawlerObj
        {
            get
            {
                if (_crawler == null) { return _crawler = new Crawler(); }
                else { return _crawler; }
            }

            set
            {
                _crawler = value;
            }
        }
       
        public void ParseLink(String url)
        {
            if (_crawler == null) { _crawler = new Crawler(); }
            _crawler.ParseLink(url);
        }

        public List<String> Crawl(String start_url, int depth)
        {
            if (_crawler == null) { _crawler = new Crawler(); }
            return _crawler.Crawl(start_url, depth);
        }
    }
}
