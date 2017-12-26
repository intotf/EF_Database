using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;

namespace SqlServer.Server
{
    /// <summary>
    /// 执行Sql 语句
    /// </summary>
    public class T_DemoTableServer : SqlServerBase
    {
        /// <summary>
        /// 指定Sql 语句
        /// </summary>
        /// <param name="id">广告主ID</param>
        public async Task RemoveBySql(int id)
        {
            var sql = string.Format(@"Delete T_DemoTable Where F_Id = {0}", id);

            await this.Db.Database.ExecuteSqlCommandAsync(sql);
        }

        /// <summary>
        /// 通过string 字段模糊查询
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public async Task<TDemoTable[]> GetModelsByLinq(string str)
        {
            //var q = from t in Db.T_DemoTable.Where(item => item.F_String.Contains(str))
            //        select t;

            //return await q.ToArrayAsync();
            return await this.Db.TDemoTable.Where(item => item.F_String.Contains(str)).ToArrayAsync();
        }
    }
}
