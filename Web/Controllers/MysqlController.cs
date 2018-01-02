using Model;
using MysqlServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Command;

namespace Web.Controllers
{
    public class MysqlController : Controller
    {
        // GET: Mysql
        public async Task<ActionResult> Index(int? pageindex)
        {
            var page = pageindex ?? 1;
            var pageSize = 10;
            var order = " Id desc";
            using (var db = new MysqlDb())
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
            model.F_Int = TDemoTable.GetNewId();
            model.F_BoolNull = true;
            model.CreateTime = DateTime.Now;
            return View("Create",model);
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> Create(TDemoTable model)
        {
            model.F_Guid = Guid.NewGuid().ToString();
            using (var db = new MysqlDb())
            {
                db.TDemoTable.Add(model);
                await db.SaveChangesAsync();

                return Json(new { state = true, value = "操作成功" });
            }
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> Edit(int id)
        {
            using (var db = new MysqlDb())
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

            using (var db = new MysqlDb())
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
            using (var db = new MysqlDb())
            {
                await db.TDemoTable.RemoveById(id);
                await db.SaveChangesAsync();
                return Json(new { state = true, value = "删除成功" });
            }
        }
    }
}