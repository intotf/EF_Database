using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// MongoDb 表类型 
    /// </summary>
    public enum MongoTableType
    {
        /// <summary>
        /// 同一个库
        /// </summary>
        [Display(Name = "同一个库")]
        Table = 1,

        /// <summary>
        /// 按月分库
        /// </summary>
        [Display(Name = "按月分库")]
        Month = 2,

        /// <summary>
        /// 按年分库
        /// </summary>
        [Display(Name = "按年分库")]
        Year = 3
    }
}
