using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinClean.Service.DTO.Order
{
    public class CancelOrderDTO
    {
        public int OrderId { get; set; }
        public int? CancelBy { get; set; }
        public string? ReasonCancel { get; set; }
    }
}
