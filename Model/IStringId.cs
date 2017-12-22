using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// 实体接口
    /// </summary>
    public interface IStringId
    {
        /// <summary>
        /// 获取或设置唯一标识
        /// </summary>
        string Id { get; set; }
    }
}
