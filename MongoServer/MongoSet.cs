using Infrastructure.Page;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Model;
using System.Reflection;

namespace MongoServer
{
    /// <summary>
    /// 表示Mongo的集合
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MongoSet<T> : IQueryable<T> where T : class, IMongoLog
    {
        /// <summary>
        /// 集合
        /// </summary>
        private readonly IMongoCollection<T> collection;

        /// <summary>
        /// IQueryable对象
        /// </summary>
        private readonly Lazy<IQueryable<T>> queryableLazy;

        /// <summary>
        /// Mongo的集合
        /// </summary>
        /// <param name="collection">集合</param>
        public MongoSet(IMongoCollection<T> collection)
        {
            this.collection = collection;
            this.queryableLazy = new Lazy<IQueryable<T>>(() => collection.AsQueryable());
        }

        /// <summary>
        /// 清除所有记录
        /// </summary>
        public void Clear()
        {
            this.collection.Database.DropCollection(typeof(T).Name);
        }

        /// <summary>
        /// 清除所有记录
        /// </summary>
        /// <returns></returns>
        public Task ClearAsync()
        {
            return this.collection.Database.DropCollectionAsync(typeof(T).Name);
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public bool Any(Expression<Func<T, bool>> where)
        {
            return this.collection
                .Aggregate(new AggregateOptions { AllowDiskUse = true })
                .Match(where)
                .Any();
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public Task<bool> AnyAsync(Expression<Func<T, bool>> where)
        {
            return this.collection
                .Aggregate(new AggregateOptions { AllowDiskUse = true })
                .Match(where)
                .AnyAsync();
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="model">模型</param>
        public void Add(T model)
        {
            this.collection.InsertOne(model);
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="model">模型</param>
        /// <returns></returns>
        public Task AddAsync(T model)
        {
            return this.collection.InsertOneAsync(model);
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="models">模型</param>
        public void AddRange(IEnumerable<T> models)
        {
            this.collection.InsertMany(models);
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="models">模型</param>
        /// <returns ></returns>
        public Task AddRangeAsync(IEnumerable<T> models)
        {
            return this.collection.InsertManyAsync(models);
        }

        /// <summary>
        /// 条件更新
        /// </summary>
        /// <param name="where">条件</param>
        /// <param name="updater">更新</param>
        /// <returns></returns>
        public int Update(Expression<Func<T, bool>> where, UpdateDefinition<T> updater)
        {
            return (int)this.collection.UpdateMany(where, updater).ModifiedCount;
        }

        /// <summary>
        /// 条件更新
        /// </summary>
        /// <param name="where">条件</param>
        /// <param name="updater">更新</param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(Expression<Func<T, bool>> where, UpdateDefinition<T> updater)
        {
            var result = await this.collection.UpdateManyAsync(where, updater);
            return (int)result.ModifiedCount;
        }

        /// <summary>
        /// 更新单个实体 
        /// Author : aXinNo1
        /// CreateTime: 2017-12-07
        /// </summary>
        /// <param name="where">更新条件</param>
        /// <param name="input">数据源</param>
        /// <param name="containFields">为空就是更新所有,不为空就是更新指定的字段</param>
        /// <returns></returns>
        public async Task<int> UpdateOneAsync(Expression<Func<T, bool>> where, T input, Expression<Func<T, object>> containFields)
        {
            ///修改的属性集合
            var fieldList = new List<UpdateDefinition<T>>();
            var result = 0;
            if (input == null)
                throw new NullReferenceException();
            var containFieldsMember = containFields == null ? Enumerable.Empty<MemberInfo>() : (containFields.Body as NewExpression).Members;
            var Ctype = input.GetType();

            foreach (var item in Ctype.GetProperties())
            {
                if (item.Name.ToLower() != "id")
                {
                    if (containFields == null || containFieldsMember.Any(o => o.Name == item.Name))
                    {
                        var replaceValue = item.GetValue(input);
                        if (replaceValue != null)
                        {
                            fieldList.Add(Builders<T>.Update.Set(item.Name, replaceValue));
                        }
                    }
                }
            }
            #region 有可修改的属性
            if (fieldList.Count > 0)
            {
                var builders = Builders<T>.Update.Combine(fieldList);
                ///执行提交修改
                var m = await this.collection.UpdateOneAsync(where, builders);
                result = (int)m.ModifiedCount;
            }
            return result;
            #endregion
        }


        /// <summary>
        /// 条件删除
        /// </summary>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public async Task<long> RemoveAsync(Expression<Func<T, bool>> where)
        {
            var result = await this.collection.DeleteManyAsync(where);
            return result.DeletedCount;
        }

        /// <summary>
        /// 条件删除第一条
        /// </summary>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public async Task<T> RemoveOneAsync(Expression<Func<T, bool>> where)
        {
            return await this.collection.FindOneAndDeleteAsync(where);
        }

        /// <summary>
        /// 单个查询
        /// Author : aXinNo1
        /// CreateTime: 2017-12-07
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public async Task<T> FindAsync(Expression<Func<T, bool>> where)
        {
            var result = await this.collection.FindAsync(where);
            return result.FirstOrDefault<T>();
        }


        /// <summary>
        /// 根据时间倒序排序分页
        /// </summary>
        /// <param name="source">数据源</param>
        /// <param name="where">条件</param>
        /// <param name="pageIndex">分页索引</param>
        /// <param name="pageSize">分页大小</param>
        /// <returns></returns>
        public async Task<PageInfo<T>> ToPageAsync(Expression<Func<T, bool>> where, int pageIndex, int pageSize)
        {
            try
            {
                int total = (int)await this.collection.CountAsync(where);
                var inc = total % pageSize > 0 ? 0 : -1;
                var maxPageIndex = (int)Math.Floor((double)total / pageSize) + inc;
                pageIndex = Math.Max(0, Math.Min(pageIndex, maxPageIndex));

                var data = await this.collection
                   .Aggregate(new AggregateOptions { AllowDiskUse = true })
                   .Match(where)
                   .SortByDescending(item => item.CreateTime)
                   .Skip(pageIndex * pageSize)
                   .Limit(pageSize)
                   .ToListAsync();

                return new PageInfo<T>(total, data) { PageIndex = pageIndex, PageSize = pageSize };
            }
            catch (FormatException)
            {
                this.Clear();
                return new PageInfo<T>(0, Enumerable.Empty<T>()) { PageIndex = pageIndex, PageSize = pageSize };
            }
        }

        #region 接口实现
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return this.queryableLazy.Value.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.queryableLazy.Value.GetEnumerator();
        }

        Type IQueryable.ElementType
        {
            get
            {
                return this.queryableLazy.Value.ElementType;
            }
        }

        Expression IQueryable.Expression
        {
            get
            {

                return this.queryableLazy.Value.Expression;
            }
        }

        IQueryProvider IQueryable.Provider
        {
            get
            {
                return this.queryableLazy.Value.Provider;
            }
        }
        #endregion
    }
}
