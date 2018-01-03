using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// 保存在Redis的同步数据
    /// </summary>    
    public class RedisSyncData
    {
        /// <summary>
        /// 变化数据id
        /// </summary>
        public string ChangeID { get; set; }

        /// <summary>
        /// 数据类型名称
        /// 例如为TDemoTable时，TypeName则为TDemoTable的内容
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// 数据行为
        /// </summary>
        public DataAction Action { get; set; }

        /// <summary>
        /// 数据变化时间
        /// </summary>
        public DateTime ChangeTime { get; set; }

        /// <summary>
        /// 数据内容
        /// 对应所有基础数据
        /// </summary>
        public virtual object Data { get; set; }

        /// <summary>
        /// 转换为泛型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public RedisSyncData<T> Generic<T>()
        {
            var model = new RedisSyncData<T>
            {
                Action = this.Action,
                ChangeID = this.ChangeID,
                ChangeTime = this.ChangeTime,
                Data = this.Data,
                TypeName = this.TypeName
            };

            var data = this.Data as JToken;
            if (data != null)
            {
                model.TData = data.ToObject<T>();
            }
            return model;
        }

        /// <summary>
        /// 转换为字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}：{1} {2} {3}", this.ChangeTime, this.ChangeID, this.Action, this.TypeName);
        }
    }

    /// <summary>
    /// 保存在Redis的同步数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RedisSyncData<T> : RedisSyncData
    {
        /// <summary>
        /// 数据内容
        /// 对应所有基础数据
        /// </summary>
        public T TData { get; set; }

        /// <summary>
        /// 数据内容
        /// 对应所有基础数据
        /// </summary>
        public override object Data
        {
            get
            {
                return this.TData;
            }
            set
            {
                base.Data = this.TData;
            }
        }

        /// <summary>
        /// 保存在Redis的同步数据
        /// </summary>
        internal RedisSyncData()
        {
        }
    }
}
