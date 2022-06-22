using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MaxRankTheme.n2;

namespace MaxRankTheme.Models
{
    public class KategoriBLL:GenericRepository<Kategori>
    {
    }
    public partial class Kategori
    {
        public string SEOLink
        {
            get
            {
                return GenelAraclarBLL.Duzelt(Adi);
            }
        }
    }
}