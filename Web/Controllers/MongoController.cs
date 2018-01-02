using Model;
using MongoServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Infrastructure;
using MongoDB.Driver;

namespace Web.Controllers
{
    public class MongoController : Controller
    {
        private MongoDbBase db = new MongoDbBase();
        // GET: Mysql
        public async Task<ActionResult> Index(int? pageindex)
        {
            var page = pageindex ?? 0;
            var pageSize = 10;

            var where = Where.True<TDemoTable>();
            var F_String = Request["F_String"];
            if (F_String != null && !F_String.ToString().IsNullOrEmpty())
            {
                where = where.And(item => item.F_String.Contains(F_String.ToString()));
            }
            var data = await db.TDemoTable().ToPageAsync(where, page, pageSize);
            return View(data);

        }


        public ActionResult Create()
        {
            var model = new TDemoTable();
            model.F_DateTime = DateTime.Now;
            model.F_Bool = true;
            model.F_Float = 0.01f;
            model.F_Int =  0;
            model.F_BoolNull = true;
            model.CreateTime = DateTime.Now;
            model.Id = TDemoTable.GetNewId();
            return View("Create", model);
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> Create(TDemoTable model)
        {
            model.F_Guid = Guid.NewGuid().ToString();

            await db.TDemoTable().AddAsync(model);
            return Json(new { state = true, value = "操作成功" });
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> Edit(int id)
        {
            var model = await db.TDemoTable().FindAsync(item => item.Id == id);
            return View("Create", model);
        }


        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> Edit(TDemoTable model)
        {
            if (!this.TryValidateModel(model))
            {
                return Json(new { state = false, value = this.ModelState.FirstModelErrorMessage() });
            }

            ///修改的属性集合
            var fieldList = new List<UpdateDefinition<TDemoTable>>();

            foreach (var item in model.GetType().GetProperties())
            {
                var replaceValue = item.GetValue(model);
                if (replaceValue != null)
                {
                    fieldList.Add(Builders<TDemoTable>.Update.Set(item.Name, replaceValue));
                }
            }

            if (fieldList.Count > 0)
            {
                var builders = Builders<TDemoTable>.Update.Combine(fieldList);
                await db.TDemoTable().UpdateAsync(item => item.Id == model.Id, builders);
            }

            return Json(new { state = true, value = "编辑成功" });
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> Remove(int id)
        {
            await db.TDemoTable().RemoveAsync(item => item.Id == id);
            return Json(new { state = true, value = "删除成功" });
        }
    }
}