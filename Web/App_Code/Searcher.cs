using Infrastructure.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;


namespace System.Web.Mvc
{
    /// <summary>
    /// 提供搜索内容查询
    /// </summary>
    public static class Searcher
    {
        /// <summary>
        /// 获取排序字符串
        /// </summary>
        public static string OrderBy
        {
            get
            {
                return Searcher.GetValue("OrderBy");
            }
        }

        /// <summary>
        /// 获取Keyword
        /// </summary>
        public static string Keyword
        {
            get
            {
                return Searcher.GetValue("Keyword");
            }
        }

        /// <summary>
        /// 获取指定的值
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns></returns>
        public static string GetValue(string name)
        {
            var cookie = HttpContext.Current.Request.Cookies["search"];
            if (cookie == null)
            {
                return null;
            }

            var values = cookie.Values.GetValues(name);
            if (values == null || values.Length == 0)
            {
                return null;
            }

            var value = HttpUtility.UrlDecode(values.FirstOrDefault()).Trim();
            if (value.IsNullOrEmpty())
            {
                return null;
            }
            return value;
        }

        /// <summary>
        /// 表示默认为true的搜索结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Searcher<T> True<T>()
        {
            return new Searcher<T>(Where.True<T>());
        }

        /// <summary>
        /// 表示默认为false的搜索结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Searcher<T> False<T>()
        {
            return new Searcher<T>(Where.True<T>());
        }
    }

    /// <summary>
    /// 表示搜索结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Searcher<T>
    {
        /// <summary>
        /// 当前表达式
        /// </summary>
        private Expression<Func<T, bool>> exp;

        /// <summary>
        /// 表示搜索结果
        /// </summary>
        /// <param name="trueOrFalse"></param>
        public Searcher(Expression<Func<T, bool>> trueOrFalse)
        {
            this.exp = trueOrFalse;
        }

        /// <summary>
        /// 获取表达式
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="keySelector"></param>
        /// <param name="op"></param>
        /// <returns></returns>
        private Expression<Func<T, bool>> GetPredicate<TKey>(Expression<Func<T, TKey>> keySelector, Operator op)
        {
            var field = keySelector.Body.OfType<MemberExpression>().Member.Name;
            var value = Searcher.GetValue(field);
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }
            else
            {
                var targetValue = Converter.Cast(value, typeof(TKey));
                return Where.GetPredicate<T>(field, targetValue, op);
            }
        }

        ///// <summary>
        ///// 获取表达式
        ///// </summary>
        ///// <typeparam name="TKey"></typeparam>
        ///// <param name="keySelector"></param>
        ///// <param name="op"></param>
        ///// <returns></returns>
        //private Expression<Func<T, bool>> GetPredicate<TKey>(Expression<Func<T, TKey>> keySelector, Operator op)
        //{
        //    var field = keySelector.Body.OfType<MemberExpression>().Member.Name;
        //    var value = Searcher.GetValue(field);
        //    if (string.IsNullOrWhiteSpace(value))
        //    {
        //        return null;
        //    }
        //    else
        //    {
        //        var targetValue = Converter.Cast<TKey>(value);
        //        return Where.GetPredicate<T>(keySelector, targetValue, op);
        //    }
        //}

        /// <summary>
        /// And运算
        /// 文本类型为Contains
        /// 其它类型为Equal
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="keySelector">健</param>
        /// <returns></returns>
        public Searcher<T> And<TKey>(Expression<Func<T, TKey>> keySelector)
        {
            var op = Operator.Equal;
            if (typeof(TKey) == typeof(string))
            {
                op = Operator.Contains;
            }
            return this.And(keySelector, op);
        }

        /// <summary>
        /// And运算
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="keySelector">健</param>
        /// <param name="op">操作符</param>
        /// <returns></returns>
        public Searcher<T> And<TKey>(Expression<Func<T, TKey>> keySelector, Operator op)
        {
            var expRight = this.GetPredicate(keySelector, op);
            if (expRight != null)
            {
                this.exp = this.exp.And(expRight);
            }
            return this;
        }

        /// <summary>
        /// Or运算
        /// 文本类型为Contains
        /// 其它类型为Equal
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="keySelector">健</param>
        /// <returns></returns>
        public Searcher<T> Or<TKey>(Expression<Func<T, TKey>> keySelector)
        {
            var op = Operator.Equal;
            if (typeof(TKey) == typeof(string))
            {
                op = Operator.Contains;
            }

            return this.Or(keySelector, op);
        }

        /// <summary>
        /// Or运算
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="keySelector">健</param>
        /// <param name="op">操作符</param>
        /// <returns></returns>
        public Searcher<T> Or<TKey>(Expression<Func<T, TKey>> keySelector, Operator op)
        {
            var expRight = this.GetPredicate(keySelector, op);
            if (expRight != null)
            {
                this.exp = this.exp.Or(expRight);
            }
            return this;
        }

        /// <summary>
        /// 转换为条件表达式
        /// </summary>
        /// <returns></returns>
        public Expression<Func<T, bool>> ToExpression()
        {
            return this.exp;
        }

        /// <summary>
        /// 转换为字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.exp.ToString();
        }
    }
}
