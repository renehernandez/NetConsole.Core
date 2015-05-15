using NetConsole.Core.Grammar;

namespace NetConsole.Core.Interfaces
{
    public interface ICommandManager
    {
        IFactory<ICommand> Factory { get; }

        ReturnInfo[] GetOutputFromString(string input);

        ReturnInfo[] GetOutputFromFile(string filePath);
    }
}