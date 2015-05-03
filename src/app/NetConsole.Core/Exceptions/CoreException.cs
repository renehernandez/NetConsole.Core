using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetConsole.Core.Exceptions
{
    public class CoreException : Exception
    {

        public CoreException(string msg) : base(msg)
        {

        }

        public CoreException(string message, Exception e) : base(message, e)
        {
            
        }

    }
}
