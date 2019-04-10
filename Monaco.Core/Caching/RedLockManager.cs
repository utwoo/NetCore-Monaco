using System;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using RedLockNet;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;
using StackExchange.Redis;

namespace Monaco.Core.Caching
{
    /// <summary>
    /// RedLock Lock Manager
    /// </summary>
    public class RedLockManager : ILockManager
    {
        private readonly RedLockConfiguration _configuration;

        private RedLockFactory _redisLockFactory => CreateRedisLockFactory();

        public RedLockManager(
            IOptions<RedLockConfiguration> configuration)
        {
            this._configuration = configuration.Value;
        }

        /// <summary>
        /// Acquire RedLock Lock
        /// </summary>
        /// <param name="resource">Resource identity which to be locked for</param>
        /// <param name="expirationTime">Expiration time of the lock</param>
        /// <returns>Lock instance</returns>
        public IDisposable AcquireLock(string resource, TimeSpan expirationTime)
        {
            IRedLock redLock = this._redisLockFactory.CreateLockAsync(resource, expirationTime).Result;

            // TODO: AcquireLockFailedException
            if (!redLock.IsAcquired)
                throw new Exception("Lock Acquire Failed");

            return redLock;
        }

        protected RedLockFactory CreateRedisLockFactory()
        {
            //get RedLock endpoints configurations
            var configurationOptions = ConfigurationOptions.Parse(_configuration.EndPoints);
            //get RedLock endpoints
            List<RedLockEndPoint> redLockEndPoints = new List<RedLockEndPoint>();
            foreach (var endpoint in configurationOptions.EndPoints)
            {
                redLockEndPoints.Add(new RedLockEndPoint
                {
                    EndPoint = endpoint,
                    Password = configurationOptions.Password,
                    Ssl = configurationOptions.Ssl,
                    RedisDatabase = configurationOptions.DefaultDatabase,
                    ConfigCheckSeconds = configurationOptions.ConfigCheckSeconds,
                    ConnectionTimeout = configurationOptions.ConnectTimeout,
                    SyncTimeout = configurationOptions.SyncTimeout
                });
            }
            //create RedLock factory to use RedLock distributed lock algorithm
            return RedLockFactory.Create(redLockEndPoints);
        }
    }
}
