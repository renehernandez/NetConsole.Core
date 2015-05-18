using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetConsole.Core.Exceptions
{
    public class OptionValueNotSetException :CoreException
    {
        public OptionValueNotSetException(string msg, bool defaultMsg = true) 
            : base(defaultMsg
                    ? string.Format("Value not set for {0} option.", msg)
                    : msg
            )
        {
        }

        public OptionValueNotSetException(string message, Exception e) : base(message, e)
        {
        }
    }
}
