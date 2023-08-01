using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinClean.Service.DTO.Service;

namespace VinClean.Service.DTO.Process
{
    public class ProcessDetail_processDTO
    {
        public int ServiceId { get; set; }
        public virtual Service_processDTO Service { get; set; }
    }
}
