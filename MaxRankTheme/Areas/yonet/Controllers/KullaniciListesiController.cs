using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaxRankTheme.Models;
using MaxRankTheme.n2;

namespace MaxRankTheme.Areas.yonet.Controllers
{
    public class KullaniciListesiController : Controller
    {
        // GET: yonet/Kullanici
        KullaniciListesiBLL _kullanici = new KullaniciListesiBLL();
        public ActionResult Index(string q="")
        {
            int count = 0;
            var model = _kullanici.Get(out count, g=>g.Adisoyadi.Contains(q));
            ViewBag.Count = count;
            return View(model);
        }
        public ActionResult IndexDetay(string term="")
        {
            int count = 0;
            var model = _kullanici.Get(out count, g => g.Adisoyadi.Contains(term));
            ViewBag.Count = count;
            return PartialView(model);
        }
        public ActionResult AramaYap(string term)
        {
            var model = _kullanici.Get(g => g.Adisoyadi.Contains(term));
            var result = model.Select(s => new { adisoyadi = s.Adisoyadi, id = s.Id }).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult KayitSil(string cbSecili = "")
        {
            if (cbSecili !="")
            {
                var parcala = cbSecili.Split(',').Select(Int32.Parse).ToList();
                foreach (var item in parcala)
                {
                    _kullanici.Delete(item);
                }
            }
            return RedirectToAction("Index","KullaniciListesi");
        }
        public ActionResult Detay(int Id=0)
        {
            var model = _kullanici.GetById(Id);
            ViewBag.Mesaj = GenelAraclarBLL.KayitYeni();
            return View(model);
        }

        [HttpPost]
        public ActionResult Detay(KullaniciListesi model)
        {
            try
            {
                _kullanici.InsertOrUpdate(model, model.Id);
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