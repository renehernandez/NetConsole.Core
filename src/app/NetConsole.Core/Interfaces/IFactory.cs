using System.Collections.Generic;

namespace NetConsole.Core.Interfaces
{
    public interface IFactory<T>
    {

        void Register(T instance);

        T Unregister(string name);

        void RegisterAll(bool includeNotRegistrable = false);

        ICommand GetInstance(string name);

        IEnumerable<T> GetAll();

        bool Contains(string name);

    }
}