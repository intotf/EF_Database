using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisServer
{
    /// <summary>
    /// 表示发布消息服务配置
    /// </summary>
    public class PubConfig
    {
        /// <summary>
        /// 获取或设置连接域名和端口
        /// </summary>
        public string HostAndPort { get; set; }

        /// <summary>
        /// 获取或设置连接密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 转换为连接字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0},password={1},allowAdmin=true,abortConnect=false,connectTimeout=2000,syncTimeout=5000", this.HostAndPort, this.Password);
        }
    }
}
