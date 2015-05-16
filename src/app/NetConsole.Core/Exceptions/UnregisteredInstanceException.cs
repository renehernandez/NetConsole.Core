using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetConsole.Core.Exceptions
{
    public class UnregisteredInstanceException : CoreException
    {

        public UnregisteredInstanceException(string name) : base(string.Format("{0} instance is not registered in the factory.", name))
        {
        }

    }
}
