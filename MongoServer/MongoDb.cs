using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using System.Configuration;
using System.Linq.Expressions;
using Infrastructure.Page;
using Model;

namespace MongoServer
{
    public class MongoDbBase
    {

        /// <summary>
        /// 连接字符串
        /// </summary>
        private static readonly string connectOption = ConfigurationManager.ConnectionStrings["MongoDbBase"].ConnectionString;

        /// <summary>
        /// 客户端
        /// </summary>
        private readonly MongoClient client = new MongoClient(connectOption);

        /// <summary>
        /// 所有内容一个表存
        /// </summary>
        /// <returns></returns>
        public MongoSet<TDemoTable> TDemoTable()
        {
            return this.Set<TDemoTable>(null, null);
        }

        /// <summary>
        /// 按月存
        /// </summary>
        /// <returns></returns>
        public MongoSet<TDemoTable> TDemoTableMonth(DateTime dt)
        {
            return this.SetMonth<TDemoTable>(dt, null);
        }

        /// <summary>
        /// 按年存
        /// </summary>
        /// <returns></returns>
        public MongoSet<TDemoTable> TDemoTableYear(DateTime dt)
        {
            return this.SetYear<TDemoTable>(dt, null);
        }

        /// <summary>
        /// 获取集合操作
        /// </summary>
        /// <typeparam name="T">集合对象类型</typeparam>
        /// <param name="dbMonth">选择月</param>
        /// <param name="setSuffix">集合名称后缀</param>
        /// <returns></returns>
        private MongoSet<T> SetMonth<T>(DateTime dbMonth, string setSuffix) where T : class,IMongoLog
        {
            var dbSuffix = dbMonth.ToString("yyMM");
            return this.Set<T>(dbSuffix, setSuffix);
        }

        /// <summary>
        /// 获取集合操作
        /// </summary>
        /// <typeparam name="T">集合对象类型</typeparam>
        /// <param name="dbMonth">选择年</param>
        /// <param name="setSuffix">集合名称后缀</param>
        /// <returns></returns>
        private MongoSet<T> SetYear<T>(DateTime dbMonth, string setSuffix) where T : class,IMongoLog
        {
            var dbSuffix = dbMonth.ToString("yy");
            return this.Set<T>(dbSuffix, setSuffix);
        }

        /// <summary>
        /// 获取集合操作
        /// </summary>
        /// <typeparam name="T">集合对象类型</typeparam>
        /// <param name="dbSuffix">db名称后缀</param>
        /// <param name="setSuffix">集合名称后缀</param>
        /// <returns></returns>
        private MongoSet<T> Set<T>(string dbSuffix, string setSuffix) where T : class,IMongoLog
        {
            var dbName = "Demo" + dbSuffix;
            var coName = typeof(T).Name + setSuffix;
            var db = client.GetDatabase(dbName);
            var set = db.GetCollection<T>(coName);

            return new MongoSet<T>(set);
        }

        /// <summary>
        /// 获取当前月往前连接的几个月
        /// </summary>
        /// <param name="m">共几个月</param>
        /// <returns></returns>
        public IEnumerable<DateTime> GetMonths(int m)
        {
            var current = DateTime.Today.AddDays(1 - DateTime.Today.Day);
            for (var i = 0; i < m; i++)
            {
                yield return current.AddMonths(-i);
            }
        }
    }
}
