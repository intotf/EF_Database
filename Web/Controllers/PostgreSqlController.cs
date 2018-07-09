using PostgreSqlServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using Model;

namespace Web.Controllers
{
    public class PostgreSqlController : Controller
    {
        // GET: PostgreSql
        public async Task<ActionResult> Index()
        {
            using (var db = new PSqlDb())
            {
                var data = await db.NpgSqlTable.Where(item => true).ToListAsync();
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
            return View("Create", model);
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> Create(NpgSqlTable model)
        {
            model.F_Guid = Guid.NewGuid().ToString();
            using (var db = new PSqlDb())
            {
                db.NpgSqlTable.Add(model);
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
            using (var db = new PSqlDb())
            {
                var model = await db.NpgSqlTable.FindAsync(id);
                return View("Create", model);
            }
        }


        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> Edit(NpgSqlTable model)
        {
            if (!this.TryValidateModel(model))
            {
                return Json(new { state = false, value = this.ModelState.FirstModelErrorMessage() });
            }

            using (var db = new PSqlDb())
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
            using (var db = new PSqlDb())
            {
                var model = await db.NpgSqlTable.FindAsync(id);
                db.NpgSqlTable.Remove(model);
                await db.SaveChangesAsync();
                return Json(new { state = true, value = "删除成功" });
            }
        }
    }
}