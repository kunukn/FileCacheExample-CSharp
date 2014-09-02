using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Caching;
using System.Text;

namespace FileCacheExample.Cache
{
    /// <summary>
    /// Author: Kunuk Nykjaer
    /// 
    /// Folder must exists with given file path
    ///     
    /// File cache which doesn't invalidate, you must manually delete the file if you to invalidate the cache
    /// This is best used when get operations are frequently used and set operations are rarely used.
    /// This is due to set operations are slow because every set operation must re-create the file on disk.
    /// 
    /// RegionName is not supported
    /// </summary>
    public class FileCache : IObjectCache
    {
        private readonly object threadsafe = new object();

        private readonly string filePath;

        private IDictionary<string, object> lookup = new Dictionary<string, object>();

        public FileCache(string filePath)
        {
            // Guard clause
            if (string.IsNullOrWhiteSpace(filePath)) throw new ArgumentException("filepath");

            // Update
            this.filePath = filePath;

            // Load cache from disk
            Load();
        }

        public Exception LastException { get; private set; }

        public string Name
        {
            get { return "FileCache"; }
        }

        internal void Load()
        {
            lock (threadsafe)
            {
                try
                {
                    string json;
                    if (!File.Exists(filePath))
                    {
                        json = Newtonsoft.Json.JsonConvert.SerializeObject(lookup);
                        File.WriteAllText(filePath, json, Encoding.UTF8);
                    }

                    json = File.ReadAllText(filePath, Encoding.UTF8);
                    lookup = Newtonsoft.Json.JsonConvert.DeserializeObject<IDictionary<string, object>>(json);
                }
                catch (Exception ex)
                {
                    LastException = ex;
                    throw;
                }
            }
        }

        internal void Save()
        {
            lock (threadsafe)
            {
                try
                {
                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(lookup);
                    File.WriteAllText(filePath, json, Encoding.UTF8);
                }
                catch (Exception ex)
                {
                    LastException = ex;
                    throw;
                }
            }
        }

        public object AddOrGetExisting(string key, object value, DateTimeOffset absoluteExpiration, string regionName = null)
        {
            if (lookup.ContainsKey(key)) return lookup[key];

            lookup.Add(key, value);
            Save();
            return value;
        }

        public object AddOrGetExisting(string key, object value, CacheItemPolicy policy, string regionName = null)
        {
            if (lookup.ContainsKey(key)) return lookup[key];

            lookup.Add(key, value);
            Save();
            return value;
        }

        public T Get<T>(string key, string regionName = null) where T : class
        {
            var obj = lookup.ContainsKey(key) ? lookup[key] : null;
            return Cast<T>(obj);
        }

        internal T Cast<T>(object obj) where T : class
        {
            if (obj == null) return null;

            Type type = obj.GetType();

            if (type == typeof(JObject)) return ((JObject)obj).ToObject<T>();
            if (type == typeof(JArray)) return ((JArray)obj).ToObject<T>();
            if (type == typeof (T)) return (T) obj;
            return obj as T;
        }

        public void Set(string key, object value, DateTimeOffset absoluteExpiration, string regionName = null)
        {
            lookup[key] = value;
            Save();
        }

        public bool Add(string key, object value, CacheItemPolicy policy, string regionName = null)
        {
            return (AddOrGetExisting(key, value, policy, regionName) == null);
        }

        public bool Add(string key, object value, DateTimeOffset absoluteExpiration, string regionName = null)
        {
            return (AddOrGetExisting(key, value, absoluteExpiration, regionName) == null);
        }

        public void Set(CacheItem item, CacheItemPolicy policy)
        {
            lookup[item.Key] = item.Value;
            Save();
        }

        public void Set(string key, object value, CacheItemPolicy policy, string regionName = null)
        {
            lookup[key] = value;
            Save();
        }

        public T Remove<T>(string key, string regionName = null) where T : class
        {
            object obj;
            if (lookup.TryGetValue(key, out obj))
            {
                lookup.Remove(key);
            }

            Save();

            return Cast<T>(obj);
        }
    }
}
