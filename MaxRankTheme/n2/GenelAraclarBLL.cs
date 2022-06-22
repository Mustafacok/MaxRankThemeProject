using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Web;
using System.Text.RegularExpressions;
using System.IO;
using System.Net.Mail;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Collections;
using System.Runtime.Serialization;
using System.Web.Configuration;
using System.Web.Routing;
using System.Xml;
using MaxRankTheme.Models;

namespace MaxRankTheme.n2
{
    public class GenelAraclarBLL
    {
        #region Resim Yükleme Kodları
        public static string CropperSaveImage(string filename, string klasor, string uzanti = "png")
        {
            string GorselYuklenmeDurumu = HttpContext.Current.Request.Form["GorselYuklenmeDurumu"];
            if (GorselYuklenmeDurumu == "1")
            {
                // baslıklar aynımı kontrol ediliyor     
                string GorselBaslik = HttpContext.Current.Request.Form["GorselBaslik"];
                string GorselBilgisi = HttpContext.Current.Request.Form["GorselBilgisi"];
                if (System.IO.File.Exists(HttpContext.Current.Server.MapPath("~/files/" + klasor + "/" + GorselBilgisi)))
                {
                    System.IO.File.Delete(HttpContext.Current.Server.MapPath("~/files/" + klasor + "/" + GorselBilgisi));
                }

                bool sonuc = true;
                int sayac = 0;
                string kisaad = GenelAraclarBLL.Duzelt(filename);
                while (sonuc)
                {
                    if (sayac == 0)
                    {
                        if (System.IO.File.Exists(HttpContext.Current.Server.MapPath("~/files/" + klasor + "/" + kisaad + "." + uzanti)))
                        {
                            sayac++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        if (System.IO.File.Exists(HttpContext.Current.Server.MapPath("~/files/" + klasor + "/" + kisaad + sayac + "." + uzanti)))
                        {
                            sayac++;
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                string file = "";
                if (sayac == 0)
                {
                    file = klasor + "/" + kisaad + "." + uzanti;
                    filename = kisaad + "." + uzanti;
                }
                else
                {
                    file = klasor + "/" + kisaad + sayac + "." + uzanti;
                    filename = kisaad + sayac + "." + uzanti;
                }


                string base64 = HttpContext.Current.Request.Form["imgCropped"];
                byte[] bytes = Convert.FromBase64String(base64.Split(',')[1]);
                using (System.IO.FileStream stream = new System.IO.FileStream(HttpContext.Current.Server.MapPath("~/files/" + file), System.IO.FileMode.Create))
                {
                    stream.Write(bytes, 0, bytes.Length);
                    stream.Flush();
                }
                return filename;
            }
            else
            {
                string GorselBilgisi = HttpContext.Current.Request.Form["GorselBilgisi"];
                return GorselBilgisi;
            }
        }
        #endregion

        #region HTML ÖZET ALMA
        public static string OtomatikHTMLOzetAl(string source, int miktar)
        {
            string result;
            if (source == null || source == "")
            {
                return "";
            }
            result = source.Replace("\r", " ");
            result = result.Replace("\n", " ");
            result = result.Replace("\t", string.Empty);

            result = System.Text.RegularExpressions.Regex.Replace(result, @"( )+", " ");
            result = System.Text.RegularExpressions.Regex.Replace(result, @"<( )*head([^>])*>", "<head>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result, @"(<( )*(/)( )*head( )*>)", "</head>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result, "(<head>).*(</head>)", string.Empty, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result, @"<( )*script([^>])*>", "<script>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result, @"(<( )*(/)( )*script( )*>)", "</script>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result, @"(<script>).*(</script>)", string.Empty, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result, @"<( )*style([^>])*>", "<style>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result, @"(<( )*(/)( )*style( )*>)", "</style>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result, "(<style>).*(</style>)", string.Empty, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result, @"<( )*td([^>])*>", "\t", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result, @"<( )*br( )*>", "\r", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result, @"<( )*li( )*>", "\r", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result, @"<( )*div([^>])*>", "\r\r", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result, @"<( )*tr([^>])*>", "\r\r", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result, @"<( )*p([^>])*>", "\r\r", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            result = System.Text.RegularExpressions.Regex.Replace(result, @"<[^>]*>", string.Empty, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result, @" ", " ", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result, @"&bull;", " * ", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            result = System.Text.RegularExpressions.Regex.Replace(result, @"&ccedil;", "ç");
            result = System.Text.RegularExpressions.Regex.Replace(result, @"&uuml;", "ü");
            result = System.Text.RegularExpressions.Regex.Replace(result, @"&ouml;", "ö");
            result = System.Text.RegularExpressions.Regex.Replace(result, @"&Ccedil;", "Ç");
            result = System.Text.RegularExpressions.Regex.Replace(result, @"&Uuml;", "Ü");
            result = System.Text.RegularExpressions.Regex.Replace(result, @"&Ouml;", "Ö");

            result = System.Text.RegularExpressions.Regex.Replace(result, @"&lsaquo;", "<", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result, @"&rsaquo;", ">", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result, @"&trade;", "(tm)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result, @"&frasl;", "/", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result, @"&lt;", "<", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result, @"&gt;", ">", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result, @"&copy;", "(c)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result, @"&reg;", "(r)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result, @"&(.{2,6});", string.Empty, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = result.Replace("\n", "\r");
            result = System.Text.RegularExpressions.Regex.Replace(result, "(\r)", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result, "(\t)", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result, "(\n)", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            result = result.Replace("   ", " ");

            result = result.Trim();

            if (result.Length > miktar)
            {
                result = result.Substring(0, miktar - 1) + "[...]";
            }
            return result;
        }
        public static string UpperFirst(string s)
        {
            return Regex.Replace(s, @"\b[a-z]\w+", delegate(Match match)
            {
                string v = match.ToString();
                return char.ToUpper(v[0]) + v.Substring(1);
            });
        }
        #endregion

        #region Harf Düzelt (Türkeç Karakter değişimi)

        public static string Duzelt(string gelen = "")
        {
            gelen = gelen.ToLower();
            gelen = gelen.Replace("ç", "c");
            gelen = gelen.Replace("ş", "s");
            gelen = gelen.Replace("ğ", "g");
            gelen = gelen.Replace("ü", "u");
            gelen = gelen.Replace("ö", "o");
            gelen = gelen.Replace("ı", "i");

            gelen = Regex.Replace(gelen, @"[^a-z0-9\s-]", "-"); // invalid chars           
            gelen = Regex.Replace(gelen, @"\s+", " ").Trim(); // convert multiple spaces into one space   
            gelen = gelen.Substring(0, gelen.Length <= 150 ? gelen.Length : 150).Trim(); // cut and trim it   
            gelen = Regex.Replace(gelen, @"\s", "-"); // hyphens   

            gelen = gelen.Replace("naci25feyza", "/");

            return gelen;
        }

        public static string DonusumYap(string gelen = "")
        {
            gelen = gelen
                .Replace('Ç', 'C')
                .Replace('Ö', 'O')
                .Replace('Ş', 'S')
                .Replace('İ', 'I')
                .Replace('Ü', 'U')
                .Replace('Ğ', 'G')
                .Replace('ç', 'c')
                .Replace('ö', 'o')
                .Replace('ş', 's')
                .Replace('ı', 'i')
                .Replace('ü', 'u')
                .Replace('ğ', 'g');

            return gelen;
        }
        #endregion

        #region Mail İşlemleri
        public static bool MailGonder(string kimemail, string kimeadi, string mailkonu, string mailicerik)
        {
            string uyeSMTPHOST = System.Configuration.ConfigurationManager.AppSettings["mailSMTPHOST"];
            string uyeGonderen = System.Configuration.ConfigurationManager.AppSettings["mailAdres"];
            string uyeGonderenAciklama = System.Configuration.ConfigurationManager.AppSettings["mailBaslik"];
            string uyeKullaniciName = System.Configuration.ConfigurationManager.AppSettings["mailAdres"];
            string uyePassword = System.Configuration.ConfigurationManager.AppSettings["mailSifre"];

            MailAddress From = new MailAddress(uyeGonderen, uyeGonderenAciklama); // gönderen kısmında görünen mail adresi
            MailAddress To = new MailAddress(kimemail, kimeadi); // mailin gönderileceği adres
            MailMessage Email = new MailMessage(From, To);
            Email.IsBodyHtml = true;
            Email.Subject = mailkonu;
            Email.Body = mailicerik;
            SmtpClient MailClient = new SmtpClient();
            MailClient.Port = 587;
            MailClient.Host = uyeSMTPHOST; // mail gönderme işlemi yapılacak mail sunucusunun adresi burası gmail için smtp.gmail.com olacak
            MailClient.EnableSsl = false; // Gmail üzerinden gönderme yapacaksanız veya sunucunuz kimlik doğrulaması gerektiriyorsa buraya true değerini vereceğiz
            MailClient.UseDefaultCredentials = true;
            MailClient.Credentials = new System.Net.NetworkCredential(uyeKullaniciName, uyePassword); // maili göndereceğiniz hesap bilgilerinizde buraya giriyoruz. Mailimiz bu hesap üzerinden gönderilecek.
            MailClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            try
            {
                MailClient.Send(Email);
                return true;
            }
            catch (Exception err)
            {
                return false;
            }
        }
        #endregion

        #region Sayfalama İşlemleri
        public static string PageSize(string sayfaadi = "")
        {

            string _opt = @"<a class='btn btn-primary btn-xs' href='#' title='Sayfa Başına Kayıt Sayısı' data-toggle='dropdown'><i class='fa fa-list-ol'>";
            string query = HttpContext.Current.Request.Url.Query;
            if (HttpContext.Current.Request.QueryString["Size"] != null)
            {
                int pc = 25;
                try
                {
                    pc = Convert.ToInt32(HttpContext.Current.Request.QueryString["Size"]);
                }
                catch (Exception)
                {
                    pc = 25;
                }
                _opt += pc + "</i></a><ul class='dropdown-menu' role='menu' style='width: 50px;min-width: 50px;'>";



                string pagequery = HttpContext.Current.Request.QueryString["Size"];

                _opt += "<li><a href='" + query.Replace("Size=" + pagequery, "Size=25") + "'>25</a></li>";
                _opt += "<li><a href='" + query.Replace("Size=" + pagequery, "Size=50") + "'>50</a></li>";
                _opt += "<li><a href='" + query.Replace("Size=" + pagequery, "Size=100") + "'>100</a></li>";
                _opt += "<li><a href='" + query.Replace("Size=" + pagequery, "Size=250") + "'>250</a></li>";
                _opt += "<li><a href='" + query.Replace("Size=" + pagequery, "Size=500") + "'>500</a></li>";
            }
            else
            {
                _opt += "25</i></a><ul class='dropdown-menu' role='menu' style='width: 50px;min-width: 50px;'>";
                if (HttpContext.Current.Request.QueryString.Count == 0)
                {
                    _opt += "<li><a href='" + query + "?Size=25" + "'>25</a></li>";
                    _opt += "<li><a href='" + query + "?Size=50" + "'>50</a></li>";
                    _opt += "<li><a href='" + query + "?Size=100" + "'>100</a></li>";
                    _opt += "<li><a href='" + query + "?Size=250" + "'>250</a></li>";
                    _opt += "<li><a href='" + query + "?Size=500" + "'>500</a></li>";
                }
                else
                {
                    _opt += "<li><a href='" + query + "&Size=25" + "'>25</a></li>";
                    _opt += "<li><a href='" + query + "&Size=50" + "'>50</a></li>";
                    _opt += "<li><a href='" + query + "&Size=100" + "'>100</a></li>";
                    _opt += "<li><a href='" + query + "&Size=250" + "'>250</a></li>";
                    _opt += "<li><a href='" + query + "&Size=500" + "'>500</a></li>";
                }
            }
            if (string.IsNullOrEmpty(sayfaadi))
            {
                string _deger = @"<div class='table-toolbar'>
                                <div class='col-md-12'>
                                    <div class='pull-left col-md-6' style='padding: 0px;'>
                                        <div class='input-group'>
                                            <span class='input-group-btn-xs list-head-items'>
                                                <input class='form-control' style=' height:34px; width:250px;' type='text' placeholder='En az üç karekter giriniz...' name='text' id='text' value='{0}'/>
                                                <button id='btnAra' type='button' class='btn btn-primary btn-xs'><i class='mdi mdi-search-web'></i></button>
                                                {2}
                                            </span>
                                        </div>
                                    </div>
                                    <div class='list-head-items'>
                                        {1}
                                    </div>
                                </div>
                        </div>";

                string _anahtarkelime = "";
                string _arama = "";
                if (HttpContext.Current.Request.QueryString["text"] != null)
                {
                    _anahtarkelime = HttpContext.Current.Request.QueryString["text"];
                    _arama = "<a class='btn default' href='{0}'>Vazgeç</a>";
                    _arama = string.Format(_arama, HttpContext.Current.Request.Path);
                }
                _deger = string.Format(_deger, _anahtarkelime, _opt + "</ul>", _arama);

                return _deger;
            }
            else
            {
                string _deger = @"<div class='table-toolbar'>
                                    <div class='col-md-12'>
                                        <div class='pull-left col-md-6' style='padding: 0px;'>
                                            <div class='input-group'>
                                                <span class='input-group-btn-xs list-head-items'>
                                                <input class='form-control' style=' height:34px; width:250px;' data-sayfaadi ='{3}' type='text' placeholder='En az üç karekter giriniz...' name='text' id='text' value='{0}'/>
                                                <button id='btnAra' type='button' class='btn btn-primary btn-xs'><i class='mdi mdi-search-web'></i></button>
                                                {2}
                                                </span>
                                            </div>
                                        </div>
                                    <div class='pull-right'>
                                        {1}
                                    </div>
                                </div>
                        </div>"; 
                 
                string _anahtarkelime = "";
                string _arama = "";
                if (HttpContext.Current.Request.QueryString["text"] != null)
                {
                    _anahtarkelime = HttpContext.Current.Request.QueryString["text"];
                    _arama = "<a class='btn default' href='{0}'>Vazgeç</a>";
                    _arama = string.Format(_arama, HttpContext.Current.Request.Path);
                }
                _deger = string.Format(_deger, _anahtarkelime, _opt + "</ul>", _arama,sayfaadi);

                return _deger;
            }
            

        }
        public static string ElemanBilgisi(int count)
        {

            if (count == 0)
                return "Sistemde kayıt bulunmamaktadır.";


            int Page = 1;
            int Size = 25;

            if (HttpContext.Current.Request["Page"] != null)
            {
                try
                {
                    Page = Convert.ToInt32(HttpContext.Current.Request["Page"].ToString());
                }
                catch (Exception)
                {
                }
            }



            if (HttpContext.Current.Request["Size"] != null)
            {
                try
                {
                    Size = Convert.ToInt32(HttpContext.Current.Request["Size"].ToString());
                }
                catch (Exception)
                {
                }
            }

            if (Page == 1)
            {
                if (count < Size)
                {
                    return "1 ile " + count + " arasındaki kayıtlar.Toplam " + count + " kayıt bulunmaktadır.";
                }
                else
                {
                    return "1 ile " + Size + " arasındaki kayıtlar.Toplam " + count + " kayıt bulunmaktadır.";
                }
            }
            else
            {
                if (Page * Size > count)
                {
                    return (((Page - 1) * Size) + 1) + " ile " + count + " arasındaki kayıtlar.Toplam " + count + " kayıt bulunmaktadır.";
                }
                else
                {
                    return (((Page - 1) * Size) + 1) + " ile " + (Page * Size) + " arasındaki kayıtlar.Toplam " + count + " kayıt bulunmaktadır.";
                }
            }
        }
        public static string SayfalamaYap(string url, int Count, string Sort = "Id", string Order = "desc", int Page = 1, int Size = 25)
        {
            try
            {
                if (HttpContext.Current.Request["Sort"] != null) { Sort = HttpContext.Current.Request["Sort"]; }
                if (HttpContext.Current.Request["Order"] != null) { Order = HttpContext.Current.Request["Order"]; }
                if (HttpContext.Current.Request["Page"] != null) { Page = Convert.ToInt32(HttpContext.Current.Request["Page"].ToString()); }
                if (HttpContext.Current.Request["Size"] != null) { Size = Convert.ToInt32(HttpContext.Current.Request["Size"].ToString()); }
            }
            catch (Exception)
            {
            }

            Size = Size > 1 ? Size : 1;

            int _toplamsayfasayisi = 0;
            if (Count == 0) _toplamsayfasayisi = 1;


            if (Count % Size == 0)
            {
                _toplamsayfasayisi = Count / Size;
            }
            else
            {
                _toplamsayfasayisi = (Count / Size) + 1;
            }


            string linkler = "<ol class='pagination pagination-sm'>";

            if (_toplamsayfasayisi > 1)
            {
                string query = HttpContext.Current.Request.Url.Query;
                //+ query.Replace("Size=" + pagequery, "Size=25") 

                linkler += "<li class='disabled'><a onclick='return false;'>" + Page + "/" + _toplamsayfasayisi + "</a></li> ";
                if (Page > 1)
                {
                    //ilk sayfa linki için
                    string pagequery = HttpContext.Current.Request.QueryString["Page"];
                    linkler += "<li class='prev'><a href='" + query.Replace("Page=" + pagequery, "Page=1") + "'>İlk Sayfa</a></li>";
                    linkler += "<li class='prev'><a href='" + query.Replace("Page=" + pagequery, "Page=" + (Page - 1) + "") + "'><i class='fa fa-step-backward' style='font-size: 16px;'></i></a></li>";
                }

                if (Page + 6 < _toplamsayfasayisi)
                {
                    string pagequery = HttpContext.Current.Request.QueryString["Page"];
                    if (pagequery != null)
                    {
                        for (int i = ((Page - 3) < 1 ? 1 : Page - 3); i < Page + 3; i++)
                        {
                            if (i == Page)
                            {
                                linkler += "<li class='active'><a onclick='return false;'>" + i + "</a></li> ";
                            }
                            else
                            {
                                linkler += "<li><a href='" + query.Replace("Page=" + pagequery, "Page=" + i + "") + "'>" + i + "</a></li>";
                            }
                        }

                        linkler += "<li class='prev'><a href='" + query.Replace("Page=" + pagequery, "Page=" + (Page + 1) + "") + "'><i class='fa fa-step-forward' style='font-size: 16px;'></i></a></li>";
                        linkler += "<li class='prev'><a href='" + query.Replace("Page=" + pagequery, "Page=" + _toplamsayfasayisi + "") + "'>Son Sayfa</a></li>";
                    }
                    else
                    {
                        if (HttpContext.Current.Request.QueryString.Count == 0)
                        {
                            for (int i = ((Page - 3) < 1 ? 1 : Page - 3); i <= ((Page + 3) > _toplamsayfasayisi ? _toplamsayfasayisi : Page + 3); i++)
                            {
                                if (i == Page)
                                {
                                    linkler += "<li class='active'><a onclick='return false;'>" + i + "</a></li> ";
                                }
                                else
                                {
                                    linkler += "<li><a href='" + query + "?Page=" + i + "'>" + i + "</a></li>";
                                }
                            }
                            if (Page != _toplamsayfasayisi)
                            {
                                linkler += "<li class='prev'><a href='" + query + "?Page=" + (Page + 1) + "'><i class='fa fa-step-forward' style='font-size: 16px;'></i></a></li>";
                                linkler += "<li class='prev'><a href='" + query + "?Page=" + _toplamsayfasayisi + "'>Son Sayfa</a></li>";
                            }
                        }
                        else
                        {
                            for (int i = ((Page - 3) < 1 ? 1 : Page - 3); i <= ((Page + 3) > _toplamsayfasayisi ? _toplamsayfasayisi : Page + 3); i++)
                            {
                                if (i == Page)
                                {
                                    linkler += "<li class='active'><a onclick='return false;'>" + i + "</a></li> ";
                                }
                                else
                                {
                                    linkler += "<li><a href='" + query + "&Page=" + i + "'>" + i + "</a></li>";
                                }
                            }
                            if (Page != _toplamsayfasayisi)
                            {
                                linkler += "<li class='prev'><a href='" + query + "&Page=" + (Page + 1) + "'><i class='fa fa-step-forward' style='font-size: 16px;'></i></a></li>";
                                linkler += "<li class='prev'><a href='" + query + "&Page=" + _toplamsayfasayisi + "'>Son Sayfa</a></li>";
                            }
                        }
                    }
                }
                else
                {
                    string pagequery = HttpContext.Current.Request.QueryString["Page"];
                    if (pagequery != null)
                    {
                        for (int i = ((Page - 3) < 1 ? 1 : Page - 3); i <= ((Page + 3) > _toplamsayfasayisi ? _toplamsayfasayisi : Page + 3); i++)
                        {
                            if (i == Page)
                            {
                                linkler += "<li class='active'><a onclick='return false;'>" + i + "</a></li> ";
                            }
                            else
                            {
                                linkler += "<li><a href='" + query.Replace("Page=" + pagequery, "Page=" + i + "") + "'>" + i + "</a></li>";
                            }
                        }
                        if (Page != _toplamsayfasayisi)
                        {
                            linkler += "<li class='prev'><a href='" + query.Replace("Page=" + pagequery, "Page=" + (Page + 1) + "") + "'><i class='fa fa-step-forward' style='font-size: 16px;'></i></a></li>";
                            linkler += "<li class='prev'><a href='" + query.Replace("Page=" + pagequery, "Page=" + _toplamsayfasayisi + "") + "'>Son Sayfa</a></li>";
                        }
                    }
                    else
                    {
                        if (HttpContext.Current.Request.QueryString.Count == 0)
                        {
                            for (int i = ((Page - 3) < 1 ? 1 : Page - 3); i <= ((Page + 3) > _toplamsayfasayisi ? _toplamsayfasayisi : Page + 3); i++)
                            {
                                if (i == Page)
                                {
                                    linkler += "<li class='active'><a onclick='return false;'>" + i + "</a></li> ";
                                }
                                else
                                {
                                    linkler += "<li><a href='" + query + "?Page=" + i + "'>" + i + "</a></li>";
                                }
                            }
                            if (Page != _toplamsayfasayisi)
                            {
                                linkler += "<li class='prev'><a href='" + query + "?Page=" + (Page + 1) + "'><i class='fa fa-step-forward' style='font-size: 16px;'></i></a></li>";
                                linkler += "<li class='prev'><a href='" + query + "?Page=" + _toplamsayfasayisi + "'>Son Sayfa</a></li>";
                            }
                        }
                        else
                        {
                            for (int i = ((Page - 3) < 1 ? 1 : Page - 3); i <= ((Page + 3) > _toplamsayfasayisi ? _toplamsayfasayisi : Page + 3); i++)
                            {
                                if (i == Page)
                                {
                                    linkler += "<li class='active'><a onclick='return false;'>" + i + "</a></li> ";
                                }
                                else
                                {
                                    linkler += "<li><a href='" + query + "&Page=" + i + "'>" + i + "</a></li>";
                                }
                            }
                            if (Page != _toplamsayfasayisi)
                            {
                                linkler += "<li class='prev'><a href='" + query + "&Page=" + (Page + 1) + "'><i class='fa fa-step-forward' style='font-size: 16px;'></i></a></li>";
                                linkler += "<li class='prev'><a href='" + query + "&Page=" + _toplamsayfasayisi + "'>Son Sayfa</a></li>";
                            }
                        }

                    }

                }
                linkler += "<li class='prev'><form method='get' style='display: inline;'><input class='form-control' style='width: 35px; display:inline;height: 29px;' type='text' name='Page' value='"
                    + Page
                    + "'/><button type='submit' class='btn btn-primary' style='padding: 4px 10px;'><i class='fa fa-list'></i></button></form></li>";
            }
            return linkler + "</ol>";
        }
        public static string SayfalamaYapAjax(string Parametre, int Count, int Page = 1, int Size = 25)
        {
            Size = Size > 1 ? Size : 1;
            int _toplamsayfasayisi = 0;
            if (Count == 0) _toplamsayfasayisi = 1;
            if (Count % Size == 0)
            {
                _toplamsayfasayisi = Count / Size;
            }
            else
            {
                _toplamsayfasayisi = (Count / Size) + 1;
            }

            string linkler = "<ol class='pagination pagination-sm'>";

            if (_toplamsayfasayisi > 1)
            {
                linkler += "<li class='disabled'><a onclick='return false;'>" + Page + "/" + _toplamsayfasayisi + "</a></li> ";
                if (Page > 1)
                {
                    linkler += "<li class='prev'><a href='javascript:;' onclick=SayfalamaYap('" + Parametre + "','1','" + Size + "');>İlk Sayfa</a></li>";
                    linkler += "<li class='prev'><a href='javascript:;' onclick=SayfalamaYap('" + Parametre + "','" + (Page - 1) + "','" + Size + "');><i class='fa fa-step-backward' style='font-size: 16px;'></i></a></li>";
                }
                if (Page + 6 < _toplamsayfasayisi)
                {
                    for (int i = ((Page - 3) < 1 ? 1 : Page - 3); i < Page + 3; i++)
                    {
                        if (i == Page)
                        {
                            linkler += "<li class='active'><a onclick='return false;'>" + i + "</a></li> ";
                        }
                        else
                        {
                            linkler += "<li><a href='javascript:;' onclick=SayfalamaYap('" + Parametre + "','" + i + "','" + Size + "');>" + i + "</a></li>";
                        }
                    }

                    linkler += "<li class='prev'><a href='javascript:;' onclick=SayfalamaYap('" + Parametre + "','" + (Page + 1) + "','" + Size + "');><i class='fa fa-step-forward' style='font-size: 16px;'></i></a></li>";
                    linkler += "<li class='prev'><a href='javascript:;' onclick=SayfalamaYap('" + Parametre + "','" + _toplamsayfasayisi + "','" + Size + "');>Son Sayfa</a></li>";

                }
                else
                {
                    for (int i = ((Page - 3) < 1 ? 1 : Page - 3); i <= ((Page + 3) > _toplamsayfasayisi ? _toplamsayfasayisi : Page + 3); i++)
                    {
                        if (i == Page)
                        {
                            linkler += "<li class='active'><a onclick='return false;'>" + i + "</a></li> ";
                        }
                        else
                        {
                            linkler += "<li><a href='javascript:;' onclick=SayfalamaYap('" + Parametre + "','" + i + "','" + Size + "');>" + i + "</a></li>";
                        }
                    }

                    if (Page != _toplamsayfasayisi)
                    {
                        linkler += "<li class='prev'><a href='javascript:;' onclick=SayfalamaYap('" + Parametre + "','" + (Page + 1) + "','" + Size + "');><i class='fa fa-step-forward' style='font-size: 16px;'></i></a></li>";
                        linkler += "<li class='prev'><a href='javascript:;' onclick=SayfalamaYap('" + Parametre + "','" + _toplamsayfasayisi + "','" + Size + "');>Son Sayfa</a></li>";
                    }

                }
            }
            string sys = "";
            if (_toplamsayfasayisi > 1)
            {
                sys = "<div style='float:right;'><a style='line-height: 1.2;' class='btn btn-primary' href='#' title='Sayfa Başına Kayıt Sayısı' data-toggle='dropdown' aria-expanded='false'>"
                            + "<i class='fa fa-list-ol'>" + Size + "</i></a>"
                            + "<ul class='dropdown-menu' role='menu' style='width: 50px;min-width: 50px;'>"
                            + "<li><a href='javascript:;' onclick=SayfalamaYap('" + Parametre + "','1','25');>25</a></li>"
                            + "<li><a href='javascript:;' onclick=SayfalamaYap('" + Parametre + "','1','50');>50</a></li>"
                            + "<li><a href='javascript:;' onclick=SayfalamaYap('" + Parametre + "','1','100');>100</a></li>"
                            + "<li><a href='javascript:;' onclick=SayfalamaYap('" + Parametre + "','1','250');>250</a></li>"
                            + "</ul></div>";

            }
            string _deger = @"<table style='width: 100%;'>
                            <tr>
                                <td style='margin: 0px; padding: 0px;'>
                                    <div class='dataTables_paginate paging_bootstrap pagination' style='margin: 0px; padding: 0px;'>
                                        {0}
                                    </div>
                                </td>
                                <td>
                                   {1}
                                </td>
                            </tr>
                        </table>";

            _deger = string.Format(_deger, linkler + "</ol>", sys);
            return _deger;
        }
        public static int SayfadakiIlkKayitIndex(int Count, int Pg = 1, int Size = 25)
        {
            try
            {
                if (HttpContext.Current.Request["Page"] != null) { Pg = Convert.ToInt32(HttpContext.Current.Request["Page"].ToString()); }
                if (HttpContext.Current.Request["Size"] != null) { Size = Convert.ToInt32(HttpContext.Current.Request["Size"].ToString()); }
            }
            catch (Exception)
            {
            }
            if (Pg <= 1)
            {
                return Count;
            }
            else
            {
                return Count - ((Pg - 1) * Size);
            }
        }
        public static string SortLink(string name)
        {
            string url = "";
            string query = HttpContext.Current.Request.Url.Query;
            if (HttpContext.Current.Request.QueryString["Sort"] != null)
            {
                query = query.Replace("Sort=" + HttpContext.Current.Request.QueryString["Sort"], "Sort=" + name);
                if (HttpContext.Current.Request.QueryString["Order"] != null)
                {
                    if (HttpContext.Current.Request.QueryString["Order"] == "asc")
                    {
                        query = query.Replace("Order=" + HttpContext.Current.Request.QueryString["Order"], "Order=desc");
                    }
                    else
                    {
                        query = query.Replace("Order=" + HttpContext.Current.Request.QueryString["Order"], "Order=asc");
                    }
                }
                else
                {
                    query += "&Order=asc";
                }
            }
            else
            {
                if (HttpContext.Current.Request.QueryString.Count == 0)
                {
                    query += "?Sort=" + name + "&Order=asc";
                }
                else
                {
                    query += "&Sort=" + name + "&Order=asc";
                }
            }

            return query;
        }
        public static string SortCss(string name)
        {
            string css = "";
            if (HttpContext.Current.Request.QueryString["Sort"] != null && HttpContext.Current.Request.QueryString["Sort"].ToString() == name)
            {
                if (HttpContext.Current.Request.QueryString["Order"] == "asc")
                {
                    css = "_asc";
                }
                else
                {
                    css = "_desc";
                }
            }
            return css;
        }
        #endregion

        #region Ajan da Tarih Convert
        public static DateTime AjandaTarihAlgilama(string Tarih)//  Gelen veri Wed May 29 2013 00:00:00 GMT 0300 (GTB Yaz Saati) şeklinde gelecek  ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun','Jul', 'Aug', 'Sep','Oct', 'Nov', 'Dec']
        {
            string[] parcali = Tarih.Split(' ');
            int Ay = 0;
            int Gun = Convert.ToInt32(parcali[2]);
            int Yil = Convert.ToInt32(parcali[3]);

            //saat Dakika
            string[] saatparcali = parcali[4].Split(':');
            int Saat = Convert.ToInt32(saatparcali[0]);
            int Dakika = Convert.ToInt32(saatparcali[1]);

            switch (parcali[1].ToLower())
            {
                case "jan":
                    Ay = 1;
                    break;
                case "feb":
                    Ay = 2;
                    break;
                case "mar":
                    Ay = 3;
                    break;
                case "apr":
                    Ay = 4;
                    break;
                case "may":
                    Ay = 5;
                    break;
                case "jun":
                    Ay = 6;
                    break;
                case "jul":
                    Ay = 7;
                    break;
                case "aug":
                    Ay = 8;
                    break;
                case "sep":
                    Ay = 9;
                    break;
                case "oct":
                    Ay = 10;
                    break;
                case "nov":
                    Ay = 11;
                    break;
                case "dec":
                    Ay = 12;
                    break;

            }


            return new DateTime(Yil, Ay, Gun, Saat, Dakika, 0);
        }
        #endregion

        #region Kaydetme Uyarıları
        public static string KayitBasarili(string mesaj = "")
        {
            if (mesaj == "")
            {
                mesaj = "Kayıt İşlemi başarılı bir şekilde gerçekleştirildi.";
            }
            return "<div class='alert alert-success' id='checkbox-errors'><strong>" + mesaj + "</strong></div>";
        }
        public static string KayitYeni(string mesaj = "")
        {
            if (mesaj == " ")
            {
                return "";
            }
            if (mesaj == "")
            {
                mesaj = "<strong>!</strong> Lütfen gerekli alanları formatına uygun ve eksiksiz bir şekilde doldurunuz.";
            }
            return "<div class='alert alert-info' id='checkbox-errors'>" + mesaj + "</div>";
        }
        public static string KayitHatali(string mesaj = "")
        {
            if (mesaj == "")
            {
                mesaj = "Kayıt işlemi gerçekleştirilemedi.Lütfen ilgili alanları formatına uygun doldurunuz.";
            }
            return "<div class='alert alert-danger' id='checkbox-errors'><strong>" + mesaj + "</strong></div>";
        }
        #endregion

        #region Döviz İşlemleri
        public static string GrafikliDovizKuruBanknote(bool _usd = true, bool _euro = true, bool _pound = false)
        {
            // Bugün (en son iş gününe) e ait döviz kurları için
            string today = "http://www.tcmb.gov.tr/kurlar/today.xml";

            //// 14 Şubat 2013 e ait döviz kurları için
            //string anyDays = "http://www.tcmb.gov.tr/kurlar/201302/14022013.xml";

            var xmlDoc = new XmlDocument();
            xmlDoc.Load(today);

            // Xml içinden tarihi alma - gerekli olabilir
            DateTime exchangeDate = Convert.ToDateTime(xmlDoc.SelectSingleNode("//Tarih_Date").Attributes["Tarih"].Value);

            string _USD_ALIS = "<span class='label label-primary'><i class='fa fa-usd' aria-hidden='true'></i>EFEKTİF DOLAR ALIŞI: <span class='badge'>"
                + xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='USD']/BanknoteBuying").InnerXml +

                "</span>";
            string _USD_SATIS = " SATIŞI: <span class='badge'>" + xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='USD']/BanknoteSelling").InnerXml + "</span></span>";

            string _EURO_ALIS = "<span class='label label-primary'><i class='fa fa-eur' aria-hidden='true'></i>EFEKTİF EURO ALIŞI: <span class='badge'>" + xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='EUR']/BanknoteBuying").InnerXml + "</span>";
            string _EURO_SATIS = " SATIŞI: <span class='badge'>" + xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='EUR']/BanknoteSelling").InnerXml + "</span></span>";

            string _POUND_ALIS = "<span class='label label-primary'>EFEKTİF STERLİN ALIŞI: <span class='badge'>" + xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='GBP']/BanknoteBuying").InnerXml + "</span>";
            string _POUND_SATIS = " SATIŞI: <span class='badge'>" + xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='GBP']/BanknoteSelling").InnerXml + "</span></span>";

            string sonuc = "";

            if (_usd)
            {
                sonuc += _USD_ALIS + _USD_SATIS;
            }

            if (_euro)
            {
                sonuc += _EURO_ALIS + _EURO_SATIS;
            }
            if (_pound)
            {
                sonuc += _POUND_ALIS + _POUND_SATIS;
            }
            return sonuc;
        }
        public static string GrafikliDovizKuruBanknoteTumu()
        {
            // Bugün (en son iş gününe) e ait döviz kurları için
            string today = "http://www.tcmb.gov.tr/kurlar/today.xml";

            //// 14 Şubat 2013 e ait döviz kurları için
            //string anyDays = "http://www.tcmb.gov.tr/kurlar/201302/14022013.xml";

            var xmlDoc = new XmlDocument();
            xmlDoc.Load(today);

            // Xml içinden tarihi alma - gerekli olabilir
            DateTime exchangeDate = Convert.ToDateTime(xmlDoc.SelectSingleNode("//Tarih_Date").Attributes["Tarih"].Value);

            string _1_ALIS = "<span class='label label-primary'>EFEKTİF 1 ABD DOLARI ALIŞI: <span class='badge'>" + xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='USD']/BanknoteBuying").InnerXml + "</span>";
            string _1_SATIS = " SATIŞI: <span class='badge'>" + xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='USD']/BanknoteSelling").InnerXml + "</span></span>";

            string _2_ALIS = "<span class='label label-primary'>EFEKTİF 1 EURO ALIŞI: <span class='badge'>" + xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='EUR']/BanknoteBuying").InnerXml + "</span>";
            string _2_SATIS = " SATIŞI: <span class='badge'>" + xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='EUR']/BanknoteSelling").InnerXml + "</span></span>";

