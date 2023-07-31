using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinClean.Service.DTO.Account
{
    public class LoginDTO
    {
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email format")]
        public string Email { get; set; }
        [Required]
        [MinLength(5, ErrorMessage = "Password can not be less than 5 characters")]
        [MaxLength(50, ErrorMessage = "Password too long")]
        public string Password { get; set; }
      
    }
}
