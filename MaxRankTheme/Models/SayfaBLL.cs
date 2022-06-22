using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MaxRankTheme.n2;

namespace MaxRankTheme.Models
{
    public class SayfaBLL: GenericRepository<Sayfa>
    {
    }
    public partial class Sayfa
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