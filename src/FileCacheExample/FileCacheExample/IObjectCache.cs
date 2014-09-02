using System;
using System.Runtime.Caching;

namespace FileCacheExample
{
    /// <summary>
    /// Partial needed methods taken from abstract object cache class
    /// </summary>
    public interface IObjectCache
    {
        string Name { get; }
        T Get<T>(string key, string regionName = null) where T : class;
        void Set(CacheItem item, CacheItemPolicy policy);
        void Set(string key, object value, CacheItemPolicy policy, string regionName = null);
        void Set(string key, object value, DateTimeOffset absoluteExpiration, string regionName = null);
        bool Add(string key, object value, CacheItemPolicy policy, string regionName = null);
        bool Add(string key, object value, DateTimeOffset absoluteExpiration, string regionName = null);
        T Remove<T>(string key, string regionName = null) where T : class;
    }
}