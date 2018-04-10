using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQServer
{
    /// <summary>
    /// 表示amqp消息
    /// </summary>
    public class MqMessage
    {
        /// <summary>
        /// 标记
        /// </summary>
        public ulong Tag { get; private set; }

        /// <summary>
        /// 内容
        /// </summary>
        public byte[] Body { get; private set; }

        /// <summary>
        /// amqp消息
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="body"></param>
        internal MqMessage(ulong tag, byte[] body)
        {
            this.Tag = tag;
            this.Body = body;
        }

        /// <summary>
        /// 转换为字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (this.Body == null)
            {
                return null;
            }
            return Encoding.UTF8.GetString(this.Body);
        }
    }
}
