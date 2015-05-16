using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetConsole.Core.Exceptions
{
    public class DeclarableOnlyOptionException : CoreException
    {

        public DeclarableOnlyOptionException(string msg, bool defaultMsg = true) : 
            base(defaultMsg
                ? string.Format("{0} does not need a value.")
                :msg
            )
        {
        }

        public DeclarableOnlyOptionException(string message, Exception e) : base(message, e)
        {
        }
    }
}
