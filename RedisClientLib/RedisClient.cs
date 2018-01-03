using Model;
using RedisServer;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;
using Command;

namespace RedisClientLib
{
    public class RedisClient
    {
        /// <summary>
        /// 连接名称
        /// </summary>
        private static readonly string ConnName = "DemoRedis";

        /// <summary>
        /// 一般缓存对象
        /// </summary>
        public readonly static RedisCache Cache = new RedisCache(ConnName, Database.Token);

        /// <summary>
        /// 表数据Token
        /// 过期时间为1分钟
        /// </summary>
        public readonly static RedisToken<TDemoTable> TDemoTable = new RedisToken<TDemoTable>(ConnName, Database.Token) { Expire = TimeSpan.FromMinutes(1), Prefix = "TDemoTable_" };

        /// <summary>
        /// 字符串
        /// 过期时间1天
        /// </summary>
        public readonly static RedisToken<string> DemoString = new RedisToken<string>(ConnName, Database.Token) { Expire = TimeSpan.FromDays(1), Prefix = "Demo_" };
    }

    /// <summary>
    /// 订阅发布
    /// </summary>
    public class PubSubRedis : RedisBase
    {
        /// <summary>
        /// 获取唯一实例
        /// </summary>
        public static readonly PubSubRedis Instance = new PubSubRedis("DemoRedis", Database.DataSyc);

        /// <summary>
        /// 通知Hash的key
        /// </summary>
        private static readonly string NotifyHashKey = "Notify_Hash";

        /// <summary>
        /// 同步锁对象
        /// </summary>
        private readonly object syncRoot = new object();

        /// <summary>
        /// Rdids增量数据同步
        /// </summary>
        /// <param name="connectionName">连接名称</param>
        /// <param name="dataBase">数据库索引</param>
        private PubSubRedis(string connectionName, Database dataBase)
            : base(connectionName, dataBase)
        {
            this.Prefix = "Pub_";
        }

        /// <summary>
        /// 获取单个订阅者信息
        /// </summary>
        /// <param name="userId">订阅者ID</param>
        /// <returns></returns>
        public async Task<RedisSubscriber> FindSubscribeAsync(string userIds)
        {
            var data = await this.GetSubscribeAsync(new[] { userIds });
            return data.FirstOrDefault();
        }

        /// <summary>
        /// 获取多个订阅者信息
        /// </summary>
        /// <param name="userIds">订阅者ID[]</param>
        /// <returns></returns>
        public async Task<IEnumerable<RedisSubscriber>> GetSubscribeAsync(string[] userIds)
        {
            var data = await this.HashGetAsync(userIds);
            return data.Select(item => item.ToModel<RedisSubscriber>());
        }

        /// <summary>
        /// 获取所有订阅者信息
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<RedisSubscriber>> GetAllSubscribeAsync()
        {
            var data = await this.HashGetAllAsync();
            return data.Select(item => item.Value.ToModel<RedisSubscriber>());
        }

        /// <summary>
        /// 订阅
        /// 重复订阅只保留最后一次订阅
        /// </summary>
        /// <param name="desc">描叙</param>
        /// <param name="notifyURL">通知回调的相对或绝对URL</param>
        /// <returns></returns>
        public async Task<bool> SubscribeAsync(RedisSubscriber user)
        {
            var db = this.GetDatabase();
            await db.HashDeleteAsync(NotifyHashKey, Encryption.GetMD5(user.Id));
            return await db.HashSetAsync(NotifyHashKey, Encryption.GetMD5(user.Id), user.ToRedisValue());
        }

        /// <summary>
        /// 取消订阅
        /// </summary>
        /// <param name="subSystem">用户ID</param>
        /// <returns></returns>
        public async Task<bool> UnSubscribe(string userId)
        {
            var db = this.GetDatabase();
            return await db.HashDeleteAsync(NotifyHashKey, Encryption.GetMD5(userId));
        }

        /// <summary>
        /// 获取用户的增量数据
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="count">获取条数(0或不填为全部)</param>
        /// <returns></returns>
        public async Task<RedisSyncData[]> GetChangeDatasAsync(string userId, int count = 10)
        {
            var key = this.MergeKey(userId);
            var hash = await this.GetDatabase().HashValuesAsync(key);
            return hash
                .Select(item => item.ToModel<RedisSyncData>())
                .OrderBy(item => item.ChangeTime)
                .Take(count)
                .ToArray();
        }

        /// <summary>
        /// 获取单条记录
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<RedisSyncData> FindRedisAsync(string userId, string Id)
        {
            var key = this.MergeKey(userId);
            var model = await this.GetDatabase().HashGetAsync(key, Id);
            return model.ToModel<RedisSyncData>();
        }

        /// <summary>
        /// 保存单条记录
        /// 如记录不存在，新增
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="TDemoTable">保实实体</param>
        /// <returns></returns>
        public async Task<bool> UpdateOrAddRedisAsync(string userId, TDemoTable model)
        {

            model.F_Guid = string.IsNullOrEmpty(model.F_Guid) ? Guid.NewGuid().ToString() : model.F_Guid;
            var data = new RedisSyncData()
            {
                ChangeID = model.F_Guid,
                TypeName = model.GetType().Name,
                ChangeTime = DateTime.Now,
                Action = DataAction.Update,
                Data = model
            };
            await this.AddToRedisAsync(new[] { userId }, new[] { data });
            return true;
        }

        /// <summary>
        /// 删除增量数据
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="changeId">要删除的增量数据的id</param>
        /// <returns>实际删除的条数</returns>
        public async Task<int> RemoveRedisAsync(string userId, params string[] changeId)
        {
            if (changeId == null || changeId.Length == 0)
            {
                return 0;
            }

            var key = this.MergeKey(userId);
            var db = this.GetDatabase();
            var fields = changeId.Select(id => (RedisValue)id).ToArray();
            return (int)await db.HashDeleteAsync(key, fields);
        }

