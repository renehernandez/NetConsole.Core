using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetConsole.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ActionHelpAttribute : Attribute
    {

        private readonly string _content;

        public string Content
        {
            get { return _content; }
        }

        public ActionHelpAttribute(string content)
        {
            _content = content;
        }

    }
}
