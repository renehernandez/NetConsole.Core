using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetConsole.Core.Interfaces;

namespace NetConsole.Core.Grammar
{
    public class ReturnInfo : IReturnInfo
    {
        public int Status { get; private set; }

        public string Type { get; private set; }
        
        public object Output { get; private set; }

        public ReturnInfo(object output, int status)
        {
            Type = output.GetType().Name;
            Output = output;
            Status = status;
        }
    }
}
