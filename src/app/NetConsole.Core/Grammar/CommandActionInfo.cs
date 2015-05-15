using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using NetConsole.Core.Extensions;
using NetConsole.Core.Interfaces;

namespace NetConsole.Core.Grammar
{
    public class CommandActionInfo : ICommandInfo
    {

        public ICommand Command { get; private set; }

        public int Status { get; private set; }

        public string ReturnType { get; private set; }
        
        public MethodInfo Action { get; private set; }
        
        public string Message { get; private set; }

        public object[] Arguments { get; private set; } 

        public CommandActionInfo(string message,  int status, ICommand cmd = null, MethodInfo action = null, object[] paramsInfo = null)
        {
            Status = status;
            Message = message;       
            if (status == 0 && action != null)
            {
                Command = cmd;
                Action = action;
                ReturnType = action.ReturnType.Name;
                Arguments = paramsInfo;
            }
        }

        public object Perform()
        {
            return Command.Perform(Action, Arguments);
        }
    }
}
