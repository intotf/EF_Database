using Model;
using SqlServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Infrastructure;
using Command;
using SqlServer.Server;

namespace Web.Controllers
{
    public class SqlServerController : Controller
    {
        // GET: SqlServer
        public async Task<ActionResult> Index(int? pageindex)
        {
            var page = pageindex ?? 1;
            var pageSize = 10;
            var order = " Id desc";
            using (var db = new SqlDb())
            {
                var where = Where.True<TDemoTable>();
                var F_String = Request["F_String"];
                if (F_String != null && !F_String.ToString().IsNullOrEmpty())
                {
                    where = where.And(item => item.F_String.Contains(F_String.ToString()));
                }
                var data = await db.TDemoTable.Where(where).ToPageAsync(order, page, pageSize);
                return View(data);
            }
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
            return View(model);
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> Create(TDemoTable model)
        {
            model.F_Guid = Guid.NewGuid().ToString();
            model.CreateTime = DateTime.Now;
            SqlDb.inter.Add(model);
            await SqlDb.inter.SaveChangesAsync();
            return Json(new { state = true, value = "操作成功" });
            ////using (var db = new SqlDb())
            ////{
            ////    db.TDemoTable.Add(model);
            ////    await db.SaveChangesAsync();

            ////    return Json(new { state = true, value = "操作成功" });
            ////}
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> Edit(int id)
        {
            using (var db = new SqlDb())
            {
                var model = await db.TDemoTable.FindAsync(id);
                return View("Create", model);
            }
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

            using (var db = new SqlDb())
            {
                db.Entry(model).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return Json(new { state = true, value = "编辑成功" });
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> Remove(int id)
        {
            using (var db = new SqlDb())
            {
                await db.TDemoTable.RemoveById(id);
                await db.SaveChangesAsync();
                return Json(new { state = true, value = "删除成功" });
            }
        }
    }
}