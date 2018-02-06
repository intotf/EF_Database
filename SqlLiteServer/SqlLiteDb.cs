using Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlLiteServer
{
    public class SqlLiteDb : DbContext
    {
        /// <summary>
        /// db路径
        /// </summary>
        public static readonly string DbFile = "App_Data\\data.db";

        /// <summary>
        /// 问题和答案
        /// </summary>
        public DbSet<SqlLiteModel> SqlLiteModel { get; set; }


        /// <summary>
        /// sqllite数据库上下文
        /// </summary>
        public SqlLiteDb()
            : this(DbFile)
        {
        }

        /// <summary>
        /// sqllite数据库上下文
        /// </summary>
        /// <param name="dbFile">db文件路径</param>
        /// <param name="pooling">pool连接方式</param>
        public SqlLiteDb(string dbFile, bool pooling = true)
            : base(CreateConnection(dbFile, pooling), true)
        {
        }

        /// <summary>
        /// 创建连接
        /// </summary>
        /// <param name="dbFile"></param>
        /// <param name="pooling">pool连接方式</param>
        /// <returns></returns>
        private static SQLiteConnection CreateConnection(string dbFile, bool pooling)
        {
            //var workPath = Environment.CurrentDirectory;
            if (File.Exists(dbFile) == false)
            {
                throw new FileNotFoundException(dbFile);
            }
            var constring = string.Format("Data Source={0};Pooling={1}", dbFile, pooling);
            return new SQLiteConnection(constring);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }
    }
}
