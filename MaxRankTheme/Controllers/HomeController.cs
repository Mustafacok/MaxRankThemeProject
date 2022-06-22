using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaxRankTheme.Models;

namespace MaxRankTheme.Controllers
{
    public class HomeController : Controller
    {
        KategoriBLL _kategori=new KategoriBLL();
        public ActionResult Index()
        {
            return View(_kategori.Get(Size:4));
        }
        public ActionResult Slider()
        {
            return PartialView();
        }
        public ActionResult Referans()
        {
            return PartialView();
        }
        public ActionResult Header()
        {
            return PartialView();
        }
        public ActionResult Footer()
        {
            return PartialView();
        }
    }
}