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
        
        // Town Model includes TownId and Town name.
        public IEnumerable<SelectListItem> TownModel { get; set; }

        // This creates an alias for TownDistrictModel and gets TownId, District and Town.
        public int TownIdSelect { get;set;}
        public string TownNameSelect { get; set; }
        public IEnumerable<TownDistrictModel> DistrictTownModel { get; set; }
    }
}
