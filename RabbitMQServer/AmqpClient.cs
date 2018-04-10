using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQServer
{
    /// <summary>
    /// 表示amqp客户端
    /// </summary>
    public class AmqpClient
    {
        private readonly object sync = new object();
        private readonly ConnectionFactory factory;

        private readonly IConnection pubConnection;
        private readonly IConnection subConnection;

        private readonly IModel pubChannel;
        private readonly IModel subChannel;

        /// <summary>
        /// amqp客户端
        /// </summary>
        /// <param name="userName">账号</param>
        /// <param name="password">密码</param>
        /// <param name="host">服务器地址</param>
        /// <param name="port">端口</param>
        public AmqpClient(string userName = "guest", string password = "guest", string host = "localhost", int port = 5672)
        {
            this.factory = new ConnectionFactory
            {
                UserName = userName,
                Password = password,
                HostName = host,
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
        /// <param name="queueName">队列名称</param>
        /// <param name="message">消息内容</param>
        public void Publish(string queueName, byte[] message)
        {
            lock (this.sync)
            {
                var channel = this.pubChannel;
                channel.QueueDeclare(queueName, true, false, false, null);

                var props = channel.CreateBasicProperties();
                props.ContentType = "text/plain";
                props.DeliveryMode = 2;

                channel.BasicPublish(string.Empty, queueName, props, message);
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
