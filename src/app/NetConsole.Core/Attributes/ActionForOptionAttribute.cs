using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetConsole.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ActionForOptionAttribute : Attribute
    {
        private readonly string _name;

        public string Name
        {
            get { return _name; }
        }

        public ActionForOptionAttribute(string name)
        {
            _name = name;
        }

    }
}
