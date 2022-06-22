using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MaxRankTheme.n2;

namespace MaxRankTheme.Models
{
    public class ReferansBLL:GenericRepository<Referans>
    {
    }
    public partial class Referans
    {
        public string SEOLink
        {
            get
            {
                return GenelAraclarBLL.Duzelt(Baslik);
            }
        }
    }
}