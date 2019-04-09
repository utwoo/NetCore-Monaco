using System;
using System.Threading.Tasks;

namespace Monaco.Core.Caching
{
    /// <summary>
    /// Represents a manager for caching between HTTP requests
    /// </summary>
    public interface IStaticCacheManager : ICacheManager
    {
        Task<T> GetAsync<T>(string key, Func<Task<T>> acquire, int? cacheTime = null);
    }
}
