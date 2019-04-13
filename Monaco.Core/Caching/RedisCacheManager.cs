using System;
using System.Threading.Tasks;
using Monaco.Core.Configurations;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Monaco.Core.Caching
{
    public class RedisCacheManager : IRemoteCacheManager
    {
        private readonly IDatabase _db;
        private readonly RedisConnectionManager _redisConnectionManager;

        public RedisCacheManager(
            RedisConnectionManager redisConnectionManager)
        {
            this._redisConnectionManager = redisConnectionManager;
            this._db = _redisConnectionManager.GetDatabase();
        }

        public async Task<T> GetAsync<T>(string key, Func<Task<T>> acquire, int? cacheTime = null)
        {
            var serializedItem = await _db.StringGetAsync(key);
            if (!serializedItem.HasValue)
            {
                //or create it using passed function
                var result = await acquire();

                await SetAsync<T>(key, result, cacheTime ?? MonacoConfiguration.Instance.CachingConfig.DefaultCacheTime);

                return result;
            }

            var item = JsonConvert.DeserializeObject<T>(serializedItem);
            return item;
        }

        public async Task<bool> SetAsync<T>(string key, object data, int cacheTime)
        {
            if (data == null)
                return true;

            //set cache time
            var expiresIn = TimeSpan.FromMinutes(cacheTime);
            //serialize item
            var serializedItem = JsonConvert.SerializeObject(data);
            //and set it to cache
            var result = await _db.StringSetAsync(key, serializedItem, expiresIn);

            return result;
        }

        public async Task<bool> IsSetAsync<T>(string key)
        {
            var result = await this._db.KeyExistsAsync(key);
            return result;
        }

        public async Task<bool> RemoveAsync<T>(string key)
        {
            //remove item from caches
            var result = await this._db.KeyDeleteAsync(key);
            return result;
        }

        public void Dispose()
        {
            this._redisConnectionManager?.Dispose();
        }
    }

    public class RedisConnectionManager
    {
        private readonly object _lock = new object();
        private volatile ConnectionMultiplexer _connection;

        private ConnectionMultiplexer GetConnection()
        {
            if (_connection != null && _connection.IsConnected) return _connection;

            lock (_lock)
            {
                // Double connection check
                if (_connection != null && _connection.IsConnected) return _connection;
                // Connection disconnected. Disposing connection...
                _connection?.Dispose();
                // Creating new instance of Redis Connection
                _connection = ConnectionMultiplexer.Connect(MonacoConfiguration.Instance.RedisConfig.Server);
            }

            return _connection;
        }

        public IDatabase GetDatabase(int? db = null)
        {
            return GetConnection().GetDatabase(db ?? -1);
        }

        public void Dispose()
        {
            // Dispose connection
            this._connection?.Dispose();
        }
    }
}
