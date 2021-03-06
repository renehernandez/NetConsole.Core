﻿using NetConsole.Core.Attributes;
using NetConsole.Core.Commands;
using NetConsole.Core.Interfaces;

namespace NetConsole.Core.Tests
{
    public class PromptCommand : BaseCommand
    {

        # region Private Fields

        private string _prompt;

        # endregion

        # region Public Properties

        public string Domain { get; set; }

        # endregion

        # region Constructors

        public PromptCommand()
        {
            Status = 0;
            Name = "prompt";
            Overview = "Command use to change/obtain prompt character";
            _prompt = "$";
        }

        # endregion

        # region Public Methods

        [DefaultAction]
        [ActionHelp("Retrieves the current prompt text to be used in the console")]
        public string Get()
        {
            return _prompt;
        }

        [ActionHelp("Change the prompt text and returns it")]
        [ParamHelp("prompt", "Store the new value to be set as prompt text")]
        public string Set(string prompt)
        {
            _prompt = prompt;
            return _prompt;
        }

        # endregion
    }
}