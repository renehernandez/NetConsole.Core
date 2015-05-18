using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetConsole.Core.Attributes;
using NetConsole.Core.Extensions;
using NetConsole.Core.Interfaces;

namespace NetConsole.Core.Factories
{
    public class ScriptFactory : IFactory<IScript>
    {
        private static readonly Dictionary<string, Func<IScript>> Scripts;

        static ScriptFactory()
        {
            var commandTypes = TypeExtensions.GetTypesWithInterface<IScript>();

            Scripts = new Dictionary<string, Func<IScript>>();

            foreach (var info in commandTypes.Where(cmd => !cmd.IsAbstract && !cmd.GetCustomAttributes(true).OfType<NotRegistrableAttribute>().Any()).
                Select(type => type.GetConstructor(Type.EmptyTypes)))
            {
                var script = info.Invoke(new object[0]) as IScript;
                Scripts.Add(script.Name, () => info.Invoke(new object[0]) as IScript);
            }

        }

        public IEnumerable<IScript> GetInstances()
        {
            return Scripts.Values.Select(f => f());
        }

        public Func<IScript> GetGenerator(string name)
        {
            return Scripts[name];
        } 

        public IEnumerable<string> GetNames()
        {
            return Scripts.Keys;
        }
    }
}
