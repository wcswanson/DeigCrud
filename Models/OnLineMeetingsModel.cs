using System.ComponentModel.DataAnnotations;

// https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-mvc-app/validation?view=aspnetcore-5.0
namespace DeigCrud.Models
{
    public class OnlineMeetingsModel
    {
        [Key]
        public int zoomid { get; set; }
        public int dayid { get; set; }
        public string day { get; set; }
        public int timeid { get; set; }
        public string time { get; set; }

        [Display(Name = "Meeting Id")]
        [StringLength(11, MinimumLength = 3)]

        public int District { get; set; }       

        [Required]
        public string meetingid { get; set; }
        [Display(Name = "Password")]
        [StringLength(6)]
        public string pswd { get; set; }
        public string telephone { get; set; }
        [Required]
        public string groupname { get; set; }
        public string notes { get; set; }
    }
}
