using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetConsole.Core.Exceptions
{
    public class DuplicatedNameException : CoreException
    {

        public DuplicatedNameException(string name)
            : base(string.Format("{0} instance already registered in the factory.", name))
        {

        }
    }
}
