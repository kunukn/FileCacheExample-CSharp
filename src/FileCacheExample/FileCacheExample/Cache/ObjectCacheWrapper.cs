using System;
using System.Runtime.Caching;

namespace FileCacheExample.Cache
{   
    /// <summary>
    /// Author: Kunuk Nykjaer
    /// </summary>
    public class ObjectCacheWrapper : IObjectCache
    {
        private readonly ObjectCache cache;
        public ObjectCacheWrapper(ObjectCache cache)
        {
            this.cache = cache;
        }

        public string Name
        {
            get { return cache.Name; }
        }

        public T Get<T>(string key, string regionName = null) where T : class
        {
            return cache.Get(key, regionName) as T;
        } 

        public void Set(CacheItem item, CacheItemPolicy policy)
        {
            cache.Set(item, policy);
        }

        public void Set(string key, object value, CacheItemPolicy policy, string regionName = null)
        {
            cache.Set(key, value, policy, regionName);
        }

        public void Set(string key, object value, DateTimeOffset absoluteExpiration, string regionName = null)
        {
            cache.Set(key, value, absoluteExpiration, regionName);
        }

        public bool Add(string key, object value, CacheItemPolicy policy, string regionName = null)
        {
            return cache.Add(key, value, policy, regionName);
        }

        public bool Add(string key, object value, DateTimeOffset absoluteExpiration, string regionName = null)
        {
            return cache.Add(key, value, absoluteExpiration, regionName);
        }
        public T Remove<T>(string key, string regionName = null) where T : class
        {
            return cache.Remove(key, regionName) as T;
        }
    }
}
