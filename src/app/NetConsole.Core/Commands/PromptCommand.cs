using NetConsole.Core.Interfaces;

namespace NetConsole.Core.Commands
{

    public class PromptCommand : ICommand
    {

        # region Private Locations

        private string _prompt;

        # endregion

        # region Properties

        public string Name { get; private set; }

        public string Overview { get; private set; }

        # endregion

        public PromptCommand()
        {
            Name = "prompt";
            Overview = "Command use to change/obtain prompt character";
            _prompt = "$";
        }

        # region Public Methods

        public string Get()
        {
            return _prompt;
        }

        public void Set(string prompt)
        {
            _prompt = prompt;
        }

        # endregion
    }
}