using Model;
using RedisServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web
{
    public class RedisCache
    {
        /// <summary>
        /// 连接管理
        /// </summary>
        private static readonly Multiplexer redis = new Multiplexer("DemoRedis");

        /// <summary>
        /// TDemoTable
        /// 60分钟有效期
        /// </summary>
        public static readonly RedisCache<TDemoTable> TDemoTable = new RedisCache<TDemoTable>(redis, TimeSpan.FromHours(2d), "TDemoTable");

        /// <summary>
        /// 验证码
        /// 30分钟有效期
        /// </summary>
        public static readonly RedisCache<string> ValidCode = new RedisCache<string>(redis, TimeSpan.FromMinutes(30d), "validCode");

        /// <summary>
        /// 基础数据
        /// 永久不过期
        /// </summary>
        public static readonly RedisCache<string> BaseData = new RedisCache<string>(redis, null, "baseData");

        /// <summary>
        /// 生成Key
        /// </summary>
        /// <returns></returns>
        public static string NewKey()
        {
            return Guid16.NewGuid().ToString();
        }
    }


    /// <summary>
    /// 表示Redis缓存
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RedisCache<T>
    {
        /// <summary>
        /// multiplexer
        /// </summary>
        private readonly Multiplexer multiplexer;

        /// <summary>
        /// 有效时间
        /// </summary>
        private readonly TimeSpan? expiry;

        /// <summary>
        /// 键前缀
        /// </summary>
        private readonly string keyPrefix;

        /// <summary>
        /// 表示会话
        /// </summary>
        /// <param name="multiplexer">Multiplexer</param>
        /// <param name="expiry">有效时间</param>
        /// <param name="keyPrefix">键的前缀</param>
        public RedisCache(Multiplexer multiplexer, TimeSpan? expiry, string keyPrefix)
        {
            this.multiplexer = multiplexer;
            this.expiry = expiry;
            this.keyPrefix = keyPrefix;
        }


        /// <summary>
        /// 获取db
        /// </summary>
        /// <returns></returns>
        private IDatabase GetDatabase()
        {
            return this.multiplexer.GetMultiplexer().GetDatabase();
        }


        /// <summary>
        /// 获取key
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        private string GetKey(string key)
        {
            if (string.IsNullOrEmpty(this.keyPrefix))
            {
                return key;
            }
            return this.keyPrefix + ":" + key;
        }

        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public bool Set(string key, T value)
        {
            key = this.GetKey(key);
            var db = this.GetDatabase();
            var redisValue = Common.Utility.JsonSerializer.Serialize(value);
            return db.StringSet(key, redisValue, this.expiry);
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public T Get(string key)
        {
            key = this.GetKey(key);
            var db = this.GetDatabase();
            var value = db.StringGet(key);

            if (value.IsNullOrEmpty == true)
            {
                return default(T);
            }

            if (this.expiry.HasValue == true)
            {
                db.KeyExpire(key, this.expiry);
            }
            return JsonSerializer.TryDeserialize<T>(value);
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public bool Exists(string key)
        {
            key = this.GetKey(key);
            var db = this.GetDatabase();
            return db.KeyExists(key);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public bool Remove(string key)
        {
            key = this.GetKey(key);
            var db = this.GetDatabase();
            return db.KeyDelete(key);
        }
    }
}