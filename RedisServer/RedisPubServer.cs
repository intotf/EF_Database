using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisServer
{
    /// <summary>
    /// 表示消息发布服务
    /// </summary>
    public class RedisPubServer
    {
        /// <summary>
        /// 获取消息发布服务
        /// </summary>
        /// <param name="hostAndPort">端口和域名</param>
        /// <param name="password">登录密码</param>
        /// <returns></returns>
        public static RedisPubServer Parse(string hostAndPort, string password)
        {
            var config = new PubConfig { HostAndPort = hostAndPort, Password = password };
            return new RedisPubServer(RedisBase.GetMultiplexer(config), config);
        }

        /// <summary>
        /// 获取消息发布服务
        /// </summary>
        /// <param name="config">配置信息</param>
        /// <returns></returns>
        public static RedisPubServer Parse(PubConfig config)
        {
            return new RedisPubServer(RedisBase.GetMultiplexer(config), config);
        }

        /// <summary>
        /// 获取消息发布服务
        /// </summary>
        /// <param name="configs">配置信息</param>
        /// <returns></returns>
        public static RedisPubServer[] Parse(params PubConfig[] configs)
        {
            return configs.Select(item => Parse(item)).ToArray();
        }


        /// <summary>
        /// 连接上下文
        /// </summary>
        private readonly ConnectionMultiplexer multiplexer;

        /// <summary>
        /// 服务
        /// </summary>
        private readonly IServer server;

        /// <summary>
        /// 服务信息
        /// </summary>
        private readonly IDictionary<string, string> info;

        /// <summary>
        /// 获取服务配置
        /// </summary>
        public PubConfig Config { get; private set; }

        /// <summary>
        /// 是否连接
        /// </summary>
        public bool IsConnected
        {
            get
            {
                return this.multiplexer.IsConnected && this.server.IsConnected;
            }
        }

        /// <summary>
        /// 连接数
        /// </summary>
        public int ConnectedCount
        {
            get
            {
                var key = "connected_clients";
                if (this.info != null && this.info.ContainsKey(key))
                {
                    return int.Parse(this.info[key]);
                }
                return 0;
            }
        }

        /// <summary>
        /// 通道数
        /// </summary>
        public int ChannelsCount
        {
            get
            {
                var key = "pubsub_channels";
                if (this.info != null && this.info.ContainsKey(key))
                {
                    return int.Parse(this.info[key]);
                }
                return 0;
            }
        }


        /// <summary>
        /// 消息发布服务
        /// </summary>
        /// <param name="multiplexer">连接上下文</param>
        /// <param name="config">配置</param>
        private RedisPubServer(ConnectionMultiplexer multiplexer, PubConfig config)
        {
            this.multiplexer = multiplexer;
            this.server = multiplexer.GetServer(config.HostAndPort);
            if (this.IsConnected)
            {
                this.info = this.server
                    .Info()
                    .SelectMany(item => item)
                    .ToDictionary((kv) => kv.Key, (kv) => kv.Value, StringComparer.OrdinalIgnoreCase);
            }
            this.Config = config;
        }

        /// <summary>
        /// 发布消息
        /// 返回订阅端的数量
        /// </summary>
        /// <param name="channel">通道</param>
        /// <param name="message">消息</param>
        /// <returns></returns>
        public int Publish(string channel, string message)
        {
            return (int)this.multiplexer.GetSubscriber().Publish(channel, message);
        }

        /// <summary>
        /// 字符串显示
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Config.ToString();
        }
    }
}
