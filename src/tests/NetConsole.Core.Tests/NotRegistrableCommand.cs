using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetConsole.Core.Attributes;
using NetConsole.Core.Interfaces;

namespace NetConsole.Core.Tests
{
    [NotRegistrable]
    public class NotRegistrableCommand : ICommand
    {
        public int Status
        {
            get { return 0; }
        }

        public string Name
        {
            get { return ""; }
        }

        public string Overview
        {
            get { return ""; }
        }
    }
}
