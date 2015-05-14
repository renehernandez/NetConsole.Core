using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetConsole.Core.Exceptions
{
    public class NullCommandInstanceException : CoreException
    {
        public NullCommandInstanceException() 
            : base("Null command instance not allowed")
        {
        }

        public NullCommandInstanceException(string msg) : base(msg)
        {
        }

        public NullCommandInstanceException(string message, Exception e) : base(message, e)
        {
        }
    }
}
