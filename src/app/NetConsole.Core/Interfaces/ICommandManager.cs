using NetConsole.Core.Grammar;

namespace NetConsole.Core.Interfaces
{
    public interface ICommandManager
    {
        ICommandFactory Factory { get; }

        ReturnInfo[] GetOutputFromString(string input);

        ReturnInfo[] GetOutputFromFile(string filePath);
    }
}