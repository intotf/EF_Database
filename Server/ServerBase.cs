using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Infrastructure.Page;
using System.Threading.Tasks;

namespace SqlServer
{
    /// <summary>
    /// 定义Sql存储服务
    /// </summary>
    public interface ISqlServer
    {
        /// <summary>
        /// 设置上下文实例
        /// </summary>
        /// <param name="db"></param>
        void SetContext(SqlDb db);
    }

    /// <summary>
    /// Sql存储服务基础类
    /// </summary>
    public abstract class SqlServerBase : ISqlServer
    {
        /// <summary>
        /// 获取数据库上下文
        /// </summary>
        protected SqlDb Db { get; private set; }

        /// <summary>
        /// 设置上下文实例
        /// </summary>
        /// <param name="db"></param>
        void ISqlServer.SetContext(SqlDb db)
        {
            this.Db = db;
        }
    }
}
