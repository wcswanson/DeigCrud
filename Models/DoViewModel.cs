﻿// using DeigCrud.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace DeigCrud.Models
{
    // Deig online View Model
    public class DoViewModel
    {
        //todo: Add models to collect:
        //todo: DOW
        //todo: Time
        // Online list

        // Day of Week (DOW)
        public string DOWSelect { get; set; }
        public IEnumerable<SelectListItem> DOWModel { get; set; }

        // Time
        public string TimeSelect { get; set; }
        public IEnumerable<SelectListItem> TimeModel { get; set; }

        //District
        public int DistrictSelect { get; set; }
        public IEnumerable<SelectListItem> DistrictModel { get; set; }
        public string UrlSelect { get; set; }

        // To get the rest of the online meeting data:
        public int zoomidSelect { get; set; }
        public string meetingidSelect { get; set; }
        public string pswdSelect { get; set; }
        public string telephoneSelect { get; set; }
        public string groupnameSelect { get; set; }
        public string notesSelect { get; set; }

        // To get the onling meeting model
        public IEnumerable<OnlineMeetingsModel> OnlineListModel { get; set; }

    }
}
