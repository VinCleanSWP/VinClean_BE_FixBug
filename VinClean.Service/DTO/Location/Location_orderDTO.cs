using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinClean.Service.DTO.Employee;

namespace VinClean.Service.DTO.WorkingBy
{
    public class Location_orderDTO
    {
        public int EmployeeId { get; set; }
        public Employee_processDTO Employee { get; set; }
    }
}
