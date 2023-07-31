using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinClean.Repo.Models
{
    public class OrderRequestModel
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string Address { get; set; } 
        public int? AccountId { get; set; }
        public string OldEmployeeName { get; set; }
        public int OldEmployeeId { get; set; }
        public string OldEmployePhone { get; set; }
        public string OldEmployeEmail { get; set; }
        public string OldEmployeImg { get; set; }
        public int ServiceId { get; set; } 
        public DateTime? Date { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public string NewEmployeeName { get; set; }
        public int? NewEmployeeId { get; set; }
        public string NewEmployePhone { get; set; }
        public string NewEmployeEmail { get; set; }
        public string NewEmployeImg { get; set; }
        public string Status { get; set; }
        public string Reason { get; set; }
        public bool? IsDeleted { get; set; }
        public TimeSpan? StartWorking { get; set; }
        public TimeSpan? EndWorking { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
