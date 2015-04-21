using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetConsole.Core
{
    public class CommandListener : CommandGrammarBaseListener
    {
        public override void EnterCommand(CommandGrammarParser.CommandContext context)
        {
            var cmdID = context.GetChild(0);
            
        }
    }
}
