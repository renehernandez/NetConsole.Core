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

        private static readonly Dictionary<string, Func<ICommand>> Commands; 

        static CommandFactory()
        {
            var commandTypes = TypeExtensions.GetTypesWithInterface<ICommand>();

            Commands = new Dictionary<string, Func<ICommand>>();

            foreach (var info in commandTypes.Where(cmd => !cmd.IsAbstract && !cmd.GetCustomAttributes(true).OfType<NotRegistrableAttribute>().Any()).
                Select(type => type.GetConstructor(Type.EmptyTypes)))
            {
                var cmd = info.Invoke(new object[0]) as ICommand;
                Commands.Add(cmd.Name, () => cmd);
            }

        }

        public IEnumerable<ICommand> GetInstances()
        {
            return Commands.Values.Select(f => f());
        }

        public Func<ICommand> GetGenerator(string name)
        {
            return Commands[name];
        }


        public IEnumerable<string> GetNames()
        {
            return Commands.Keys;
        }

    }
}
