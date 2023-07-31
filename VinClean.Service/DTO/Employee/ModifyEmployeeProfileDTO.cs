using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinClean.Service.DTO.Employee
{
    public class ModifyEmployeeProfileDTO
    {
        public int EmployeeId { get; set; }

        public int AccountId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
    }

}