        /// <summary>
        /// 删除所有用户的增量数据
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public async Task<bool> RemoveAllChangeDataAsync(string userId)
        {
            return await base.RemoveAsync(userId);
        }

        /// <summary>
        /// 获取订阅者回调接口
        /// </summary>
        /// <returns></returns>
        public async Task GetSubscriber(RedisChannel channel, Action<RedisChannel, RedisValue> act)
        {
            var sub = this.GetISubscriber();
            await sub.SubscribeAsync(Encryption.GetMD5(channel), act);
        }


        #region 发布增量数据
        /// <summary>
        /// 发布增量数据和通知,单条数据+多个用户
        /// </summary>
        /// <param name="action">数据行为</param>
        /// <param name="data">增量数据</param>
        /// <param name="userIds">发送的Userids</param>
        /// <param name="notify">是否要通知</param>
        /// <returns></returns>
        public async Task<int> Publish<T>(DataAction action, T data, string[] userIds, bool notify = true) where T : ISyncData
        {
            return await this.PublishSyncData<T>(action, new T[] { data }, userIds, notify);
        }

        /// <summary>
        /// 发布增量数据和通知,多条数据++多个用户
        /// </summary>
        /// <param name="action">数据行为</param>
        /// <param name="datas">增量数据</param>
        /// <param name="except">要排除的子系统</param>
        /// <param name="notify">是否要通知</param>
        /// <returns></returns>
        public async Task<int> Publish<T>(DataAction action, T[] datas, string[] userIds, bool notify = true) where T : ISyncData
        {
            return await this.PublishSyncData<T>(action, datas, userIds, notify);
        }

        /// <summary>
        /// 发布增量数据和通知,单条数据+单个用户
        /// </summary>
        /// <param name="action">数据行为</param>
        /// <param name="data">增量数据</param>
        /// <param name="userIds">发送的Userids</param>
        /// <param name="notify">是否要通知</param>
        /// <returns></returns>
        public async Task<int> Publish<T>(DataAction action, T data, string userIds, bool notify = true) where T : ISyncData
        {
            return await this.PublishSyncData<T>(action, new T[] { data }, new[] { userIds }, notify);
        }

        /// <summary>
        /// 发布增量数据和通知,多条数据+单个用户
        /// </summary>
        /// <param name="action">数据行为</param>
        /// <param name="datas">增量数据</param>
        /// <param name="except">要排除的子系统</param>
        /// <param name="notify">是否要通知</param>
        /// <returns></returns>
        public async Task<int> Publish<T>(DataAction action, T[] datas, string userIds, bool notify = true) where T : ISyncData
        {
            return await this.PublishSyncData<T>(action, datas, new[] { userIds }, notify);
        }


        #endregion

        #region Redis 处理方法
        /// <summary>
        /// 获取订阅者回调接口
        /// </summary>
        /// <returns></returns>
        private ISubscriber GetISubscriber()
        {
            return this.GetSubscriber();
        }


        /// <summary>
        /// 发布增量数据和通知
        /// </summary>
        /// <param name="action">数据行为</param>
        /// <param name="datas">增量数据</param>
        /// <param name="except">排除的子系统</param>
        /// <param name="notify">是否要通知</param>
        /// <returns></returns>
        private async Task<int> PublishSyncData<T>(DataAction action, T[] datas, string[] userIds, bool notify) where T : ISyncData
        {
            var syncDatas = datas.Select(item => new RedisSyncData
            {
                Action = action,
                ChangeID = string.IsNullOrEmpty(item.F_Guid) ? Guid.NewGuid().ToString() : item.F_Guid,
                ChangeTime = DateTime.Now,
                Data = item,
                TypeName = typeof(T).Name
            });

            await this.AddToRedisAsync(userIds, syncDatas);

            if (notify == true)
            {
                await this.TryNotify(userIds);
            }
            return syncDatas.Count();
        }

        /// <summary>
        /// 异步通知用户
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns></returns>
        private async Task TryNotify(IEnumerable<string> userIds)
        {
            var Subscriber = this.GetISubscriber();
            foreach (var channel in userIds)
            {
                await Subscriber.PublishAsync(Encryption.GetMD5(channel), "有新消息");
            }
        }

        /// <summary>
        /// 获取订阅者资料
        /// </summary>
        /// <returns></returns>
        private async Task<IEnumerable<RedisValue>> HashGetAsync(string[] userIds)
        {
            var db = this.GetDatabase();
            var fields = userIds.Select(id => (RedisValue)Encryption.GetMD5(id)).ToArray();
            var data = await db.HashGetAsync(NotifyHashKey, fields);
            return data;
        }

        /// <summary>
        /// 获取所有订阅者资料
        /// </summary>
        /// <returns></returns>
        private async Task<HashEntry[]> HashGetAllAsync()
        {
            var db = this.GetDatabase();
            var data = await db.HashGetAllAsync(NotifyHashKey);
            return data;
        }

        /// <summary>
        /// 将增量数据添加到Reids
        /// </summary>
        /// <param name="subSystems">子系统</param>
        /// <param name="datas">增量数据</param>   
        private async Task AddToRedisAsync(IEnumerable<string> userIds, IEnumerable<RedisSyncData> datas)
        {
            var db = this.GetDatabase();
            foreach (var id in userIds)
            {
                var key = this.MergeKey(id);
                foreach (var item in datas)
                {
                    await db.HashSetAsync(key, item.ChangeID, item.ToRedisValue());
                }
            }
        }
        #endregion
    }

}