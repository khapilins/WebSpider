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
       
        public void ParseLinkText(String url)
        {
            if (_crawler == null) { _crawler = new Crawler(); }
            _crawler.ParseLinkText(url);
        }

        public void Crawl(String start_url, int depth)
        {
            if (_crawler == null) { _crawler = new Crawler(); }
            _crawler.Crawl(start_url, depth);
        }
    }
}