            string _3_ALIS = "<span class='label label-primary'>EFEKTİF 1 İNGİLİZ STERLİNİ ALIŞI: <span class='badge'>" + xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='GBP']/BanknoteBuying").InnerXml + "</span>";
            string _3_SATIS = " SATIŞI: <span class='badge'>" + xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='GBP']/BanknoteSelling").InnerXml + "</span></span>";

            string _4_ALIS = "<span class='label label-primary'>EFEKTİF 1 AVUSTRALYA DOLARI ALIŞI: <span class='badge'>" + xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='AUD']/BanknoteBuying").InnerXml + "</span>";
            string _4_SATIS = " SATIŞI: <span class='badge'>" + xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='AUD']/BanknoteSelling").InnerXml + "</span></span>";

            string _5_ALIS = "<span class='label label-primary'>EFEKTİF 1 DANİMARKA KRONU ALIŞI: <span class='badge'>" + xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='DKK']/BanknoteBuying").InnerXml + "</span>";
            string _5_SATIS = " SATIŞI: <span class='badge'>" + xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='DKK']/BanknoteSelling").InnerXml + "</span></span>";

            string _6_ALIS = "<span class='label label-primary'>EFEKTİF 1 İSVİÇRE FRANGI ALIŞI: <span class='badge'>" + xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='SEK']/BanknoteBuying").InnerXml + "</span>";
            string _6_SATIS = " SATIŞI: <span class='badge'>" + xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='SEK']/BanknoteSelling").InnerXml + "</span></span>";

            string _7_ALIS = "<span class='label label-primary'>EFEKTİF 1 KANADA DOLARI ALIŞI: <span class='badge'>" + xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='CAD']/BanknoteBuying").InnerXml + "</span>";
            string _7_SATIS = " SATIŞI: <span class='badge'>" + xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='CAD']/BanknoteSelling").InnerXml + "</span></span>";

            string _8_ALIS = "<span class='label label-primary'>EFEKTİF 1 KUVEYT DİNARI ALIŞI: <span class='badge'>" + xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='KWD']/BanknoteBuying").InnerXml + "</span>";
            string _8_SATIS = " SATIŞI: <span class='badge'>" + xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='KWD']/BanknoteSelling").InnerXml + "</span></span>";

            string _9_ALIS = "<span class='label label-primary'>EFEKTİF 1 NORVEÇ KRONU ALIŞI: <span class='badge'>" + xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='NOK']/BanknoteBuying").InnerXml + "</span>";
            string _9_SATIS = " SATIŞI: <span class='badge'>" + xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='NOK']/BanknoteSelling").InnerXml + "</span></span>";

            string _10_ALIS = "<span class='label label-primary'>EFEKTİF 1 SUUDİ ARABİSTAN RİYALİ ALIŞI: <span class='badge'>" + xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='SAR']/BanknoteBuying").InnerXml + "</span>";
            string _10_SATIS = " SATIŞI: <span class='badge'>" + xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='SAR']/BanknoteSelling").InnerXml + "</span></span>";

            string _11_ALIS = "<span class='label label-primary'>EFEKTİF 100 JAPON YENİ ALIŞI: <span class='badge'>" + xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='JPY']/BanknoteBuying").InnerXml + "</span>";
            string _11_SATIS = " SATIŞI: <span class='badge'>" + xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='JPY']/BanknoteSelling").InnerXml + "</span></span>";


            string sonuc = _1_ALIS + _1_SATIS + _2_ALIS + _2_SATIS + _3_ALIS + _3_SATIS + _4_ALIS + _4_SATIS + _5_ALIS + _5_SATIS + _6_ALIS + _6_SATIS + _7_ALIS + _7_SATIS +
                 _8_ALIS + _8_SATIS + _9_ALIS + _9_SATIS + _10_ALIS + _10_SATIS + _11_ALIS + _11_SATIS;

            return sonuc;
        }
        public static string GrafikliDovizKuruForex(bool _usd = true, bool _euro = true, bool _pound = false)
        {
            // Bugün (en son iş gününe) e ait döviz kurları için
            string today = "http://www.tcmb.gov.tr/kurlar/today.xml";

            //// 14 Şubat 2013 e ait döviz kurları için
            //string anyDays = "http://www.tcmb.gov.tr/kurlar/201302/14022013.xml";

            var xmlDoc = new XmlDocument();
            xmlDoc.Load(today);

            // Xml içinden tarihi alma - gerekli olabilir
            DateTime exchangeDate = Convert.ToDateTime(xmlDoc.SelectSingleNode("//Tarih_Date").Attributes["Tarih"].Value);

            string _USD_ALIS = "<span class='label label-primary'><i class='fa fa-usd' aria-hidden='true'></i>DOLAR ALIŞ: <span class='badge'>" + xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='USD']/ForexBuying").InnerXml + "</span>";
            string _USD_SATIS = " SATIŞ: <span class='badge'>" + xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='USD']/ForexSelling").InnerXml + "</span></span>";

            string _EURO_ALIS = "<span class='label label-primary'><i class='fa fa-eur' aria-hidden='true'></i>EURO ALIŞ: <span class='badge'>" + xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='EUR']/ForexBuying").InnerXml + "</span>";
            string _EURO_SATIS = " SATIŞ: <span class='badge'>" + xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='EUR']/ForexSelling").InnerXml + "</span></span>";

            string _POUND_ALIS = "<span class='label label-primary'>STERLİN ALIŞ: <span class='badge'>" + xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='GBP']/ForexBuying").InnerXml + "</span>";
            string _POUND_SATIS = " SATIŞ: <span class='badge'>" + xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='GBP']/ForexSelling").InnerXml + "</span></span>";

            string sonuc = "";

            if (_usd)
            {
                sonuc += _USD_ALIS + _USD_SATIS;
            }

            if (_euro)
            {
                sonuc += _EURO_ALIS + _EURO_SATIS;
            }
            if (_pound)
            {
                sonuc += _POUND_ALIS + _POUND_SATIS;
            }
            return sonuc;
        }
        public static string GrafikliDovizKuruForexTumu()
        {
            // Bugün (en son iş gününe) e ait döviz kurları için
            string today = "http://www.tcmb.gov.tr/kurlar/today.xml";

            //// 14 Şubat 2013 e ait döviz kurları için
            //string anyDays = "http://www.tcmb.gov.tr/kurlar/201302/14022013.xml";

            var xmlDoc = new XmlDocument();
            xmlDoc.Load(today);

            // Xml içinden tarihi alma - gerekli olabilir
            DateTime exchangeDate = Convert.ToDateTime(xmlDoc.SelectSingleNode("//Tarih_Date").Attributes["Tarih"].Value);

            string _1_ALIS = "<span class='label label-primary'>1 ABD DOLARI ALIŞI: <span class='badge'>" + xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='USD']/ForexBuying").InnerXml + "</span>";
            string _1_SATIS = " SATIŞI: <span class='badge'>" + xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='USD']/ForexSelling").InnerXml + "</span></span>";

            string _2_ALIS = "<span class='label label-primary'>1 EURO ALIŞI: <span class='badge'>" + xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='EUR']/ForexBuying").InnerXml + "</span>";
            string _2_SATIS = " SATIŞI: <span class='badge'>" + xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='EUR']/ForexSelling").InnerXml + "</span></span>";

            string _3_ALIS = "<span class='label label-primary'>1 İNGİLİZ STERLİNİ ALIŞI: <span class='badge'>" + xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='GBP']/ForexBuying").InnerXml + "</span>";
            string _3_SATIS = " SATIŞI: <span class='badge'>" + xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='GBP']/ForexSelling").InnerXml + "</span></span>";

            string _4_ALIS = "<span class='label label-primary'>1 AVUSTRALYA DOLARI ALIŞI: <span class='badge'>" + xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='AUD']/ForexBuying").InnerXml + "</span>";
            string _4_SATIS = " SATIŞI: <span class='badge'>" + xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='AUD']/ForexSelling").InnerXml + "</span></span>";

            string _5_ALIS = "<span class='label label-primary'>1 DANİMARKA KRONU ALIŞI: <span class='badge'>" + xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='DKK']/ForexBuying").InnerXml + "</span>";
            string _5_SATIS = " SATIŞI: <span class='badge'>" + xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='DKK']/ForexSelling").InnerXml + "</span></span>";

            string _6_ALIS = "<span class='label label-primary'>1 İSVİÇRE FRANGI ALIŞI: <span class='badge'>" + xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='SEK']/ForexBuying").InnerXml + "</span>";
            string _6_SATIS = " SATIŞI: <span class='badge'>" + xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='SEK']/ForexSelling").InnerXml + "</span></span>";

            string _7_ALIS = "<span class='label label-primary'>1 KANADA DOLARI ALIŞI: <span class='badge'>" + xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='CAD']/ForexBuying").InnerXml + "</span>";
            string _7_SATIS = " SATIŞI: <span class='badge'>" + xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='CAD']/ForexSelling").InnerXml + "</span></span>";

            string _8_ALIS = "<span class='label label-primary'>1 KUVEYT DİNARI ALIŞI: <span class='badge'>" + xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='KWD']/ForexBuying").InnerXml + "</span>";
            string _8_SATIS = " SATIŞI: <span class='badge'>" + xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='KWD']/ForexSelling").InnerXml + "</span></span>";

            string _9_ALIS = "<span class='label label-primary'>1 NORVEÇ KRONU ALIŞI: <span class='badge'>" + xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='NOK']/ForexBuying").InnerXml + "</span>";
            string _9_SATIS = " SATIŞI: <span class='badge'>" + xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='NOK']/ForexSelling").InnerXml + "</span></span>";

            string _10_ALIS = "<span class='label label-primary'>1 SUUDİ ARABİSTAN RİYALİ ALIŞI: <span class='badge'>" + xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='SAR']/ForexBuying").InnerXml + "</span>";
            string _10_SATIS = " SATIŞI: <span class='badge'>" + xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='SAR']/ForexSelling").InnerXml + "</span></span>";

            string _11_ALIS = "<span class='label label-primary'>1 JAPON YENİ ALIŞI: <span class='badge'>" + xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='JPY']/ForexBuying").InnerXml + "</span>";
            string _11_SATIS = " SATIŞI: <span class='badge'>" + xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='JPY']/ForexSelling").InnerXml + "</span></span>";


            string sonuc = _1_ALIS + _1_SATIS + _2_ALIS + _2_SATIS + _3_ALIS + _3_SATIS + _4_ALIS + _4_SATIS + _5_ALIS + _5_SATIS + _6_ALIS + _6_SATIS + _7_ALIS + _7_SATIS +
                 _8_ALIS + _8_SATIS + _9_ALIS + _9_SATIS + _10_ALIS + _10_SATIS + _11_ALIS + _11_SATIS;

            return sonuc;
        }
        public static string KurHesapla(decimal miktar, string yon = "Selling", string cinsi = "Banknote", string dovizkodu = "USD")
        {
            // Bugün (en son iş gününe) e ait döviz kurları için
            string today = "http://www.tcmb.gov.tr/kurlar/today.xml";

            var xmlDoc = new XmlDocument();
            xmlDoc.Load(today);

            DateTime exchangeDate = Convert.ToDateTime(xmlDoc.SelectSingleNode("//Tarih_Date").Attributes["Tarih"].Value);
            try
            {
                string _resultgenel = "";
                switch (dovizkodu)
                {
                    case "USD": _resultgenel += "ABD DOLARI"; break;
                    case "AUD": _resultgenel += "AVUSTRALYA DOLARI"; break;
                    case "DKK": _resultgenel += "DANİMARKA KRONU"; break;
                    case "EUR": _resultgenel += "EURO"; break;
                    case "GBP": _resultgenel += "İNGİLİZ STERLİNİ"; break;
                    case "CHF": _resultgenel += "İSVİÇRE FRANGI"; break;
                    case "SEK": _resultgenel += "İSVEÇ KRONU"; break;
                    case "CAD": _resultgenel += "KANADA DOLARI"; break;
                    case "KWD": _resultgenel += "KUVEYT DİNARI"; break;
                    case "NOK": _resultgenel += "NORVEÇ KRONU"; break;
                    case "SAR": _resultgenel += "SUUDİ ARABİSTAN RİYALİ"; break;
                    case "JPY": _resultgenel += "JAPON YENİ"; break;
                    default: break;
                }



                string _result = xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='" + dovizkodu + "']/" + cinsi + yon).InnerXml;
                string _sonuc = "";
                if (yon == "Selling")
                {
                    if (cinsi == "Banknote")
                    {
                        _sonuc = "<span class='label label-primary'>" + miktar + " " + _resultgenel + " EFEKTİF Satışa göre <span class='badge'>" + String.Format("{0:0.##}", (Convert.ToDecimal(_result) * miktar / 10000)) + "</span> Türk Lirası etmektedir.</span>";
                        _sonuc += "<br><span class='label label-danger'>1 " + _resultgenel + " EFEKTİF şatışa göre <span class='badge'>" + Convert.ToDecimal(Convert.ToDecimal(_result) / 10000).ToString() + "</span> Türk Lirası etmektedir.";
                    }
                    else
                    {
                        _sonuc = "<span class='label label-primary'>" + miktar + " " + _resultgenel + " Döviz Satışa göre <span class='badge'>" + String.Format("{0:0.##}", (Convert.ToDecimal(_result) * miktar / 10000)) + "</span> Türk Lirası etmektedir.</span>";
                        _sonuc += "<br><span class='label label-danger'>1 " + _resultgenel + " Döviz şatışa göre <span class='badge'>" + Convert.ToDecimal(Convert.ToDecimal(_result) / 10000).ToString() + "</span> Türk Lirası etmektedir.</span>";
                    }
                }
                else
                {
                    if (cinsi == "Banknote")
                    {
                        _sonuc = "<span class='label label-primary'>" + miktar + " Türk Lirası EFEKTİF Alışa göre <span class='badge'>" + String.Format("{0:0.####}", (miktar / Convert.ToDecimal(_result) * 10000)) + "</span> " + _resultgenel + " etmektedir.</span>";
                        _sonuc += "<br><span class='label label-danger'>1 " + _resultgenel + " EFEKTİF alışa göre <span class='badge'>" + Convert.ToDecimal(Convert.ToDecimal(_result) / 10000).ToString() + "</span> Türk Lirası etmektedir.</span>";
                    }
                    else
                    {
                        _sonuc = "<span class='label label-primary'>" + miktar + " Türk Lirası Döviz Alışa göre <span class='badge'>" + String.Format("{0:0.####}", (Convert.ToDecimal(miktar / Convert.ToDecimal(_result) * 10000))) + "</span> " + _resultgenel + " etmektedir.</span>";
                        _sonuc += "<br><span class='label label-danger'>1 " + _resultgenel + " Döviz alışa göre <span class='badge'>" + Convert.ToDecimal(Convert.ToDecimal(_result) / 10000).ToString() + "</span> Türk Lirası etmektedir.</span>";
                    }
                }
                return _sonuc;
            }
            catch (Exception)
            {
                return "";
            }
        }
        #endregion

        #region Üye İşlemleri
        public static Uye GetUye()
        {
            try
            {
                Uye u = (Uye)HttpContext.Current.Session["Uye"];
                UyeBLL _uye = new UyeBLL();

                if (u == null)
                {
                    HttpCookie ck = HttpContext.Current.Request.Cookies["Uye"];
                    if (ck != null && !string.IsNullOrEmpty(ck.Value))
                    {
                        int uyeID = Convert.ToInt32(ck.Value);

                        Uye kul2 = _uye.GetFirstOrDefault(d => d.Id == uyeID);
                        if (kul2 != null)
                            return kul2;
                    }
                }
                else
                    LoadUye(u);
                return u;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static bool LoadUye(Uye u)
        {
            try
            {
                HttpContext.Current.Session["Uye"] = u;
                HttpCookie ck = HttpContext.Current.Request.Cookies["Uye"];
                if (ck == null)
                    ck = new HttpCookie("usr", u.Id + "");
                if (ck.Expires < DateTime.Now)
                {
                    ck.Value = u.Id + "";
                    ck.Expires = DateTime.Now.AddDays(1);
                    //ck.Expires = DateTime.Now.AddMinutes(MesaiSaatleriKontrolu());
                    HttpContext.Current.Response.Cookies.Add(ck);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion
    }
}
