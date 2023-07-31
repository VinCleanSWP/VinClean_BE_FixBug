using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinClean.Service.DTO.Order
{
    public class OrderRangeDTO
    {
        public int? OrderId { get; set; }

        public int ServiceName { get; set; }
        public DateTime? DateWork { get; set; }
        public decimal Total { get; set; }
    }
}
