using Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Command
{
    public static partial class DbExtension
    {
        /// <summary>
        /// 条件删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbSet"></param>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public static async Task<IEnumerable<T>> RemoveAsync<T>(this DbSet<T> dbSet, Expression<Func<T, bool>> where) where T : class, IStringId
        {
            var items = await dbSet.Where(where).ToArrayAsync();
            return dbSet.RemoveRange(items);
        }

        /// <summary>
        /// 根据自增型 ID 删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbSet">数据库上下文对象</param>
        /// <param name="id">自增ID</param>
        /// <returns></returns>
        public static async Task<T> RemoveById<T>(this DbSet<T> dbSet, int id) where T : class,IAutoId
        {
            var model = await dbSet.FindAsync(id);
            return dbSet.Remove(model);
        }
    }
}
