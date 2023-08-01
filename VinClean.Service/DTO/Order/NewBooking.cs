using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinClean.Service.DTO.Process
{
    public class NewBooking
    {
        [Required]
        public int CustomerId { get; set; }
        [Required]
        public TimeSpan? StarTime { get; set; }
        [Required]
        public DateTime? Date { get; set; }
        [Required]
        public int ServiceId {get; set; }
        [Required]
        public string Address { get; set; }
        public int BuildingId { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public decimal Price { get; set; }
        public int PointUsed { get; set; }

        public string Note { get; set; } 

    }
}
