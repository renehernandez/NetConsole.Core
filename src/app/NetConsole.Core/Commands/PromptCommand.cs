using NetConsole.Core.Attributes;
using NetConsole.Core.Interfaces;

namespace NetConsole.Core.Commands
{

    public class PromptCommand : ICommand
    {

        # region Private Locations

        private string _prompt;

        # endregion

        # region Properties

        public int Status { get; private set; }

        public string Name { get; private set; }

        public string Overview { get; private set; }

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
        public string Get()
        {
            return _prompt;
        }

        public string Set(string prompt)
        {
            _prompt = prompt;
            return _prompt;
        }

        # endregion
    }
}