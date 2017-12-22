using Infrastructure.Reflection;
using Infrastructure.Utility;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SqlServer
{
    /// <summary>
    /// Database查询扩展
    /// </summary>
    public static class SqlQueryExtend
    {
        /// <summary>
        /// 类型的属性缓存
        /// </summary>
        private static readonly ConcurrentDictionary<Type, ModelProperty[]> cacehd = new ConcurrentDictionary<Type, ModelProperty[]>();

        /// <summary>
        /// 获取类型的属性
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        private static Property[] GetProperties(Type type)
        {
            return cacehd.GetOrAdd(type, (t) => t.GetProperties()
                .Where(item => item.PropertyType != typeof(object))
                .Where(item => !item.IsDefined(typeof(NotMappedAttribute)))
                .Select(p => new ModelProperty(p))
                .ToArray());
        }

        /// <summary>
        /// 将sql查询映射为模型
        /// </summary>
        /// <typeparam name="T">模型</typeparam>
        /// <param name="db"></param>
        /// <param name="sql">sql语句</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public static IEnumerable<T> SqlQueryEx<T>(this Database db, string sql, params object[] parameters) where T : class,new()
        {
            return db.SqlQueryEx(typeof(T), sql, parameters).Cast<T>();
        }

        /// <summary>
        /// 将sql查询映射为模型
        /// </summary>
        /// <param name="modelType">模型类型</param>
        /// <param name="db"></param>
        /// <param name="sql">sql语句</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public static IEnumerable<object> SqlQueryEx(this Database db, Type modelType, string sql, params object[] parameters)
        {
            var cmd = db.Connection.CreateCommand();
            cmd.CommandText = SqlQueryExtend.GetSqlText(sql);
            cmd.Parameters.AddRange(SqlQueryExtend.GetParameters(cmd, parameters));
            if (cmd.Connection.State != ConnectionState.Open)
            {
                cmd.Connection.Open();
            }

            using (var reader = cmd.ExecuteReader())
            {
                Func<Property, bool> complexFunc = item =>
                    item.Info.PropertyType != typeof(string) &&
                    item.Info.PropertyType.IsClass &&
                    item.IsVirtual == false;

                var properties = SqlQueryExtend.GetProperties(modelType);
                var complex = properties.Where(complexFunc);
                var primitive = properties.Where(item => !complexFunc(item));

                while (reader.Read())
                {
                    var instance = Activator.CreateInstance(modelType);
                    SqlQueryExtend.SetProperties(instance, primitive, reader);
                    foreach (var item in complex)
                    {
                        SqlQueryExtend.SetComplexProperty(instance, item, reader);
                    }
                    yield return instance;
                }
            }
        }

        /// <summary>
        /// 设置复杂属性的值
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="property"></param>
        /// <param name="reader"></param>
        private static void SetComplexProperty(object instance, Property property, DbDataReader reader)
        {
            var type = property.Info.PropertyType;
            try
            {
                var propertyInstance = Activator.CreateInstance(type);
                var properties = SqlQueryExtend.GetProperties(type);

                SqlQueryExtend.SetProperties(propertyInstance, properties, reader);
                property.SetValue(instance, propertyInstance);
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 设置属性的值
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="properties"></param>
        /// <param name="reader"></param>
        private static void SetProperties(object instance, IEnumerable<Property> properties, DbDataReader reader)
        {
            foreach (var item in properties)
            {
                var value = reader[item.Name];
                if (value != DBNull.Value)
                {
                    var valueCast = Converter.Cast(value, item.Info.PropertyType);
                    item.SetValue(instance, valueCast);
                }
            }
        }

        /// <summary>
        /// 获取sql文本
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        private static string GetSqlText(string sql)
        {
            return Regex.Replace(sql, @"{\d+}", (m) => "@P" + Regex.Match(m.Value, @"\d+"));
        }

        /// <summary>
        /// 生成参数
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private static DbParameter[] GetParameters(DbCommand cmd, object[] parameters)
        {
            var dbParameters = new DbParameter[parameters.Length];
            for (var i = 0; i < parameters.Length; i++)
            {
                dbParameters[i] = cmd.CreateParameter();
                dbParameters[i].ParameterName = "@P" + i.ToString();
                dbParameters[i].Value = parameters[i];
            }
            return dbParameters;
        }

        /// <summary>
        /// 模型属性
        /// </summary>
        private class ModelProperty : Property
        {
            public ModelProperty(PropertyInfo property)
                : base(property)
            {
                var attribute = property.GetCustomAttributes(typeof(ColumnAttribute), false).FirstOrDefault() as ColumnAttribute;
                this.Name = attribute == null ? property.Name : attribute.Name;
            }
        }
    }
}
