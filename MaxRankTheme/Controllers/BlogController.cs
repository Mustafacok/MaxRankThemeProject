using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaxRankTheme.Models;

namespace MaxRankTheme.Controllers
{
    public class BlogController : Controller
    {
        // GET: Blog
        KategoriBLL _kategori = new KategoriBLL();
        public ActionResult Index(string Adi = "")
        {
            var liste = _kategori.GetAll();
            var model = liste.FirstOrDefault(f => f.SEOLink == Adi);
            return View(model);
        }
    }
}