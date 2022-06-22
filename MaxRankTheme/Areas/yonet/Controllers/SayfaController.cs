using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaxRankTheme.Models;
using MaxRankTheme.n2;

namespace MaxRankTheme.Areas.yonet.Controllers
{
    public class SayfaController : Controller
    {
        // GET: yonet/Sayfa
        SayfaBLL _sayfa = new SayfaBLL();
        public ActionResult Index(string q ="")
        {
            int count = 0;
            var model = _sayfa.Get(out count,g=>g.Adi.Contains(q));
            ViewBag.Count = count;
            return View(model);
        }
        public ActionResult IndexDetay(string term = "")
        {
            int count = 0;
            var model = _sayfa.Get(out count, g=>g.Adi.Contains(term));
            ViewBag.Count = count;
            return PartialView(model);
        }
        public ActionResult AramaYap(string term)
        {
            var model = _sayfa.Get(g => g.Adi.Contains(term));
            var result = model.Select(s => new { adi = s.Adi, id = s.Id }).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult KayitSil(string cbSecili = "")
        {
            if (cbSecili != "")
            {
                var parcala = cbSecili.Split(',').Select(Int32.Parse).ToList();
                foreach (var item in parcala)
                {
                    _sayfa.Delete(item);
                }
                
            }
            return RedirectToAction("Index","Sayfa");
        }
        public ActionResult Detay(int Id = 0)
        {
            var model = _sayfa.GetById(Id);
            ViewBag.Mesaj = GenelAraclarBLL.KayitYeni();
            return View(model);
        }
        [HttpPost]
        public ActionResult Detay(Sayfa model, HttpPostedFileBase fGorsel)
        {
            try
            {
                if (fGorsel != null && fGorsel.ContentLength > 0)
                {
                    model.Gorsel = fGorsel.FileName;
                    fGorsel.SaveAs(HttpContext.Request.PhysicalApplicationPath + "img/sayfa/" + fGorsel.FileName);
                }
                _sayfa.InsertOrUpdate(model, model.Id);
                ViewBag.Mesaj = GenelAraclarBLL.KayitBasarili();
                return View(model);
            }
            catch (Exception ex)
            {
                ViewBag.Mesaj = GenelAraclarBLL.KayitHatali(mesaj: ex.Message);
                return View(model);
            }
        }
    }
}