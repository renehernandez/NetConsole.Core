using System;
using System.Linq;
using NetConsole.Core.Attributes;
using NetConsole.Core.Commands;

namespace NetConsole.Core.Tests
{
    public class EchoCommand : BaseCommand
    {

        # region Constructors

        public EchoCommand()
        {
            Name = "echo";
            Overview = "It returns the same input to the console";
            Status = 0;
        }

        # endregion

        # region Public Methods

        [DefaultAction]
        [ActionHelp("Redirects the standard input to the standard output int the console")]
        public string Echoed(params object[] input)
        {
            string output;
            try
            {
                output = input.Select(x => x.ToString()).Aggregate((accum, curr) => accum + " " + curr);
                Status = 0;
            }
            catch (Exception e)
            {
                Status = 1;
                output = e.Message;
            }
            return output;
        }

        # endregion
    }
}