using System.Linq;
using NetConsole.Core.Interfaces;

namespace NetConsole.Core.Actions
{
    public class OrAction : IAction
    {

        # region Public Properties

        public int Status { get; private set; }
        
        public string Message { get; private set; }

        public IAction Left { get; private set; }

        public IAction Right { get; private set; }

        # endregion

        # region Constructors

        public OrAction(IAction left, IAction right)
        {
            Message = "Or action for script";
            Status = 0;
            Left = left;
            Right = right;
        }

        # endregion

        # region Public Methods

        public object[] Perform()
        {
            var leftOutput = Left.Perform();

            if (Left.Status == 0)
                return leftOutput;

            var rightOutput = Right.Perform();

            if (Right.Status != 0)
                Status = Right.Status;

            return leftOutput.Concat(rightOutput).ToArray();
        }

        # endregion
    }
}