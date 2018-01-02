using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisServer
{
    /// <summary>
    /// RedisValue扩展
    /// </summary>
    public static class RedisValueExtend
    {
        /// <summary>
        /// 模型转换为RedisValue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public static RedisValue ToRedisValue<T>(this T model)
        {
            if (model == null)
            {
                return RedisValue.Null;
            }
            return JsonConvert.SerializeObject(model);
        }

        /// <summary>
        /// RedisValue转换为模型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T ToModel<T>(this RedisValue value)
        {
            if (value == RedisValue.Null || value == RedisValue.EmptyString)
            {
                return default(T);
            }
            return JsonConvert.DeserializeObject<T>(value);
        }
    }
}
