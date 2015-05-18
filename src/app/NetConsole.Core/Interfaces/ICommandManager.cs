using NetConsole.Core.Grammar;

namespace NetConsole.Core.Interfaces
{
    public interface IManager<T> where T : class, IRegistrable
    {

        IFactory<T> Factory { get;  }

        ICache<T> Cache { get; }

        ReturnInfo[] ProcessInput(string input);

    }
}