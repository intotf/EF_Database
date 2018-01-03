using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Infrastructure.Utility;
using System.Threading.Tasks;
using RedisClientLib;

namespace Web.Controllers
{
    public class RedisController : Controller
    {
        /// <summary>
        /// 获取前10条数据
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Index()
        {
            var userlist = await PubSubRedis.Instance.GetAllSubscribeAsync();
            var userId = "";
            if (userlist.Count() > 0)
            {
                userId = userlist.FirstOrDefault().Id;
            }

            if (Request["User"] != null)
            {
                userId = Request["User"].ToString();
            }

            var data = await PubSubRedis.Instance.GetChangeDatasAsync(userId, 10);
            var listModel = new List<TDemoTable>();
            foreach (var item in data)
            {
                var model = JsonSerializer.Deserialize<TDemoTable>(item.Data.ToString());
                model.F_Guid = item.ChangeID;
                listModel.Add(model);
            }

            ViewBag.UserId = userId;
            ViewBag.UserList = userlist.Select(item => new SelectListItem() { Text = item.Name, Value = item.Id, Selected = (userId == item.Id) });

            return View(listModel);
        }

        /// <summary>
        /// 创建订阅者
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateSubscribe()
        {
            var model = new RedisSubscriber();
            model.Id = TDemoTable.GetNewId().ToString();
            return View(model);
        }

        /// <summary>
        /// 创建订阅者
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> CreateSubscribe(RedisSubscriber model)
        {
            var state = await PubSubRedis.Instance.SubscribeAsync(model);
            return Json(new { state = state, value = "操作成功" });
        }


        public async Task<ActionResult> Create()
        {
            var model = new TDemoTable();
            model.F_DateTime = DateTime.Now;
            model.F_Bool = true;
            model.F_Float = 0.01f;
            model.F_Int = 0;
            model.F_BoolNull = true;
            model.CreateTime = DateTime.Now;
            model.Id = TDemoTable.GetNewId();

            var userlist = await PubSubRedis.Instance.GetAllSubscribeAsync();
            ViewBag.UserList = userlist.Select(item => new SelectListItem() { Text = item.Name, Value = item.Id });
            return View(model);
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> Create(TDemoTable model, string UserId)
        {
            var user = await PubSubRedis.Instance.FindSubscribeAsync(UserId);
            if (user == null)
            {
                return Json(new { state = false, value = "操作失败" });
            }
            model.F_Guid = Guid.NewGuid().ToString();
            model.CreateTime = DateTime.Now;
            var state = await PubSubRedis.Instance.Publish(DataAction.Add, model, UserId);
            return Json(new { state = state > 0, value = "操作成功" });
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> Edit(string userId, string id)
        {
            var redisModel = await PubSubRedis.Instance.FindRedisAsync(userId, id);
            var model = JsonSerializer.Deserialize<TDemoTable>(redisModel.Data.ToString());

            var userlist = await PubSubRedis.Instance.GetAllSubscribeAsync();
            ViewBag.UserList = userlist.Select(item => new SelectListItem() { Text = item.Name, Value = item.Id, Selected = (userId == item.Id) });

            return View("Create", model);
        }


        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> Edit(TDemoTable model, string userId)
        {
            if (!this.TryValidateModel(model))
            {
                return Json(new { state = false, value = this.ModelState.FirstModelErrorMessage() });
            }
            var state = await PubSubRedis.Instance.UpdateOrAddRedisAsync(userId, model);
            return Json(new { state = state, value = "编辑成功" });
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> Remove(string id, string userId)
        {
            var state = await PubSubRedis.Instance.RemoveRedisAsync(userId, id);

            return Json(new { state = state > 0, value = "删除成功" });
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> RemoveAllSubscribe()
        {
            var userlist = await PubSubRedis.Instance.GetAllSubscribeAsync();
            foreach (var item in userlist)
            {
                await PubSubRedis.Instance.UnSubscribe(item.Id);
            }

            return Json(new { state = true, value = "删除成功" });
        }
    }
}