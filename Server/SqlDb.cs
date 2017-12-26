using Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SqlServer
{
    /// <summary>
    /// Sql数据库
    /// </summary>
    public class SqlDb : DbContext
    {
        /// <summary>
        /// 所有广告视图
        /// </summary>
        public DbSet<TDemoTable> TDemoTable { get; set; }

        /// <summary>
        /// Sql数据库
        /// </summary>
        public SqlDb()
        {
        }


        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="model">模型</param>
        /// <param name="id">id</param>
        /// <param name="newModel">更新后的模型</param>
        /// <returns></returns>
        public T Update<T>(T model) where T : class, IStringId
        {
            try
            {
                var entry = this.Entry(model);
                if (entry.State == EntityState.Detached)
                {
                    model = this.Set<T>().Attach(model);
                    entry.State = EntityState.Modified;
                }
                return model;
            }
            catch (InvalidOperationException)
            {
                var local = this.Set<T>().Find(model.Id);
                var entry = this.Entry(local);
                entry.CurrentValues.SetValues(model);
                return local;
            }
        }

        /// <summary>
        /// 获取服务
        /// </summary>
        /// <typeparam name="TServer">服务类型</typeparam>
        /// <returns></returns>
        public TServer Server<TServer>() where TServer : ISqlServer, new()
        {
            var server = Activator.CreateInstance<TServer>();
            server.SetContext(this);
            return server;
        }

        /// <summary>
        /// 创建模型时
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }
    }
}
