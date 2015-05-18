using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetConsole.Core.Attributes;
using NetConsole.Core.Commands;
using NetConsole.Core.Interfaces;

namespace NetConsole.Core.Tests
{
    [NotRegistrable]
    public class NotRegistrableCommand : BaseCommand
    {

        public NotRegistrableCommand()
        {
            Name = "not_registrable";
            Overview = "None";
            Status = 0;
        }

    }
}
