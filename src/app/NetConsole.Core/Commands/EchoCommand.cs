using System;
using System.Linq;
using NetConsole.Core.Attributes;
using NetConsole.Core.Interfaces;

namespace NetConsole.Core.Commands
{
    public class EchoCommand : ICommand
    {

        # region Public 

        public int Status { get; private set; }

        public string Name { get; private set; }

        public string Overview { get; private set; }

        # endregion

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
        public string Echoed(params string[] input)
        {
            string output;
            try
            {
                output = input.Aggregate((accum, curr) => accum + " " + curr);
                Status = 0;
            }
            catch (Exception e)
            {
                Status = 1;
                output = e.Message;
            }
            return output;
        }

        //[DefaultAction]
        //public string Echoed(string input)
        //{
        //    string output = input;
        //    Status = 0;
        //    return output;
        //}

        # endregion
    }
}