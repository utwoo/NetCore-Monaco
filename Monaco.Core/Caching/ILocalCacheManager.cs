using System;

namespace Monaco.Core.Caching
{
    /// <summary>
    /// Cache manager interface
    /// </summary>
    public interface ILocalCacheManager : IDisposable
    {
        T Get<T>(string key, Func<T> acquire, int? cacheTime = null);
        bool Set(string key, object data, int cacheTime);
        bool IsSet(string key);
        bool Remove(string key);
        void RemoveByPattern(string pattern);
        void Clear();
    }
}
