using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetConsole.Core.Attributes
{
    public class OptionHelpAttribute : Attribute
    {
        private readonly string _name;

        private readonly string _content;

        public string Name
        {
            get { return _name; }
        }

        public string Content
        {
            get { return _content; }
        }

        public OptionHelpAttribute(string name, string content)
        {
            _name = name;
            _content = content;
        }
    }
}
