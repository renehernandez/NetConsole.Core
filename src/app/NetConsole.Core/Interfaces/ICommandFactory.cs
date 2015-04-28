using System.Collections.Generic;

namespace NetConsole.Core.Interfaces
{
    public interface ICommandFactory
    {

        void Register<T>(T command) where T : ICommand;

        ICommand Unregister(string cmdName);

        void RegisterAll(bool includeNotRegistrable = false);

        ICommand GetInstance(string cmdName);

        IEnumerable<ICommand> GetAll();

        bool Contains(string cmdName);

    }
}