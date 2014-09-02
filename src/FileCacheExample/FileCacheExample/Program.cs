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

            var dataBeforeLoaded1 = cache1.Get<IDictionary<string, List<string>>>(key);
            var dataBeforeLoaded2 = cache2.Get<IDictionary<string, List<string>>>(key);

            if (dataBeforeLoaded1 != null) Console.WriteLine("dataBeforeLoaded1 was loaded count: {0}", dataBeforeLoaded1.Count);
            if (dataBeforeLoaded2 != null) Console.WriteLine("dataBeforeLoaded2 was loaded count: {0}", dataBeforeLoaded2.Count);

            IDictionary<string, List<string>> dataSaved = new Dictionary<string, List<string>>
            {
                {"a", new List<string>{"a1","a2"}},
                {"b", new List<string>{"b1","b2"}}
            };

            cache1.Set(key, dataSaved, DateTimeOffset.MaxValue);
            cache2.Set(key, dataSaved, DateTimeOffset.MaxValue);

            var dataAfter1 = cache1.Get<IDictionary<string, List<string>>>(key);
            var dataAfter2 = cache2.Get<IDictionary<string, List<string>>>(key);

            if (dataAfter1 != null) Console.WriteLine("dataAfter1 was loaded count: {0}", dataAfter1.Count);
            if (dataAfter2 != null) Console.WriteLine("dataAfter2 was loaded count: {0}", dataAfter2.Count);


            Console.WriteLine("press a key...");
            Console.ReadKey();
        }
    }
}
