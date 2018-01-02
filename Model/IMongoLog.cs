using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// MongoDb
    /// </summary>
    public interface IMongoLog
    {
        /// <summary>
        /// 日志的创建时间
        /// </summary>
        DateTime CreateTime { get; set; }
    }
}
