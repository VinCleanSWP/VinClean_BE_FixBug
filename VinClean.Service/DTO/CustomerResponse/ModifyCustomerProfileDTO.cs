using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinClean.Service.DTO.CustomerResponse
{
    public class ModifyCustomerProfileDTO
    {
        public int CustomerId { get; set; }

        public int AccountId { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public int TotalPoint { get; set; }

    }
}
