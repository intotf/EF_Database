using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// 增量数据操作类型 
    /// </summary>
    public enum DataAction
    {
        /// <summary>
        /// 新增 0
        /// </summary>
        Add = 0,
        /// <summary>
        /// 更新 1
        /// </summary>
        Update = 1,
        /// <summary>
        /// 删除 2
        /// </summary>
        Delete = 2
    }
}
