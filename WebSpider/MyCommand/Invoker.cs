using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSpider.MyCommand
{
    public class Invoker
    {
        private List<MyCommand> _commands = new List<MyCommand>();

        public List<MyCommand> Commands
        {
            get
            {
                return _commands;
            }

            set
            {
                _commands = value;
            }
        }

        public int Current { get; set; }

        public List<SearchResults> ExecuteCommand(MyCommand cmd)
        {
            _commands.Add(cmd);
            return cmd.Execute();
        }

        public List<SearchResults> Undo()
        {
            if (Current > 0)
            {
                Current--;
                return _commands[Current].Execute();
            }
            else
            {
                return null;
            }
        }

        public List<SearchResults> Redo()
        {
            if (Current < _commands.Count - 1)
            {
                Current++;
                return _commands[Current].Execute();
            }
            else
            {
                return null;
            }
        }
    }
}
