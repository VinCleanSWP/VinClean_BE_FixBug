using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinClean.Service.DTO.CustomerResponse
{
    public class RegisterDTO
    {
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinLength(5, ErrorMessage = "Password can not be less than 5 characters")]
        [MaxLength(50, ErrorMessage = "Password too long")]
        public string Password { get; set; }
        [Required, Compare("Password")]
        public string ConfirmPassword { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Address { get; set; }
    }
}
