using System.Collections.Generic;

namespace NetConsole.Core.Interfaces
{
    public interface ICache<T> where T : class
    {

        void Register(T instance);

        T Unregister(string name);

        void RegisterAll(IFactory<T> factory);

        T GetInstance(string name);

        IEnumerable<T> GetAll();

        bool Contains(string name);

        void Clear();

    }
}