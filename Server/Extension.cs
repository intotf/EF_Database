using Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SqlServer
{
    /// <summary>
    /// 扩展
    /// </summary>
    public static partial class Extension
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

        public static async Task<T> RemoveById<T>(this DbSet<T> dbSet, int id) where T : class,IAutoId
        {
            var model = await dbSet.FindAsync(id);
            return dbSet.Remove(model);
        }
    }
}
