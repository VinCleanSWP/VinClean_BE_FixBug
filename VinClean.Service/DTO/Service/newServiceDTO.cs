using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinClean.Service.DTO.Service
{
    public class newServiceDTO
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Cost is required")]
        public decimal Cost { get; set; }

        public string Description { get; set; }
        [Required(ErrorMessage = "TypeID is required")]
        public int TypeId { get; set; }
        public byte MinimalSlot { get; set; }
    }
}
