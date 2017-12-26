using Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlServer
{
    public class SqlDbTwo : DbContext
    {
        /// <summary>
        /// 所有广告视图
        /// </summary>
        public DbSet<TDemoTable> TDemoTable { get; set; }

        /// <summary>
        /// Sql数据库
        /// </summary>
        public SqlDbTwo()
        {
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
