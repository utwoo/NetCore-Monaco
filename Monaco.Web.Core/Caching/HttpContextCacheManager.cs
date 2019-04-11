using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using Microsoft.AspNetCore.Http;
using Monaco.Core.Caching;
using Monaco.Core.ComponentModel;
using Monaco.Core.Configurations;

namespace Monaco.Web.Core.Caching
{
    public class HttpContextCacheManager : ICacheManager
    {
        private readonly ReaderWriterLockSlim _locker;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpContextCacheManager(
            IHttpContextAccessor httpContextAccessor
            )
        {
            this._locker = new ReaderWriterLockSlim();
            this._httpContextAccessor = httpContextAccessor;
        }

        protected virtual IDictionary<object, object> GetCacheItems()
        {
            return this._httpContextAccessor.HttpContext?.Items;
        }

        public T Get<T>(string key, Func<T> acquire, int? cacheTime = null)
        {
            IDictionary<object, object> cacheItems;

            using (new ReaderWriteLockDisposable(_locker, ReaderWriterLockType.Read))
            {
                cacheItems = GetCacheItems();
                if (cacheItems == null)
                    return acquire();

                // item already is in cache, so return it
                if (cacheItems[key] != null)
                    return (T)cacheItems[key];
            }

            //or create it using passed function
            var result = acquire();

            if (result == null || (cacheTime ?? Convert.ToInt32(MonacoConfiguration.Instance.CachingConfig.DefaultCacheTime)) <= 0)
                return result;

            //and set in cache (if cache time is defined)
            using (new ReaderWriteLockDisposable(_locker, ReaderWriterLockType.Write))
            {
                cacheItems[key] = result;
            }

            return result;
        }

        public bool Set(string key, object data, int cacheTime)
        {
            if (data == null)
                return true;

            using (new ReaderWriteLockDisposable(_locker, ReaderWriterLockType.Write))
            {
                var cacheItems = GetCacheItems();
                if (cacheItems == null)
                    return false;

                cacheItems[key] = data;
                return true;
            }
        }

        public bool IsSet(string key)
        {
            using (new ReaderWriteLockDisposable(_locker, ReaderWriterLockType.Read))
            {
                var cacheItems = GetCacheItems();
                return cacheItems?[key] != null;
            }
        }

        public bool Remove(string key)
        {
            using (new ReaderWriteLockDisposable(_locker, ReaderWriterLockType.Write))
            {
                var cacheItems = GetCacheItems();
                return cacheItems?.Remove(key) ?? true;
            }
        }

        public void RemoveByPattern(string pattern)
        {
            using (new ReaderWriteLockDisposable(_locker, ReaderWriterLockType.UpgradeableRead))
            {
                var cacheItems = GetCacheItems();

                if (cacheItems == null)
                    return;

                var regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
                var matchesKeys = cacheItems.Keys.Select(key => key.ToString()).Where(key => regex.IsMatch(key)).ToList();

                using (new ReaderWriteLockDisposable(_locker, ReaderWriterLockType.Write))
                {
                    //remove matching values
                    foreach (var key in matchesKeys)
                    {
                        cacheItems.Remove(key);
                    }
                }
            }
        }

        public void Clear()
        {
            using (new ReaderWriteLockDisposable(_locker, ReaderWriterLockType.Write))
            {
                var cacheItems = GetCacheItems();
                cacheItems?.Clear();
            }
        }

        public void Dispose() { }
    }
}
