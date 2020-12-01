using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DeigCrud.Models
{
    // Use spList_get parms: DOWID, TimeID, Town, Suspend  -- default null

    public class MeetingListModel
    {
        [BindProperty]
        [Key]
        public int ListID { get; set; }
        [BindProperty]
        public int DOW { get; set; }
        [BindProperty]
        public int District { get; set; }
        [BindProperty]
        public string Day { get; set; }
        [BindProperty]
        public int TimeID { get; set; }
        [BindProperty]
        public string Time { get; set; }
        [BindProperty]
        public string Town { get; set; }
        [BindProperty]
        public string GroupName { get; set; }
        [BindProperty]
        public string Information { get; set; }
        [BindProperty]
        public string Location { get; set; }
        [BindProperty]
        public string Type { get; set; }
        [BindProperty]
        public bool suspend { get; set; }
    }
}
