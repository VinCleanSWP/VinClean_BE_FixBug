using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinClean.Repo.Models;
using VinClean.Service.DTO.CustomerResponse;

// Data holder

namespace VinClean.Service.DTO
{
    public class OrderDTO
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public string Status { get; set; }
        public string Note { get; set; }
        public bool isDelete { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int ModifiedBy { get; set; }
        public TimeSpan StartWorking { get; set; }
        public TimeSpan EndWorking { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StarTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public virtual CustomerDTO Customer { get; set; }
    }
}
