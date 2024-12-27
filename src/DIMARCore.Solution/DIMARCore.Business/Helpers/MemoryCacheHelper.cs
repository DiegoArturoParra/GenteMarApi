using System;

namespace DIMARCore.Business.Helpers
{
    public class MemoryCacheHelper
    {
        public static void Add<T>(string key, T value, int expirationTimeDays)
        {
            System.Runtime.Caching.MemoryCache.Default.Add(key, value, DateTimeOffset.Now.AddDays(expirationTimeDays));
        }

        public static T Get<T>(string key)
        {
            return (T)System.Runtime.Caching.MemoryCache.Default.Get(key);
        }

        public static bool Contains(string key)
        {
            return System.Runtime.Caching.MemoryCache.Default.Contains(key);
        }

        public static void Remove(string key)
        {
            System.Runtime.Caching.MemoryCache.Default.Remove(key);
        }
    }
}
