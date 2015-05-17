using System;
using System.Collections.Generic;

namespace NetConsole.Core.Interfaces
{
    public interface IFactory<T> where T : class, IRegistrable
    {

        IEnumerable<T> GetInstances();

        Func<T> GetGenerator(string name);

        IEnumerable<string> GetNames();

    }
}