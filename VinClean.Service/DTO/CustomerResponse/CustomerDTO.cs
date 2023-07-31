using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinClean.Service.DTO.CustomerResponse
{
    public class CustomerDTO
    {
        public int CustomerId { get; set; } 

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public decimal TotalMoney { get; set; }
        public int TotalPoint { get; set; }
        public string Status { get; set; }
        public virtual AccountdDTO Account { get; set; }

    }
}
