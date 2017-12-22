using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Infrastructure.Page;
namespace SqlServer
{
    public static class PageInfoExtend
    {
        /// <summary>
        /// 执行分页        
        /// 性能比较好
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="source">数据源</param>    
        /// <param name="orderBy">排序字符串</param>
        /// <param name="pageIndex">分页索引</param>
        /// <param name="pageSize">分页大小</param>
        /// <returns></returns>
        public static async Task<PageInfo<T>> ToPageAsync<T>(this IQueryable<T> source, string orderBy, int pageIndex, int pageSize) where T : class
        {
            int total = await source.CountAsync();
            var inc = total % pageSize > 0 ? 0 : -1;
            var maxPageIndex = (int)Math.Floor((double)total / pageSize) + inc;
            pageIndex = Math.Max(0, Math.Min(pageIndex, maxPageIndex));

            var data = await source.OrderBy(orderBy).Skip(pageIndex * pageSize).Take(pageSize).AsNoTracking().ToListAsync();
            var page = new PageInfo<T>(total, data) { PageIndex = pageIndex, PageSize = pageSize };
            return page;
        }


        ///// <summary>
        ///// 根据时间倒序排序分页
        ///// </summary>
        ///// <typeparam name="T">实体类型</typeparam>
        ///// <param name="source">数据源</param>
        ///// <param name="where">条件</param>
        ///// <param name="pageIndex">分页索引</param>
        ///// <param name="pageSize">分页大小</param>
        ///// <returns></returns>
        //public static PageInfo<T> ToPage<T>(this IMongoCollection<T> source, Expression<Func<T, bool>> where, int pageIndex, int pageSize) where T : class,IMongoLog
        //{
        //    int total = (int)source.Count(where);

        //    var data = source
        //       .Aggregate(new AggregateOptions { AllowDiskUse = true })
        //       .Match(where)
        //       .SortByDescending(item => item.CreateTime)
        //       .Skip(pageIndex * pageSize)
        //       .Limit(pageSize)
        //       .ToList();

        //    return new PageInfo<T>(total, data) { PageIndex = pageIndex, PageSize = pageSize };
        //}
    }
}
