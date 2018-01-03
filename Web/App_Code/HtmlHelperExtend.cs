using Infrastructure.Page;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc.Ajax;
using System.Web.UI;
using Webdiyer.WebControls.Mvc;


namespace System.Web.Mvc.Html
{
    /// <summary>
    /// Html分页相关扩展
    /// </summary>
    public static partial class HtmlHelperExtend
    {
        /// <summary>
        /// 查询Form
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static MvcForm PageSearchForm(this HtmlHelper html)
        {
            return html.BeginForm(null, null, FormMethod.Get, new { data_search = "search-form" });
        }

        /// <summary>
        /// 生成分页数据容器和分页数据
        /// </summary>
        /// <param name="html"></param>   
        /// <param name="partialView">视图</param>
        /// <returns></returns>
        public static IHtmlString PageContainer<T>(this HtmlHelper<T> html, string partialView = "ToPage")
        {
            var model = html.ViewData.Model;
            var container = new TagBuilder("div");
            container.Attributes.Add("id", "page-container");
            container.InnerHtml = html.Partial(partialView, model).ToString();
            return new HtmlString(container.ToString());
        }


        /// <summary>
        /// 生成分页控件
        /// </summary>        
        /// <param name="html"></param>   
        /// <returns></returns>
        public static IHtmlString PageControl<T>(this HtmlHelper<T> html) where T : IPageInfo
        {
            var model = new PagedList(html.ViewData.Model);
            var ajax = new AjaxHelper(html.ViewContext, html.ViewDataContainer);
            var htmlBuilder = new StringBuilder();
            htmlBuilder.Append("<div class=\"pagewrap\">");
            //htmlBuilder.Append("<div class=\"pageSizebox\">每页显示");
            //var list = new List<SelectListItem>()
            //{
            //        new SelectListItem() { Value = "10", Text = "10", Selected = model.PageSize == 10 },
            //        new SelectListItem() { Value = "20", Text = "20", Selected = model.PageSize == 20 },
            //        new SelectListItem() { Value = "50", Text = "50", Selected = model.PageSize == 50 },
            //        new SelectListItem() { Value = "100", Text = "100", Selected = model.PageSize == 100 }
            //};
            //htmlBuilder.Append(html.DropDownList("pageSizebox", list, new { datapagesize = "", @style = "height:28px;" }).ToHtmlString());
            //htmlBuilder.Append("条</div>");
            htmlBuilder.Append(ajax.Pager(model,
                new PagerOptions
                {
                    PageIndexParameterName = "pageindex",
                    CurrentPagerItemTemplate = "<a class=\"active\">{0}</a>",
                    ShowFirstLast = true,
                    ContainerTagName = "div",
                    DisabledPagerItemTemplate = "<a class=\"disabled\">{0}</a>",
                    CssClass = "page-items",
                    //PageIndexBoxId = "pagebox",
                    //GoToButtonId = "goBtn",
                    AutoHide = false
                },
                new MvcAjaxOptions
                {
                    UpdateTargetId = "page-container",
                    HttpMethod = "post",
                    EnablePartialLoading = false,
                    InsertionMode = InsertionMode.Replace,
                    OnBegin = "showMask($('#page-container'))",
                }).ToHtmlString());

            //htmlBuilder.Append("<div class=\"PageIndexBox\"><span>转到第</span><input type=\"text\"  id=\"pagebox\" value=\"\" /><span>页</span><span><button class=\"btn btn-info btn-sm\" id=\"goBtn\">跳转</button></span></div>");
            htmlBuilder.Append("</div>");

            return new HtmlString(htmlBuilder.ToString());
        }

        /// <summary>
        /// 生成表头排序属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="html"></param>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public static IHtmlString SortFor<T, TKey>(this HtmlHelper<T> html, Expression<Func<T, TKey>> keySelector) where T : IPageInfo
        {
            var orderBy = Searcher.OrderBy;
            var field = (keySelector.Body as MemberExpression).Member.Name.ToLower();

            if (orderBy == null || !orderBy.StartsWith(field, StringComparison.OrdinalIgnoreCase))
            {
                return new HtmlString(string.Format("field=\"{0}\"", field));
            }
            else
            {
                var sort = orderBy.EndsWith("desc", StringComparison.OrdinalIgnoreCase) ? "desc" : "asc";
                return new HtmlString(string.Format("field=\"{0}\" sort=\"{1}\"", field, sort));
            }
        }

        /// <summary>
        /// 搜索文本框
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="html"></param>
        /// <param name="keySelector"></param>
        /// <param name="htmlAttribute"></param>
        /// <returns></returns>
        public static IHtmlString TextBoxSearchFor<T, TKey>(this HtmlHelper<T> html, Expression<Func<T, TKey>> keySelector, object htmlAttribute) where T : IPageInfo
        {
            var field = keySelector.Body.OfType<MemberExpression>().Member.Name;
            var value = Searcher.GetValue(field);
            return html.TextBox(field, value, htmlAttribute);
        }

        /// <summary>
        /// 搜索下拉框
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="html"></param>
        /// <param name="keySelector"></param>
        /// <param name="selectList"></param>
        /// <param name="optionLabel"></param>
        /// <param name="htmlAttribute"></param>
        /// <returns></returns>
        public static IHtmlString DrowdownListSearchFor<T, TKey>(this HtmlHelper<T> html, Expression<Func<T, TKey>> keySelector, IEnumerable<SelectListItem> selectList, string optionLabel, object htmlAttribute) where T : IPageInfo
        {
            var field = keySelector.Body.OfType<MemberExpression>().Member.Name;
            var value = Searcher.GetValue(field);

            var selectItems = selectList == null ? new SelectListItem[0] : selectList.ToArray();
            foreach (var item in selectItems)
            {
                item.Selected = item.Value == value;
            }
            return html.DropDownList(field, selectItems, optionLabel, htmlAttribute);
        }

        /// <summary>
        /// IPagedList的实现
        /// </summary>
        private class PagedList : IPagedList
        {
            private readonly IPageInfo pageInfo;

            public PagedList(IPageInfo page)
            {
                this.pageInfo = page;
                this.TotalItemCount = pageInfo.TotalCount;
                this.PageSize = pageInfo.PageSize;
                this.CurrentPageIndex = pageInfo.PageIndex + 1;
            }

            public int CurrentPageIndex { get; set; }
            public int PageSize { get; set; }
            public int TotalItemCount { get; set; }
            public System.Collections.IEnumerator GetEnumerator()
            {
                return this.pageInfo.GetEnumerator();
            }
        }
    }
}