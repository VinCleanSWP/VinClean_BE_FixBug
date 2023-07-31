   using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinClean.Repo.Models;
using VinClean.Service.DTO.Slot;

namespace VinClean.Service.DTO.Process
{
    public class AddOrderRequest
    {
        public int? ProcessId { get; set; }
        public int? OldEmployeeId { get; set; }
        public string? Note { get; set; }
        public DateTime? CreateAt { get; set; }
        public int? CreateBy { get; set; }
    }
}
