using Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostgreSqlServer
{
    /// <summary>
    /// PostgreSql 数据库连接上下文
    /// </summary>
    public class PSqlDb : DbContext
    {
        /// <summary>
        /// 测试表
        /// </summary>
        public DbSet<NpgSqlTable> NpgSqlTable { get; set; }

    }
}
