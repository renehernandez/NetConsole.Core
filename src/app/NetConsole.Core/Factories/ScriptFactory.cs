using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetConsole.Core.Interfaces;

namespace NetConsole.Core.Factories
{
    public class ScriptFactory : IFactory<ICommandScript>
    {
        public void Register(ICommandScript instance)
        {
            throw new NotImplementedException();
        }

        public ICommandScript Unregister(string name)
        {
            throw new NotImplementedException();
        }

        public void RegisterAll(bool includeNotRegistrable = false)
        {
            throw new NotImplementedException();
        }

        public ICommand GetInstance(string name)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ICommandScript> GetAll()
        {
            throw new NotImplementedException();
        }

        public bool Contains(string name)
        {
            throw new NotImplementedException();
        }
    }
}
