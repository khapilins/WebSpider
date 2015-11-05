using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSpider.MyCommand
{
    public class SimpleSearchCommand : MyCommand
    {
        public SimpleSearchCommand(Reciever rec, String query) :
            base(rec, query)
        { }

        public override List<SearchResults> Execute()
        {
            return Reciever.Action(this.SearchQuery, this);
        }
    }
}
