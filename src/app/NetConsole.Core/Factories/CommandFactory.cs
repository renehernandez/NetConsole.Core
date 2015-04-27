using System;
using System.Collections.Generic;
using System.Linq;
using NetConsole.Core.Exceptions;
using NetConsole.Core.Extensions;
using NetConsole.Core.Interfaces;

namespace NetConsole.Core.Factories
{
    public class CommandFactory : ICommandFactory
    {

        private static Dictionary<string, ICommand> _cache;

        public CommandFactory()
        {
            _cache = new Dictionary<string, ICommand>();
        }

        public void Register<T>(T instance) where T : ICommand
        {
            if(Contains(instance.Name))
                throw new DuplicatedCommandNameException(instance.Name);

            _cache.Add(instance.Name, instance);
        }

        public ICommand Unregister(string cmdName)
        {
            if(!Contains(cmdName))
                throw new FailedUnregisterOperationException(cmdName);

            var cmd = _cache[cmdName];
            _cache.Remove(cmdName);

            return cmd;
        }

        public void RegisterAll()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var type in assemblies.SelectMany(asm => asm.GetLoadableTypes()).
                GetTypesWithInterface<ICommand>())
            {
                Register((ICommand)Activator.CreateInstance(type));
            }
        }

        public bool Contains(string cmdName)
        {
            return _cache.ContainsKey(cmdName);
        }

        public ICommand GetInstance(string cmdName)
        {
            if(!Contains(cmdName))
                throw new UnregisteredCommandException(cmdName);

            return _cache[cmdName];
        }

        public IEnumerable<ICommand> GetAll()
        {
            return _cache.Values;
        } 

    }
}