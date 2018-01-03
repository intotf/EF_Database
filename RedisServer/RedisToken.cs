using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisServer
{
    /// <summary>
    /// Redis提供的token
    /// </summary>
    public class RedisToken<T> : RedisBase where T : class
    {
        /// <summary>
        /// 获取或设置Token过期时间
        /// </summary>
        public TimeSpan? Expire { get; set; }

        /// <summary>
        /// Token
        /// </summary>
        /// <param name="connectionName">连接名称</param>
        /// <param name="dataBase">数据库</param>
        public RedisToken(string connectionName, Database dataBase)
            : base(connectionName, dataBase)
        {
        }

        /// <summary>
        /// 生成token号
        /// </summary>
        /// <returns></returns>
        public string GenerateToken()
        {
            return Guid.NewGuid().ToString().Replace("-", null);
        }

        /// <summary>
        /// 生成新的token    
        /// 使用ExpireMinutes的过期时间
        /// </summary>
        /// <param name="value">用户数据</param>
        /// <returns></returns>
        public async Task<string> NewTokenAsync(T value)
        {
            var token = this.GenerateToken();
            return await this.NewTokenAsync(token, value);
        }


        /// <summary>
        /// 使用ExpireMinutes的过期时间
        /// </summary>
        /// <param name="token">token值</param>
        /// <param name="value">用户数据</param>
        /// <returns></returns>
        public async Task<string> NewTokenAsync(string token, T value)
        {
            var key = this.MergeKey(token);
            if (await this.GetDatabase().StringSetAsync(key, value.ToRedisValue(), this.Expire))
            {
                return token;
            }
            return null;
        }

        /// <summary>
        /// 获取token对应的数据
        /// Token的过期时间会重置
        /// </summary>
        /// <param name="token">token</param>
        /// <returns></returns>
        public async Task<T> GetValueAsync(string token)
        {
            if (string.IsNullOrEmpty(token) == true)
            {
                return default(T);
            }

            var key = this.MergeKey(token);
            var db = this.GetDatabase();

            if (this.Expire.HasValue)
            {
                await db.KeyExpireAsync(key, this.Expire, CommandFlags.FireAndForget);
            }
            return (await db.StringGetAsync(key)).ToModel<T>();
        }

        /// <summary>
        /// 获取token对应的数据
        /// Token的过期时间不重置
        /// </summary>
        /// <param name="token">token</param>
        /// <returns></returns>
        public async Task<T> GetValueNoExpireAsync(string token)
        {
            if (string.IsNullOrEmpty(token) == true)
            {
                return default(T);
            }

            var key = this.MergeKey(token);
            var db = this.GetDatabase();

            return (await db.StringGetAsync(key)).ToModel<T>();
        }

        /// <summary>
        /// 更新token对应的数据
        /// Token的过期时间会重置
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="token">键</param>
        /// <param name="value">值</param>
        public async Task<bool> SetValueAsync(string token, T value)
        {
            if (string.IsNullOrEmpty(token) == true)
            {
                return false;
            }
            var key = this.MergeKey(token);
            return await this.GetDatabase().StringSetAsync(key, value.ToRedisValue(), this.Expire);
        }

        /// <summary>
        /// 删除token
        /// </summary>
        /// <param name="token">token</param>
        /// <returns></returns>
        public async Task<bool> RemoveTokenAsync(string token)
        {
            if (string.IsNullOrEmpty(token) == true)
            {
                return false;
            }
            var key = this.MergeKey(token);
            return await this.GetDatabase().KeyDeleteAsync(key);
        }
    }
}
