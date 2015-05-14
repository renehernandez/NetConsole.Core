using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetConsole.Core.Commands
{
    public class Option
    {

        public string Name { get; set; }

        public bool DeclarableOnly { get; set; }

        public bool Permanent { get; set; }

        public bool OverrideExecution { get; set; }
    }
}
