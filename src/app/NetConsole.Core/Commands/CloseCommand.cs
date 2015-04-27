using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NetConsole.Core.Interfaces;

namespace NetConsole.Core.Commands
{
    public class CloseCommand : ICommand
    {
        # region Properties

        public int Status { get; set; }

        public string Name { get; set; }

        public string Overview { get; set; }

        # endregion

        # region Constructors

        public CloseCommand()
        {
            Name = "close";
            Overview = "This command close the current console instance";
        }

        # endregion
    }
}