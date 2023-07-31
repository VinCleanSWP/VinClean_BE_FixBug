using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinClean.Repo.Models;
using VinClean.Service.DTO.CustomerResponse;
using VinClean.Service.DTO.WorkingBy;
using VinClean.Service.DTO.WorkingSlot;

namespace VinClean.Service.DTO.Process
{
    public class ProcessInfo
    {
        public int ProcessId { get; set; }

        public int? CustomerId { get; set; }

        public string? Status { get; set; }

        public string? Note { get; set; }

        public bool? IsDeleted { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? ModifiedBy { get; set; }

        public virtual Customer_processDTO? Customer { get; set; }

        /// <summary> 
        /// 
        /// </summary>
        public virtual ICollection<ProcessDetail_processDTO> ProcessDetails { get; set; }
        public virtual ICollection<AddOrderRequest> ProcessSlots { get; set; }
        public virtual ICollection<Location_orderDTO> WorkingBies { get; set; }
    }
}
