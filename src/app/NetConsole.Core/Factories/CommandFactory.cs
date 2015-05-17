using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using NetConsole.Core.Attributes;
using NetConsole.Core.Extensions;
using NetConsole.Core.Interfaces;

namespace NetConsole.Core.Factories
{
    public class CommandFactory : IFactory<ICommand>
    {

        private static readonly List<Func<ICommand>> Constructors; 

        static CommandFactory()
        {
            var commandTypes = TypeExtensions.GetTypesWithInterface<ICommand>();

            Constructors = new List<Func<ICommand>>();

            foreach (var info in commandTypes.Where(cmd => !cmd.IsAbstract && !cmd.GetCustomAttributes(true).OfType<NotRegistrableAttribute>().Any()).
                Select(type => type.GetConstructor(Type.EmptyTypes)))
            {
                Constructors.Add(() => info.Invoke(new object[0]) as ICommand);
            }

        }

        public IEnumerable<ICommand> GenerateAll()
        {
            return Constructors.Select(f => f());
        } 

    }
}
