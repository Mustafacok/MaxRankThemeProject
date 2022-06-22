using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaxRankTheme.Models;

namespace MaxRankTheme.Controllers
{
    public class SayfaController : Controller
    {
        // GET: Sayfa
        SayfaBLL _sayfa = new SayfaBLL();
        public ActionResult Index(int Id=0)
        {
            if (Id<=0)
            {
                return RedirectToAction("Index", "Hata");
            }
            var model = _sayfa.GetById(Id);
            if (model.Id ==0)
            {
                return RedirectToAction("Index", "Hata");
            }
            return View(model);
        }
    }
}