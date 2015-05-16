using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetConsole.Core.Exceptions
{
    public class FailedUnregisterOperationException : CoreException
    {

        public FailedUnregisterOperationException(string name)
            : base(string.Format("Unable to unregister {0} object because is not presented in the factory", name))
        {

        }
    }
}
