using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using FileCacheExample.Cache;

namespace FileCacheExample
{
    class Program
    {
        static void Main(string[] args)
        {
            const string key = "key";
            const string filepath = @"c:\temp\file.json";

            IObjectCache cache1 = new FileCache(filepath);
            IObjectCache cache2 = new ObjectCacheWrapper(MemoryCache.Default);

            var dataFirstLoaded1 = cache1.Get<IDictionary<string, List<string>>>(key);
            var dataFirstLoaded2 = cache2.Get<IDictionary<string, List<string>>>(key);

            IDictionary<string, List<string>> dataSaved = new Dictionary<string, List<string>>
            {
                {"a", new List<string>{"a1","a2"}},
                {"b", new List<string>{"b1","b2"}}
            };
                        
            cache1.Set(key, dataSaved, DateTimeOffset.MaxValue);
            cache2.Set(key, dataSaved, DateTimeOffset.MaxValue);

            var dataLoaded1 = cache1.Get<IDictionary<string, List<string>>>(key);
            var dataLoaded2 = cache2.Get<IDictionary<string, List<string>>>(key);

            Console.WriteLine("press a key...");
            Console.ReadKey();
        }
    }
}
