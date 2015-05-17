using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using NetConsole.Core.Attributes;
using NetConsole.Core.Exceptions;
using NetConsole.Core.Extensions;
using NetConsole.Core.Interfaces;

namespace NetConsole.Core.Caching
{
    public abstract class BaseCache<TCache, TItem> : ICache<TItem> 
        where TCache : BaseCache<TCache, TItem>, new()
        where TItem : class, IRegistrable
    {

        # region Private Fields

        protected MemoryCache _cache;

        # endregion

        # region Constructors

        protected BaseCache(MemoryCache cache)
        {
            _cache = cache;
        }

        # endregion

        # region Public Methods

        public static TCache GetCache()
        {
            var tCache = new TCache();
            return tCache;
        }

        public static TCache GetEmptyCache()
        {
            var tCache = GetCache();
            tCache.Clear();
            return tCache;
        }

        public virtual void Register(TItem instance)
        {
            if (instance == null)
                throw new NullInstanceException();

            if (Contains(instance.Name))
                throw new DuplicatedNameException(instance.Name);

            _cache.Add(instance.Name, instance, new CacheItemPolicy());
        }

        public virtual TItem Unregister(string name)
        {
            if (!Contains(name))
                throw new FailedUnregisterOperationException(name);

            var instance = _cache.Remove(name);

            return instance as TItem;
        }

        public virtual void RegisterAll(IFactory<TItem> factory)
        {
            foreach (var item in factory.GenerateAll())
            {
                Register(item);
            }
        }

        public virtual TItem GetInstance(string name)
        {
            if (!Contains(name))
                throw new UnregisteredInstanceException(name);

            return _cache.Get(name) as TItem;
        }

        public virtual IEnumerable<TItem> GetAll()
        {
            return _cache.Select(kv => kv.Value).Cast<TItem>();
        }

        public bool Contains(string name)
        {
            return _cache.Contains(name);
        }

        public virtual void Clear()
        {
            _cache.ToList().ForEach(kv => _cache.Remove(kv.Key));
        }

        # endregion
    }
}
