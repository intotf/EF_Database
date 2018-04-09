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
    /// <summary>
    /// MongoDb 操作 
    /// </summary>
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
            model.F_Int = 0;
            model.F_BoolNull = true;
            model.CreateTime = DateTime.Now;
            model.Id = TDemoTable.GetNewId();
            model.F_Guid = Guid.NewGuid().ToString();
            return View("Create", model);
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> Create(TDemoTable model)
        {
            await db.TDemoTable().AddAsync(model);
            return Json(new { state = true, value = "操作成功" });
        }

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <returns></returns>
        /// <param name="Count">批量添加数量</param>
        [HttpPost]
        public async Task<JsonResult> CreateRange(int Count)
        {
            var list = new List<TDemoTable>();
            for (int i = 0; i < Count; i++)
            {
                var model = new TDemoTable();
                model.F_DateTime = DateTime.Now;
                model.F_Bool = true;
                model.F_Float = 0.01f;
                model.F_Int = 0;
                model.F_BoolNull = true;
                model.CreateTime = DateTime.Now;
                model.F_Guid = Guid.NewGuid().ToString();
                model.Id = model.F_Guid.GetHashCode();
                list.Add(model);
            }
            await db.TDemoTable().AddRangeAsync(list);
            return Json(new { state = true, value = "批量成功添加 " + Count + "条记录." });
        }

        /// <summary>
        /// 清空数据库
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> RemoveAll()
        {
            await db.TDemoTable().RemoveAsync(item => true);
            return Json(new { state = true, value = "清空数据完成." });
        }

        /// <summary>
        /// 单个字段修改
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> EditMany()
        {
            /////修改的属性集合
            //var fieldList = new List<UpdateDefinition<TDemoTable>>();

            //fieldList.Add(Builders<TDemoTable>.Update.Set(item => item.F_String, "单条个修改"));

            //if (fieldList.Count > 0)
            //{
            //    var builders = Builders<TDemoTable>.Update.Combine(fieldList);
            //    await db.TDemoTable().UpdateOneAsync(item => item.Id == 904616499, builders);
            //}

            var model = new TDemoTable();
            model.F_DateTime = DateTime.Now;
            model.F_Bool = true;
            model.F_Float = 0.01f;
            model.F_Int = 0;
            model.F_BoolNull = true;
            model.CreateTime = DateTime.Now;
            model.F_Guid = Guid.NewGuid().ToString();
            model.F_String = "只修改该字段";
            model.Id = 904616499;
            await db.TDemoTable().UpdateOneAsync(item => item.Id == model.Id, () => new TDemoTable { F_Int = 55555 });
            return Json(new { state = true, value = "批量编辑成功" });
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

            await db.TDemoTable().UpdateOneAsync(item => item.Id == model.Id, model);

            //指定指定修改
            //await db.TDemoTable().UpdateOneAsync(item => item.Id == model.Id, () => new TDemoTable { F_Int = 55555 });

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