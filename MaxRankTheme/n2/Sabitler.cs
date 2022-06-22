using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MaxRankTheme.n2
{
    public class Sabitler
    {
        public const int BİLGİ_INFO = 1;
        public const int BİLGİ_SUCCESS = 2;      
        public const int BİLGİ_DANGER = 3;


        #region Sayfa Durumları
        public const int DURUMU_AKTIF = 1;
        public const int DURUMU_PASIF = 0;

        public static string DURUMU(int Key)
        {
            switch (Key)
            {
                case 1: return "AKTİF";
                case 0: return "PASİF";
                default: return "HATA";
            }
        }
        public static string DURUMUhtml(int Key)
        {
            switch (Key)
            {
                case 1: return "<span class='text text-success'>AKTİF</span>";
                case 0: return "<span class='text text-danger'>PASİF</span>";
                default: return "<span class='text text-warning'>HATA</span>";
            }
        }
        public static string DURUMUhtml(bool Key)
        {
            switch (Key)
            {
                case true: return "<span class='text text-success'>AKTİF</span>";
                case false: return "<span class='text text-danger'>PASİF</span>";
                default: return "<span class='text text-warning'>HATA</span>";
            }
        }
        #endregion
    }
}