using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using NetConsole.Core.Interfaces;

namespace NetConsole.Core.Actions
{
    public class PipeAction : IAction
    {

        # region Public Properties

        public int Status { get; private set; }
        
        public object[] Arguments { get; private set; }
        
        public CommandAction[] Sequence { get; private set; }

        public string Message { get; private set; }

        # endregion

        # region Constructors

        public PipeAction(CommandAction[] sequence, object[] args)
        {
            Status = 0;
            Arguments = args;
            Sequence = sequence;
            Message = "Pipe action for script";
        }

        # endregion

        # region Public Methods

        public object[] Perform()
        {
            Sequence[0].Arguments = Arguments;
            object[] output = Sequence[0].Perform();

            if (Sequence[0].Status != 0)
            {
                Status = Sequence[0].Status;
                return output;
            }

            foreach (var commandAction in Sequence.Skip(1))
            {
                commandAction.Arguments = new[] {output[0]};
                output = commandAction.Perform();

                if (commandAction.Status != 0)
                {
                    Status = commandAction.Status;
                    break;
                }
            }

            return output;
        }

        # endregion
    }
}
