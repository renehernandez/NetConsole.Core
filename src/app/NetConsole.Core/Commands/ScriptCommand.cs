using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NetConsole.Core.Attributes;

namespace NetConsole.Core.Commands
{
    public class ScriptCommand : BaseCommand
    {

        # region Constructors

        public ScriptCommand()
        {
            Name = "script";
            Overview = "Executes a serie of commands in a consecutive way";
            Status = 0;

            Accessor.ExtendOptions(new Option()
            {
                DeclarableOnly = false,
                Name = "name",
                OverrideExecution = false
            });
        }

        # endregion

        # region Actions

        [DefaultAction]
        [ActionHelp("Perform the execution of a command series coming from the input")]
        [OptionHelp("name", "Change the behavior to expect the name of an already defined script")]
        public void Run(string input)
        {
            if (Accessor.HasOptionValue("name"))
            {
                
            }
        }

        # endregion

    }
}
