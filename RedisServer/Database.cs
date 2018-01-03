using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisServer
{
    /// <summary>
    /// 数据库枚举
    /// </summary>
    public enum Database
    {
        /// <summary>
        /// Token、验证码缓存 0
        /// </summary>
        Token = 0,

        /// <summary>
        /// 数据队例 1
        /// </summary>
        DataSyc = 1,

        /// <summary>
        /// 订阅终端对应的发布服务
        /// </summary>
        SubClients = 2,
    }
}
