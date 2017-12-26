using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlServer
{
    /// <summary>
    /// 表示数据库全局上下文
    /// </summary>
    public class Context : IDisposable
    {
        /// <summary>
        /// 当前线程运行的数据库上下文
        /// </summary>
        [ThreadStatic]
        private static Context current;

        /// <summary>
        /// 同步锁
        /// </summary>
        private static readonly object syncRoot = new object();

        /// <summary>
        /// 获取或设置连接字符串名称
        /// </summary>
        public static string DbName = typeof(SqlDb).Name;

        /// <summary>
        /// 获取当前线程运行的数据库上下文
        /// </summary>
        public static Context Current
        {
            get
            {
                lock (syncRoot)
                {
                    if (current == null)
                    {
                        current = new Context();
                    }
                    return current;
                }
            }
        }

        /// <summary>
        /// 延时初始化的上下文
        /// </summary>
        private Lazy<SqlDb> lazyContext;


        /// <summary>
        /// 获取数据库上下文
        /// </summary>
        internal SqlDb DbContext
        {
            get
            {
                return this.lazyContext.Value;
            }
        }

        /// <summary>
        /// 获取上下文对象是否已创建
        /// </summary>
        public bool IsCreated
        {
            get
            {
                return this.lazyContext.IsValueCreated;
            }
        }

        /// <summary>
        /// 数据库全局上下文
        /// </summary>     
        private Context()
        {
            this.lazyContext = new Lazy<SqlDb>(() => new SqlDb());
        }

        /// <summary>
        /// 保存变更
        /// </summary>
        /// <returns></returns>
        public int SaveChanges()
        {
            return this.DbContext.SaveChanges();
        }

        /// <summary>
        /// 开始一个事务
        /// </summary>
        /// <returns></returns>
        public DbContextTransaction BeginTransaction()
        {
            return this.DbContext.Database.BeginTransaction();
        }

        /// <summary>
        /// 开始一个事务
        /// </summary>
        /// <param name="isolationLevel">等级</param>
        /// <returns></returns>
        public DbContextTransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            return this.DbContext.Database.BeginTransaction(isolationLevel);
        }

        /// <summary>
        /// 使用一个事务
        /// </summary>
        /// <param name="tran">事务</param>
        public void UseTransaction(DbContextTransaction tran)
        {
            this.DbContext.Database.UseTransaction(tran.UnderlyingTransaction);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (this.IsCreated == true)
            {
                this.DbContext.Dispose();
            }
            current = null;
        }
    }
}
