using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinClean.Service.DTO.Type
{
    public class TypeDTO
    {
        
        public int TypeId { get; set; }
        [Required]
        public string? Type1 { get; set; }
        public string Img { get; set; }
        public bool? Avaiable { get; set; }
        
    }
}
