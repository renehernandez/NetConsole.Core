using System.Collections.Generic;

namespace NetConsole.Core.Interfaces
{
    public interface IFactory<T> where T : class
    {

        IEnumerable<T> GenerateAll();

    }
}