using System.ComponentModel.DataAnnotations;

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
