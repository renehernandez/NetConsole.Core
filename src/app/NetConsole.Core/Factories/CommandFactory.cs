using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NetConsole.Core.Attributes;
using NetConsole.Core.Exceptions;
using NetConsole.Core.Extensions;
using NetConsole.Core.Interfaces;

namespace NetConsole.Core.Factories
{
    public class CommandFactory : BaseFactory<ICommand>
    {


        public CommandFactory()
        {
        }

        //public ICommand Unregister(string name)
        //{
        //    if(!Contains(name))
        //        throw new FailedUnregisterOperationException(name);

        //    var cmd = _cache[name];
        //    _cache.Remove(name);

        //    return cmd;
        //}

        //public void RegisterAll(bool includeNotRegistrable = false)
        //{
        //    var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        //    var commandTypes = assemblies.SelectMany(asm => asm.GetLoadableTypes()).GetTypesWithInterface<ICommand>();

        //    if (!includeNotRegistrable)
        //        commandTypes = commandTypes.Where(cmd => !cmd.GetCustomAttributes(true).OfType<NotRegistrableAttribute>().Any());

        //    foreach (var type in commandTypes.Where(t => !t.IsAbstract))
        //    {
        //        Register((ICommand)Activator.CreateInstance(type));
        //    }
        //}

        //public bool Contains(string name)
        //{
        //    return _cache.ContainsKey(name);
        //}

        //public ICommand GetInstance(string name)
        //{
        //    if(!Contains(name))
        //        throw new UnregisteredInstanceException(name);

        //    return _cache[name];
        //}

        //public IEnumerable<ICommand> GetAll()
        //{
        //    return _cache.Values;
        //}

    }
}