using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinClean.Service.DTO.Account;

namespace VinClean.Service.DTO.CustomerResponse
{
    public class Customer_processDTO
    {
        public int CustomerId { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public virtual Account_processDTO Account { get; set; }
    }
}
