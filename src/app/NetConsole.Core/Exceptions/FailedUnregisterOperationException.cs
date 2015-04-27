using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetConsole.Core.Exceptions
{
    public class FailedUnregisterOperationException : CoreException
    {

        public FailedUnregisterOperationException(string cmdName)
            : base(string.Format("Unable to unregister {0} command because is not presented in the factory", cmdName))
        {

        }
    }
}
