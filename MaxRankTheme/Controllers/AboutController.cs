using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaxRankTheme.Models;

namespace MaxRankTheme.Controllers
{
    public class AboutController : Controller
    {
        // GET: About
        SayfaBLL _sayfa = new SayfaBLL();
        public ActionResult Index(string Adi ="")
        {
            var liste = _sayfa.GetAll();
            var model = liste.FirstOrDefault(f=>f.SEOLink== Adi);
            return View(model);
        }
    }
}