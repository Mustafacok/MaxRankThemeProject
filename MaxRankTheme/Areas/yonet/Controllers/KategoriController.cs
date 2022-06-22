using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaxRankTheme.n2;
using MaxRankTheme.Models;

namespace MaxRankTheme.Areas.yonet.Controllers
{
    public class KategoriController : Controller
    {
        // GET: yonet/Kategori
        KategoriBLL _kategori = new KategoriBLL();
        public ActionResult Index(string q = "")
        {
            int count = 0;
            var model = _kategori.Get(out count, g => g.Adi.Contains(q));
            ViewBag.Count = count;
            return View(model);
        }
        public ActionResult IndexDetay(string term = "")
        {
            int count = 0;
            var model = _kategori.Get(out count, g => g.Adi.Contains(term));
            ViewBag.Count = count;
            return PartialView(model);
        }
        public ActionResult AramaYap(string term)
        {
            var model = _kategori.Get(g => g.Adi.Contains(term));
            var result = model.
                Select(s => new { adi = s.Adi, id = s.Id }).ToList();
            return Json(result, JsonRequestBehavior.AllowGet); //gelen istek davranış sonuncusu
        }
        [HttpPost]
        public ActionResult KayitSil(string cbSecili = "")
        {
            if (cbSecili != "")
            {
                var parcala = cbSecili.Split(',').Select(Int32.Parse).ToList();
                foreach (var item in parcala)
                {
                    _kategori.Delete(item);
                }
            }
            return RedirectToAction("Index", "Kategori");
        }
        public ActionResult Detay(int Id = 0)
        {
            var model = _kategori.GetById(Id);
            ViewBag.Mesaj = GenelAraclarBLL.KayitYeni();
            return View(model);
        }
        [HttpPost]
        public ActionResult Detay(Kategori model, HttpPostedFileBase fGorsel)
        {
            try
            {
                if (fGorsel != null && fGorsel.ContentLength > 0)
                {
                    model.Gorsel = fGorsel.FileName;
                    fGorsel.SaveAs(HttpContext.Request.PhysicalApplicationPath+ "img/kategori/" + fGorsel.FileName);
                }
                _kategori.InsertOrUpdate(model, model.Id);
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