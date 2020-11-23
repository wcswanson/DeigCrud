using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeigCrud.Models
{
    public class TownDistrictModel
    {
        [Key]
        public int TownId { get; set; }
        public int District { get; set; }
        public string Town { get; set; }
    }
}
