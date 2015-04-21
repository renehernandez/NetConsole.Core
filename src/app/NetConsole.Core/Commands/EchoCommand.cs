using NetConsole.Core.Attributes;
using NetConsole.Core.Interfaces;

namespace NetConsole.Core.Commands
{
    public class EchoCommand : ICommand
    {
        public string Name { get; private set; }

        public string Overview { get; private set; }

        public EchoCommand()
        {
            Name = "echo";
            Overview = "It returns the same input to the console";
        }

        [DefaultAction]
        public string Echoed(string input)
        {
            return input;
        }
    }
}