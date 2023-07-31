using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinClean.Repo.Models;
using VinClean.Service.DTO.Service;

// Data holder

namespace VinClean.Service.DTO.Process
{
    public class ProcessDetailDTO
    {
        public int ProcessId { get; set; }

        public int ServiceId { get; set; }

        public virtual OrderDTO Process { get; set; }

        public virtual ServiceDTO Service { get; set; }
    }
}
