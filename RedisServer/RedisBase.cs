using Command;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisServer
{
    /// <summary>
    /// Rdids客户端抽象类
    /// </summary>
    public abstract class RedisBase
    {
        /// <summary>
        /// 数据库索引
        /// </summary>
        private readonly int dataBase;

        /// <summary>
        /// 连接名称
        /// </summary>
        private readonly string connectionName;

        /// <summary>
        /// 获取或设置键的前缀
        /// </summary>
        public string Prefix { get; set; }


        /// <summary>
        /// Rdids客户端抽象类
        /// </summary>
        /// <param name="connectionName">连接名称</param>
        /// <param name="dataBase">数据库索引</param>
        public RedisBase(string connectionName, Database dataBase)
        {
            this.dataBase = (int)dataBase;
            this.connectionName = connectionName;
        }

        /// <summary>
        /// 获取数据库
        /// </summary>
        /// <returns></returns>
        protected IDatabase GetDatabase()
        {
            var con = GetMultiplexer(this.connectionName);
            return con.GetDatabase(this.dataBase);
        }

        /// <summary>
        /// 获取订阅者接口
        /// </summary>
        /// <returns></returns>
        protected ISubscriber GetSubscriber()
        {
            var con = GetMultiplexer(this.connectionName);
            return con.GetSubscriber();
        }

        /// <summary>
        /// 获取连接对象
        /// </summary>
        /// <param name="pubConfig">配置</param>
        /// <returns></returns>
        public static ConnectionMultiplexer GetMultiplexer(PubConfig pubConfig)
        {
            var config = pubConfig.ToString();
            return Multiplexer.GetMultiplexer(config);
        }

        /// <summary>
        /// 获取连接对象
        /// </summary>
        /// <param name="connectionName">连接字符串名称</param>
        /// <returns></returns>
        public static ConnectionMultiplexer GetMultiplexer(string connectionName)
        {
            var config = ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
            return Multiplexer.GetMultiplexer(config);
        }

        /// <summary>
        /// 将前缀合并到Key
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        protected string MergeKey(string key)
        {
            return this.Prefix + Encryption.GetMD5(key);
        }

        /// <summary>
        /// 删除指定键
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public virtual async Task<bool> RemoveAsync(string key)
        {
            key = this.MergeKey(key);
            return await this.GetDatabase().KeyDeleteAsync(key);
        }

        /// <summary>
        /// 查看指定键是否存在
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public virtual bool Exists(string key)
        {
            key = this.MergeKey(key);
            return this.GetDatabase().KeyExists(key);
        }
    }
}
