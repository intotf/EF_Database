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
    public interface ISqlServer<T> where T : DbContext
    {
        /// <summary>
        /// 设置上下文实例
        /// </summary>
        /// <param name="db"></param>
        void SetContext(T db);
    }

    /// <summary>
    /// Sql存储服务基础类
    /// </summary>
    public abstract class SqlServerBase<T> : ISqlServer<T> where T : DbContext
    {
        /// <summary>
        /// 获取数据库上下文
        /// </summary>
        protected T Db { get; private set; }

        /// <summary>
        /// 设置上下文实例
        /// </summary>
        /// <param name="db"></param>
        void ISqlServer<T>.SetContext(T db)
        {
            this.Db = db;
        }
    }
}
