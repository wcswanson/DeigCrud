using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeigCrud.Models
{
    public class DTownDistrictViewModel
    {
        public int DistrictSelect { get; set; }
        public IEnumerable<SelectListItem> DistrictModel { get; set; }

        public int TownSelect { get; set; }
        public IEnumerable<SelectListItem> TownModel { get; set; }

        public string TownIdSelect { get;set;}
        public IEnumerable<TownDistrictModel> DistrictTownModel { get; set; }
    }
}
