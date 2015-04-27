using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetConsole.Core.Exceptions
{
    public class UnregisteredCommandException : CoreException
    {

        public UnregisteredCommandException(string cmdName) : base(string.Format("Command {0} is not registered in the factory.", cmdName))
        {
        }

    }
}
