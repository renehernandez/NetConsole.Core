using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetConsole.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CommandHelpAttribute : Attribute
    {
        private readonly string _content;

        public string Content
        {
            get { return _content; }
        }

        public CommandHelpAttribute(string content)
        {
            _content = content;
        }

    }
}
