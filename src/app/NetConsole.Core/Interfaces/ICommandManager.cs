using NetConsole.Core.Grammar;

namespace NetConsole.Core.Interfaces
{
    public interface IManager<T> where T : IRegistrable
    {
        IFactory<T> Factory { get; }

        ReturnInfo[] ProcessInput(string input);

    }
}