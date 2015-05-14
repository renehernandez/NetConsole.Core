using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetConsole.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ParamHelpAttribute : Attribute
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

        public ParamHelpAttribute(string name, string content)
        {
            _name = name;
            _content = content;
        }
    }
}
