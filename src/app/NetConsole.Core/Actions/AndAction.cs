using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetConsole.Core.Interfaces;

namespace NetConsole.Core.Actions
{
    public class AndAction : IAction
    {
        # region Public Properties

        public int Status { get; private set; }

        public string Message { get; private set; }

        public IAction Left { get; private set; }

        public IAction Right { get; private set; }

        # endregion

        # region Constructors

        public AndAction(IAction left, IAction right)
        {
            Message = "And action for script";
            Status = 0;
            Left = left;
            Right = right;
        }

        # endregion

        public object[] Perform()
        {
            var leftOutput = Left.Perform();

            if (Left.Status != 0)
            {
                Status = Left.Status;
                return leftOutput;
            }

            var rightOutput = Right.Perform();

            if (Right.Status != 0)
                Status = Right.Status;

            return leftOutput.Concat(rightOutput).ToArray();
        }
    }
}
