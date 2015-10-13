using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSpider
{
    public interface ICrawler
    {
        void ParseLink(string url);
        List<String> Crawl(String start_url,int depth);
    }
}
