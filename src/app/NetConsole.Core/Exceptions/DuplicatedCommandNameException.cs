using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetConsole.Core.Exceptions
{
    public class DuplicatedCommandNameException : CoreException
    {

        public DuplicatedCommandNameException(string cmdName)
            : base(string.Format("{0} command already registered in the factory.", cmdName))
        {

        }
    }
}
