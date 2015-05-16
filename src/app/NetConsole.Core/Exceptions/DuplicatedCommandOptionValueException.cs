using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetConsole.Core.Exceptions
{
    public class DuplicatedCommandOptionValueException : CoreException
    {
        public DuplicatedCommandOptionValueException(string msg, bool defaultMsg = true) 
            : base(defaultMsg
                ? string.Format("There is a value saved for {0} option", msg)
                : msg
            )
        {
        }

        public DuplicatedCommandOptionValueException(string message, Exception e) : base(message, e)
        {
        }
    }
}
