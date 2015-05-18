using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetConsole.Core.Exceptions
{
    public class UndefinedCommandOption : CoreException
    {
        public UndefinedCommandOption(string msg, bool defaultMsg = true)
            : base(defaultMsg 
                    ? string.Format("{0} option is not defined for the command", msg)
                    : msg
            )
        {
        }

        public UndefinedCommandOption(string message, Exception e) : base(message, e)
        {
        }
    }
}
