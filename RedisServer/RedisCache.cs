using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisServer
{
    /// <summary>
    /// 表示缓存
    /// </summary>
    public class RedisCache : RedisBase
    {
        /// <summary>
        /// 缓存
        /// </summary>
        /// <param name="connectionName">Redis连接名称</param>
        /// <param name="dataBase">数据库</param>
        public RedisCache(string connectionName, Database dataBase)
            : base(connectionName, dataBase)
        {
            this.Prefix = "CACHE_";
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public string Get(string key)
        {
            key = this.MergeKey(key);
            return this.GetDatabase().StringGet(key);
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public T Get<T>(string key) where T : class
        {
            key = this.MergeKey(key);
            return this.GetDatabase().StringGet(key).ToModel<T>();
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expire">过期时间</param>
        public bool Set(string key, string value, TimeSpan? expire)
        {
            key = this.MergeKey(key);
            return this.GetDatabase().StringSet(key, value, expire);
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expire">过期时间</param>
        public bool SetIfNotExists(string key, string value, TimeSpan? expire)
        {
            key = this.MergeKey(key);
            return this.GetDatabase().StringSet(key, value, expire, StackExchange.Redis.When.NotExists);
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expire">过期时间</param>
        public bool Set<T>(string key, T value, TimeSpan? expire) where T : class
        {
            key = this.MergeKey(key);
            return this.GetDatabase().StringSet(key, value.ToRedisValue(), expire);
        }


        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expire">过期时间</param>
        public bool SetIfNotExists<T>(string key, T value, TimeSpan? expire) where T : class
        {
            key = this.MergeKey(key);
            return this.GetDatabase().StringSet(key, value.ToRedisValue(), expire, StackExchange.Redis.When.NotExists);
        }
    }
}
