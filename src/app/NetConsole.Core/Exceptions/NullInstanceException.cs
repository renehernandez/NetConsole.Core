using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetConsole.Core.Exceptions
{
    public class NullInstanceException : CoreException
    {
        public NullInstanceException() 
            : base("Null instance not allowed")
        {
        }

        public NullInstanceException(string msg) : base(msg)
        {
        }

        public NullInstanceException(string message, Exception e)
            : base(message, e)
        {
        }
    }
}
