using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace DeigCrud.Models
{
    public class OnlineMeetingsModel
    {
        [Key]
        public int zoomid { get; set; }
        public int dayid { get; set; }
        public string day { get; set; }
        public int timeid { get; set; }
        public string time {get; set;}
        public string meetingid { get; set; }
        public string pswd { get; set; }
        public string telephone { get; set; }
        public string groupname { get; set; }
        public string notes { get; set; }
    }
}
