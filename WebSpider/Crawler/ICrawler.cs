using System;
using System.Collections.Generic;

namespace WebSpider
{
    public interface ICrawler
    {
        void ParseLinkText(string url);

        void Crawl(String start_url, int depth);
    }
}
