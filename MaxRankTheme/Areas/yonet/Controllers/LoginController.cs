using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaxRankTheme.n2;
using MaxRankTheme.Models;

namespace MaxRankTheme.Areas.yonet.Controllers
{
    public class LoginController : Controller
    {
        // GET: yonet/Login
        UyeBLL _uye = new UyeBLL();
       
        public ActionResult Index()
        {
            if (Session["Uye"] != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        } 
        
        [HttpPost]
        public ActionResult Index(string Email, string Sifre)
        {
            if (string.IsNullOrEmpty(Email)||string.IsNullOrEmpty(Sifre))
            {
                ViewBag.Mesaj = "Lütfen Gerekli Alanları Doldurunuz.";
            }
            else
            {
                var kontrol = _uye.GetFirstOrDefault(g=>g.Sifre == Sifre && g.EMail== Email);
                if (kontrol == null)
                {
                    ViewBag.Mesaj = "Kullanıcı Bilgilerine Ulaşılamadı.";
                }
                else
                {
                    // yönlendirme yap.
                    GenelAraclarBLL.LoadUye(kontrol);
                    return RedirectToAction("Index","Home");
                }
            }
            return View();
        }        
    }
}