using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetConsole.Core.Extensions;
using NetConsole.Core.Interfaces;

namespace NetConsole.Core.Factories
{
    public class ScriptFactory : IFactory<IScript>
    {

        private static readonly List<Func<IScript>> Constructors; 

        static ScriptFactory()
        {
            var commandTypes = TypeExtensions.GetTypesWithInterface<IScript>();

            Constructors = new List<Func<IScript>>();

            foreach (var info in commandTypes.Where(cmd => !cmd.IsAbstract).Select(type => type.GetConstructor(Type.EmptyTypes)))
            {
                Constructors.Add(() => info.Invoke(new object[0]) as IScript);
            }
        }


        public IEnumerable<IScript> GenerateAll()
        {
            return Constructors.Select(f => f());
        }
    }
}
