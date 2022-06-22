using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MaxRankTheme.Areas.yonet.Controllers
{
    public class HomeController : Controller
    {
        // GET: yonet/Home
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Header()
        {
            return PartialView();
        }
        public ActionResult Footer()
        {
            return PartialView();
        }
        public ActionResult Uyari(string name)
        {
            ViewBag.name = name;
            return PartialView();
        }
    }
}