using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinClean.Repo.Models;
using VinClean.Service.DTO.Account;
using VinClean.Service.DTO.Employee;
using VinClean.Service.DTO.Slot;

// Data holder

namespace VinClean.Service.DTO.Process
{
    public class OrderRequestDTO
    {
        public int ProcessId { get; set; }

        public int OldEmployeeId { get; set; }

        public int NewEmployeeId { get; set; }

        public string Note { get; set; }

        public string Satus { get; set; }

        public DateTime CreateAt { get; set; }

        public int CreateBy { get; set; }

        /*public virtual Account_processDTO CreateByNavigation { get; set; }*/

        public virtual EmployeeDTO NewEmployee { get; set; }

        public virtual EmployeeDTO OldEmployee { get; set; }

        public virtual OrderDTO Process { get; set; }
    }
}
