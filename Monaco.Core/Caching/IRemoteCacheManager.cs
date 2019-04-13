using System;
using System.Threading.Tasks;

namespace Monaco.Core.Caching
{
    /// <summary>
    /// Represents a manager for caching between HTTP requests
    /// </summary>
    public interface IRemoteCacheManager : IDisposable
    {
        Task<T> GetAsync<T>(string key, Func<Task<T>> acquire, int? cacheTime = null);
        Task<bool> SetAsync<T>(string key, object data, int cacheTime);
        Task<bool> IsSetAsync<T>(string key);
        Task<bool> RemoveAsync<T>(string key);
    }
}
