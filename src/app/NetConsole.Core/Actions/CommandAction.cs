using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using NetConsole.Core.Extensions;
using NetConsole.Core.Interfaces;

namespace NetConsole.Core.Actions
{
    public class CommandAction : IAction
    {

        # region Public Properties

        public ICommand Command { get; private set; }

        public int Status { get; private set; }

        public MethodInfo[] Body { get; private set; }
        
        public string Message { get; private set; }

        public object[] Arguments { get; private set; }

        public string Name
        {
            get { return Body[0].Name; }
        }

        # endregion

        # region Constructors

        public CommandAction(string message,  int status, ICommand cmd = null, MethodInfo action = null, object[] args = null)
        {
            Status = status;
            Message = message;       
            if (status == 0 && action != null)
            {
                Command = cmd;
                Body = new[] {action};
                Arguments = args;
            }
        }

        public CommandAction(ICommand cmd, MethodInfo action, object[] args)
        {
            Message = "Action for script";
            Status = 0;
            Body = new []{action};
            Arguments = args;
            Command = cmd;
        }

        # endregion

        # region Public Methods

        public object Perform()
        {
            var output = Command.Perform(Body[0], Arguments);
            Status = Command.Status;
            return output;
        }

        # endregion
    }
}
