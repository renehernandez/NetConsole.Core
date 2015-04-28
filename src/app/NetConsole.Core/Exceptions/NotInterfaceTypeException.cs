using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetConsole.Core.Exceptions
{
    public class NotInterfaceTypeException : CoreException
    {

        # region Constructors

        public NotInterfaceTypeException() : base("Expected to be called with a generic T representing an interface")
        {
            
        }

        public NotInterfaceTypeException(string message) : base(message)
        {

        }

        # endregion

    }
}
