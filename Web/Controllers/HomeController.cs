using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "EF_DataBase 是一个连接各类DB 小实例，包含Web及WinForm 两种方式调用.对指定数据库的读取、添加、编辑、删除基本功能。";
            return View();
        }
    }
}