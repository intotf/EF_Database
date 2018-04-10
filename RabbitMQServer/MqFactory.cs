using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQServer
{
    /// <summary>
    /// 队列工厂
    /// </summary>
    public class MqFactory
    {
        private readonly object sync = new object();

        private readonly ConnectionFactory factory;

        private readonly IConnection pubConnection;
        private readonly IConnection subConnection;

        private readonly IModel pubChannel;
        private readonly IModel subChannel;

        /// <summary>
        /// 构造队列通道
        /// </summary>
        /// <param name="userName">登录名</param>
        /// <param name="password">登录密码</param>
        /// <param name="hostName">Server 地址</param>
        /// <param name="port">端口</param>
        public MqFactory(string userName = "guest", string password = "guest", string hostName = "localhost", int port = 5672)
        {
            this.factory = new ConnectionFactory
            {
                UserName = userName,
                Password = password,
                HostName = hostName,
                Port = port,
                AutomaticRecoveryEnabled = true
            };

            this.pubConnection = this.factory.CreateConnection();
            this.subConnection = this.factory.CreateConnection();

            this.pubChannel = this.pubConnection.CreateModel();
            this.subChannel = this.subConnection.CreateModel();
        }


        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="queueName">队列名称/主题</param>
        /// <param name="body">消息内容</param>
        /// <param name="IsDurable">是否持久化</param>
        public void Publish(string queueName, byte[] body, bool IsDurable = true)
        {
            lock (this.sync)
            {
                var channel = this.pubChannel;
                channel.QueueDeclare(queueName, IsDurable, false, false, null);

                var props = channel.CreateBasicProperties();
                props.ContentType = "text/plain";
                props.DeliveryMode = (byte)(IsDurable ? 2 : 1);
                channel.BasicPublish(string.Empty, queueName, props, body);
            }
        }

        /// <summary>
        /// 提取消息
        /// </summary>
        /// <param name="queueName">队列名称</param>
        /// <param name="delete">是否同时从服务器中删除消息</param>
        /// <returns></returns>
        public MqMessage Get(string queueName, bool delete = false)
        {
            lock (this.sync)
            {
                var channel = this.subChannel;
                channel.QueueDeclare(queueName, true, false, false, null);
                var result = channel.BasicGet(queueName, delete);
                if (result == null)
                {
                    return null;
                }
                return new MqMessage(result.DeliveryTag, result.Body);
            }
        }

        /// <summary>
        /// 确认并删除消息
        /// </summary>
        /// <param name="tag">消息标记</param>
        public void Ack(ulong tag)
        {
            lock (this.sync)
            {
                var channel = this.subChannel;
                channel.BasicAck(tag, false);
            }
        }

        /// <summary>
        /// 消息归队
        /// </summary>
        /// <param name="tag">消息标记</param>
        public void Requeue(ulong tag)
        {
            lock (this.sync)
            {
                var channel = this.subChannel;
                channel.BasicNack(tag, false, true);
            }
        }
    }
}
