using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinClean.Service.DTO.Email
{
    public class EmailFormDTO
    {
        [Required]
        public int OrderId { get; set; }
        [EmailAddress]
        public string To { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty; 
        public string Body { get; set; } = string.Empty;
    }
}
