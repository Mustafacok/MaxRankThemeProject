using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MaxRankTheme.n2;

namespace MaxRankTheme.Models
{
    public class SliderBLL:GenericRepository<Slider>
    {
    }
    public partial class Slider
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