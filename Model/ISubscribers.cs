using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// 订阅者信息
    /// </summary>
    public interface ISubscribers
    {
        /// <summary>
        /// 订阅者ID
        /// </summary>
        string Id { get; set; }
    }
}
