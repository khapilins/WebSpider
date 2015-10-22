using System;
using System.Collections.Generic;

namespace WebSpider
{
    public interface ICrawler
    {
        void ParseLink(string url);

        List<String> Crawl(String start_url, int depth);
    }
}
