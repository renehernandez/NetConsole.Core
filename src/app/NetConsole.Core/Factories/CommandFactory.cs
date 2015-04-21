using System;
using System.Collections.Generic;
using System.Linq;
using NetConsole.Core.Extensions;
using NetConsole.Core.Interfaces;

namespace NetConsole.Core.Factories
{
    public static class CommandFactory
    {

        static Dictionary<string, ICommand> _cache = new Dictionary<string, ICommand>();

        public static void Register<T>(T instance) where T : ICommand
        {
            _cache.Add(instance.Name, instance);
        }

        public static void RegisterAll()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var type in assemblies.SelectMany(asm => asm.GetLoadableTypes()).
                GetTypesWithInterface<ICommand>())
            {
                Register((ICommand)Activator.CreateInstance(type));
            }
        }

        public static ICommand Get(string textKey)
        {
            return _cache[textKey];
        }

        public static IEnumerable<ICommand> Get()
        {
            return _cache.Values;
        } 

    }
}