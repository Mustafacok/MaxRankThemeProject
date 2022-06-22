using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaxRankTheme.Models;
using MaxRankTheme.n2;

namespace MaxRankTheme.Areas.yonet.Controllers
{
    public class BlgHaberController : Controller
    {
        // GET: yonet/BlgHaber
        BlgHaberBLL _blghaber = new BlgHaberBLL();
        public ActionResult Index(string q="")
        {
            int count = 0;
            var model = _blghaber.Get(out count, g=>g.Baslik.Contains(q));
            ViewBag.Count = count;
            return View(model);
        }
        public ActionResult IndexDetay(string term="")
        {
            int count = 0;
            var model = _blghaber.Get(out count, g=>g.Baslik.Contains(term));
            ViewBag.Count = count;
            return PartialView(model);
        }
        public ActionResult AramaYap(string term)
        {
            var model = _blghaber.Get(g=>g.Baslik.Contains(term));
            var result = model.Select(s => new { baslik = s.Baslik, id = s.Id }).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult KayitSil(string cbSecili="")
        {
            if (cbSecili !="")
            {
                var parcala = cbSecili.Split(',').Select(Int32.Parse).ToList();
                foreach (var item in parcala)
                {
                    var model = _blghaber.GetById(item);
                    if (System.IO.File.Exists(HttpContext.Request.PhysicalApplicationPath +"img/blghaber"+ model.Gorsel))
                    {
                        System.IO.File.Exists(HttpContext.Request.PhysicalApplicationPath + "img/blghaber" + model.Gorsel);
                    }

                    using (BlgHaberBLL del = new BlgHaberBLL())
                    {
                        del.Delete(item);
                    }
                }                
            }
            return RedirectToAction("Index","BlgHaber");
        }
        public ActionResult Detay(int Id=0)
        {
            var model = _blghaber.GetById(Id);
            ViewBag.Mesaj = GenelAraclarBLL.KayitYeni();
            return View(model);
        }
        [HttpPost]
        public ActionResult Detay(BlgHaber model, HttpPostedFileBase fGorsel)
        {
            try
            {
                if (fGorsel != null && fGorsel.ContentLength >0)
                {
                    model.Gorsel = fGorsel.FileName;
                    fGorsel.SaveAs(HttpContext.Request.PhysicalApplicationPath + "img/blghaber/" + fGorsel.FileName);
                }
                _blghaber.InsertOrUpdate(model,model.Id);
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