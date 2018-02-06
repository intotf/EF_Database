using SqlLiteServer;
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
    public class SqlLiteController : Controller
    {
        // GET: SqlLite
        public async Task<ActionResult> Index()
        {
            using (var db = new SqlLiteDb(HttpContext.Server.MapPath(@"~\App_Data\data.db")))
            {
                var list = await db.SqlLiteModel.Where(item => true).ToArrayAsync();
                return View(list);
            }
        }

        public ActionResult Create()
        {
            var model = new SqlLiteModel();
            model.F_DateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            model.F_Bool = true;
            model.F_Float = 0.01f;
            model.F_Int = SqlLiteModel.GetNewId();
            model.F_BoolNull = true;
            model.CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            return View("Create", model);
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> Create(SqlLiteModel model)
        {
            model.F_Guid = Guid.NewGuid().ToString();
            using (var db = new SqlLiteDb(HttpContext.Server.MapPath(@"~\App_Data\data.db")))
            {
                db.SqlLiteModel.Add(model);
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
            using (var db = new SqlLiteDb(HttpContext.Server.MapPath(@"~\App_Data\data.db")))
            {
                var model = await db.SqlLiteModel.FindAsync(id);
                return View("Create", model);
            }
        }


        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> Edit(SqlLiteModel model)
        {
            if (!this.TryValidateModel(model))
            {
                return Json(new { state = false, value = this.ModelState.FirstModelErrorMessage() });
            }

            using (var db = new SqlLiteDb(HttpContext.Server.MapPath(@"~\App_Data\data.db")))
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
            using (var db = new SqlLiteDb(HttpContext.Server.MapPath(@"~\App_Data\data.db")))
            {
                var model = await db.SqlLiteModel.FindAsync(id);
                db.SqlLiteModel.Remove(model);
                await db.SaveChangesAsync();
                return Json(new { state = true, value = "删除成功" });
            }
        }
    }
}