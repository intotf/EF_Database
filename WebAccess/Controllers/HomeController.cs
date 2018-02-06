using AccessServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebAccess.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var db = AccDb.db;

            var list = db.TDemoTable.Where(item => true).ToList();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}