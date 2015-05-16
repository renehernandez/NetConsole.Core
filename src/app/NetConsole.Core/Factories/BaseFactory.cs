using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetConsole.Core.Attributes;
using NetConsole.Core.Exceptions;
using NetConsole.Core.Extensions;
using NetConsole.Core.Interfaces;

namespace NetConsole.Core.Factories
{
    public abstract class BaseFactory<T> : IFactory<T> where T : class, IRegistrable
    {

        private static Dictionary<string, T> _cache;

        protected BaseFactory()
        {
            _cache = new Dictionary<string, T>();
        }

        public void Register(T instance)
        {
            if (instance == null)
                throw new NullInstanceException();

            if (Contains(instance.Name))
                throw new DuplicatedNameException(instance.Name);

            _cache.Add(instance.Name, instance);
        }

        public T Unregister(string name)
        {
            if (!Contains(name))
                throw new FailedUnregisterOperationException(name);

            var cmd = _cache[name];
            _cache.Remove(name);

            return cmd;
        }

        public void RegisterAll(bool includeNotRegistrable = false)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            var factoryTypes = assemblies.SelectMany(asm => asm.GetLoadableTypes()).GetTypesWithInterface<T>();

            if (!includeNotRegistrable)
                factoryTypes = factoryTypes.Where(cmd => !cmd.GetCustomAttributes(true).OfType<NotRegistrableAttribute>().Any());

            foreach (var type in factoryTypes.Where(t => !t.IsAbstract))
            {
                Register(Activator.CreateInstance(type) as T);
            }
        }

        public T GetInstance(string name)
        {
            if (!Contains(name))
                throw new UnregisteredInstanceException(name);

            return _cache[name];
        }

        public IEnumerable<T> GetAll()
        {
            return _cache.Values;
        }

        public bool Contains(string name)
        {
            return _cache.ContainsKey(name);
        }
    }
}
