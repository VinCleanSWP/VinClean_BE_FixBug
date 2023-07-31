using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinClean.Service.DTO.Account;

namespace VinClean.Service.DTO.Employee
{
    public class Employee_processDTO
    {

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Phone { get; set; }

        public string Status { get; set; }

        public int AccountId { get; set; }
        public Account_processDTO Account { get; set; }
    }
}
