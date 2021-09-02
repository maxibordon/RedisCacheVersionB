using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RedisCacheVersionB
{
    public class RedisHelper : IDisposable, IDistributedCache
    {
        private string _connectionString; // cadena de conexión
        private string _instanceName; // nombre de instancia
        private int _defaultDB; // base de datos predeterminada
        private ConcurrentDictionary<string, ConnectionMultiplexer> _connections;
        public RedisHelper(string connectionString, string instanceName, int defaultDB = 0)
        {
            _connectionString = connectionString;
            _instanceName = instanceName;
            _defaultDB = defaultDB;
            _connections = new ConcurrentDictionary<string, ConnectionMultiplexer>();
        }

        /// <summary>
        /// obtener ConnectionMultiplexer
        /// </summary>
        /// <returns></returns>
        private ConnectionMultiplexer GetConnect()
        {
            return _connections.GetOrAdd(_instanceName, p => ConnectionMultiplexer.Connect(_connectionString));
        }

        /// <summary>
        /// obtener la base de datos
        /// </summary>
        /// <param name="configName"></param>
        /// <param name = "db"> El valor predeterminado es 0: la configuración db del código de prioridad, seguida de la configuración en config </ param>
        /// <returns></returns>
        public IDatabase GetDatabase()
        {
            return GetConnect().GetDatabase();
        }

        public IServer GetServer(string configName = null, int endPointsIndex = 0)
        {
            var confOption = ConfigurationOptions.Parse(_connectionString);
            return GetConnect().GetServer(confOption.EndPoints[endPointsIndex]);
        }

        public ISubscriber GetSubscriber(string configName = null)
        {
            return GetConnect().GetSubscriber();
        }

        public void Dispose()
        {
            if (_connections != null && _connections.Count > 0)
            {
                foreach (var item in _connections.Values)
                {
                    item.Close();
                }
            }
        }

        byte[] IDistributedCache.Get(string key)
        {
            throw new NotImplementedException();
        }

        Task<byte[]> IDistributedCache.GetAsync(string key, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        void IDistributedCache.Refresh(string key)
        {
            throw new NotImplementedException();
        }

        Task IDistributedCache.RefreshAsync(string key, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        void IDistributedCache.Remove(string key)
        {
            throw new NotImplementedException();
        }

        Task IDistributedCache.RemoveAsync(string key, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        void IDistributedCache.Set(string key, byte[] value, DistributedCacheEntryOptions options)
        {
            throw new NotImplementedException();
        }

        Task IDistributedCache.SetAsync(string key, byte[] value, DistributedCacheEntryOptions options, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}