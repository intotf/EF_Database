using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisServer
{
    /// <summary>
    /// 表示Redis队列
    /// </summary>
    public class RedisQueue<T> : RedisBase where T : class
    {
        /// <summary>
        /// 队列的Redis键
        /// </summary>
        private readonly string queueKey;

        /// <summary>
        /// 获取键队列的Redis键
        /// 已包含Prefix
        /// </summary>
        protected string Key
        {
            get
            {
                return this.MergeKey(this.queueKey);
            }
        }

        /// <summary>
        /// 队列
        /// </summary>
        /// <param name="connectionName">Redis连接名称</param>
        /// <param name="dataBase">数据库</param>
        /// <param name="key">键</param>
        public RedisQueue(string connectionName, Database dataBase, string key)
            : base(connectionName, dataBase)
        {
            this.queueKey = key;
        }

        /// <summary>
        /// 推进
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public long Push(T value)
        {
            return this.GetDatabase().ListLeftPush(this.Key, value.ToRedisValue());
        }

        /// <summary>
        /// 推进
        /// </summary>
        /// <param name="value"></param>
        public void PushFireAndForget(T value)
        {
            this.GetDatabase().ListLeftPush(this.Key, value.ToRedisValue(), StackExchange.Redis.When.Always, StackExchange.Redis.CommandFlags.FireAndForget);
        }

        /// <summary>
        /// 弹出
        /// </summary>
        /// <returns></returns>
        public T Pop()
        {
            return this.GetDatabase().ListRightPop(this.Key).ToModel<T>();
        }
    }
}
