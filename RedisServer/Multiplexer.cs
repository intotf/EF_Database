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
    /// 表示Redis连接对象
    /// </summary>
    static class Multiplexer
    {
        /// <summary>
        /// 同步锁
        /// </summary>
        private static readonly object syncRoot = new object();

        /// <summary>
        /// 连接管理
        /// </summary>
        private static readonly Dictionary<string, ConnectionMultiplexer> table = new Dictionary<string, ConnectionMultiplexer>(StringComparer.OrdinalIgnoreCase);


        /// <summary>
        /// 获取连接对象
        /// </summary> 
        /// <param name="config">配置字符串</param>
        /// <returns></returns>
        public static ConnectionMultiplexer GetMultiplexer(string config)
        {
            lock (syncRoot)
            {
                var muliplexer = default(ConnectionMultiplexer);
                if (table.TryGetValue(config, out muliplexer) == false)
                {
                    muliplexer = Multiplexer.CreateConnection(config);
                    table[config] = muliplexer;
                    return muliplexer;
                }

                if (muliplexer.IsConnected == true)
                {
                    return muliplexer;
                }
                else
                {
                    muliplexer.Dispose();
                }

                muliplexer = Multiplexer.CreateConnection(config);
                table[config] = muliplexer;
                return muliplexer;
            }
        }


        /// <summary>
        /// 创建一个连接
        /// </summary>
        /// <param name="config">配置内容</param>
        /// <returns></returns>
        private static ConnectionMultiplexer CreateConnection(string config)
        {
            var opt = ConfigurationOptions.Parse(config);
            return ConnectionMultiplexer.Connect(opt);
        }
    }
}
