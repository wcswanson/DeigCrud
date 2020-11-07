using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;

namespace DeigCrud.Models
{
    // Define models -- in seperate files
    // Except for Suspend -- define it here
    // Follow the pattern for public properties: 
    //  1. Singular item var to put the selected value in: 'TownSelection'
    //  2. Plural items model name: TownModel

    // Deig list View Model
    public class DlViewModel
    {
        [TempData]
        public string FormResult { get; set; }
        public int ListIdSelect { get; set; }
        // Suspend
        [BindProperty]
        public string SuspendSelect { get; set; }
        // Plural
        public List<SelectListItem> Suspended = new List<SelectListItem>
        {            
            new SelectListItem { Value = "1", Text = "True" },
            new SelectListItem { Value = "0", Text = "False"  },
        };

        // Day of Week (DOW)
        public string DOWSelect { get; set; }
        public IEnumerable<SelectListItem> DOWModel { get; set; }

        // Time
        public string TimeSelect { get; set; }
        public IEnumerable<SelectListItem> TimeModel { get; set; }

        // Towns
        public string TownSelect { get; set; }
        public IEnumerable<SelectListItem> TownModel { get; set; }

        // Meeting list  use for display but can't receive the content so add *Select vars.      
       public IEnumerable< MeetingListModel> ListModel { get; set; }       
        public string GroupNameSelect { get; set; }
        public string InformationSelect { get; set; }
        public string LocationSelect { get; set; }
        public string TypeSelect { get; set; }

    }
}
